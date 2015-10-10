﻿/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Data.Plugins.Content;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Patcher.Data.Plugins
{
    sealed class FormLoader : IDisposable
    {
        readonly RecordReader stockReader;
        readonly bool asyncLoading;

        readonly SharedData sharedData;
        readonly List<Worker> workers = new List<Worker>();

        public long Loaded { get { return sharedData.Loaded; } }
        public long Skipped { get { return sharedData.Skipped; } }
        public long Unsupported { get { return sharedData.Unsupported; } }
        public long Supported { get { return sharedData.Loaded - sharedData.Unsupported; } }

        public FormLoader(Plugin plugin, RecordReader stockReader, bool lazyLoading, int backgroundJobs)
        {
            if (backgroundJobs < 0)
                throw new ArgumentException("Number of bakcground jobs must be a positive integer or zero");

            sharedData = new SharedData()
            {
                Plugin = plugin,
                StockReader = stockReader,
                LazyLoading = lazyLoading,
                FormsToLoad = new BlockingCollection<Form>(new ConcurrentQueue<Form>(), 1024),
                WorkerCompleteEvent = new AutoResetEvent(false)
            };

            this.stockReader = stockReader;
            asyncLoading = backgroundJobs > 0;

            bool useStockReader = true;
            while (backgroundJobs-- > 0)
            {
                Worker worker = new Worker(sharedData, useStockReader);
                worker.RunWorkerAsync();
                workers.Add(worker);

                // Only the first worker can use the stock reader
                useStockReader = false;
            }
        }

        public void LoadForm(Form form)
        {
            if (asyncLoading)
            {
                sharedData.FormsToLoad.Add(form);
            }
            else
            {
                LoadSingleForm(form, stockReader, sharedData);
            }
        }

        private static void LoadSingleForm(Form form, RecordReader reader, SharedData sharedData)
        {
            // Skip if record has been already loaded 
            // unless it has been loaded lazily and current loading scheme is lazy again
            if (form.Record != null && (!form.IsLazyLoaded || sharedData.LazyLoading))
            {
                Interlocked.Increment(ref sharedData.Skipped);
            }
            else
            {
                if (sharedData.LazyLoading)
                {
                    form.Flags |= FormFlags.LazyLoaded;
                }
                else
                {
                    form.Flags &= ~FormFlags.LazyLoaded;
                }

                form.Record = sharedData.Plugin.Context.CreateGenericFormRecord(form.FormKind);

                reader.ReadRecordAt(form.FilePosition, form.Record, sharedData.LazyLoading);

                if (form.IsDummyRecord)
                    Interlocked.Increment(ref sharedData.Unsupported);

                Interlocked.Increment(ref sharedData.Loaded);
            }
        }

        public void Complete()
        {
            if (!asyncLoading)
            {
                throw new InvalidOperationException("Cannot wait for completion during synchronous form loading");
            }

            sharedData.FormsToLoad.CompleteAdding();
        }

        public bool IsBusy
        {
            get
            {
                return workers.Any(w => w.IsBusy);
            }
        }

        public void WaitForCompleted()
        {
            if (!asyncLoading)
            {
                throw new InvalidOperationException("Cannot wait for completion during synchronous form loading");
            }

            while (IsBusy)
            {
                sharedData.WorkerCompleteEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            if (asyncLoading)
            {
                if (!sharedData.FormsToLoad.IsAddingCompleted)
                    Complete();
                
                WaitForCompleted();
            }

            foreach (Worker w in workers)
            {
                w.Dispose();
            }
        }

        class SharedData
        {
            public Plugin Plugin { get; set; }
            public RecordReader StockReader { get; set; }
            public bool LazyLoading { get; set; }
            public BlockingCollection<Form> FormsToLoad { get; set; }
            public AutoResetEvent WorkerCompleteEvent { get; set; }
            public long Loaded;
            public long Skipped;
            public long Unsupported;
        }

        class Worker : BackgroundWorker
        {
            readonly SharedData sharedData;
            readonly RecordReader reader;
            readonly bool usingStockReader;

            public Worker(SharedData sharedData, bool useStockReader)
            {
                this.sharedData = sharedData;

                usingStockReader = useStockReader;
                if (useStockReader)
                {
                    reader = sharedData.StockReader;
                }
                else
                {
                    // Create new reader of the same plugin file
                    Stream stream = sharedData.Plugin.Context.DataFileProvider.GetDataFile(FileMode.Open, sharedData.Plugin.FileName).Open();
                    reader = sharedData.Plugin.Context.CreateReader(stream);
                    reader.PluginFlags = sharedData.StockReader.PluginFlags;
                    reader.ReferenceMapper = sharedData.StockReader.ReferenceMapper;
                    reader.StringLocator = sharedData.StockReader.StringLocator;
                }
            }

            protected override void OnDoWork(DoWorkEventArgs e)
            {
                while (!sharedData.FormsToLoad.IsCompleted)
                {
                    Form form;
                    if (sharedData.FormsToLoad.TryTake(out form, -1))
                    {
                        LoadSingleForm(form, reader, sharedData);
                    }
                }
            }

            protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
            {
                sharedData.WorkerCompleteEvent.Set();

                if (!usingStockReader)
                {
                    reader.Dispose();
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (!usingStockReader)
                {
                    reader.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
