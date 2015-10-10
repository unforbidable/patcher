/// Copyright(C) 2015 Unforbidable Works
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    class AsyncRecordFinder : IEnumerable<RecordEntry>
    {
        readonly RecordReader reader;
        readonly Options options;

        public AsyncRecordFinder(RecordReader reader, int recordCacheSize)
            : this(reader, new Options()
            {
                RecordCacheSize = recordCacheSize
            })
        {
        }

        private AsyncRecordFinder(RecordReader reader, Options options)
        {
            this.reader = reader;
            this.options = options;
        }

        public IEnumerator<RecordEntry> GetEnumerator()
        {
            return new Enumerator(reader, options);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(reader, options);
        }

        class Options
        {
            public int RecordCacheSize { get; set; }
        }

        class Enumerator : IEnumerator<RecordEntry>
        {
            readonly RecordReader reader;
            readonly Options options;
            RecordEntry current;
            Worker worker;

            public Enumerator(RecordReader reader, Options options)
            {
                this.reader = reader;
                this.options = options;
            }

            public RecordEntry Current
            {
                get { return current; }
            }

            public void Dispose()
            {
                if (worker != null)
                {
                    if (worker.IsBusy)
                    {
                        worker.CancelAsync();
                    }
                    worker.Dispose();
                    worker = null;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return current; }
            }

            public bool MoveNext()
            {
                if (worker == null)
                {
                    worker = new Worker(reader, options);
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerAsync();
                }

                if (current != null)
                {
                    worker.ReleaseEntry(current);
                }

                //Log.Fine("Moving to next entry");
                current = worker.GetNextEntry();
                return current != null;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            class Worker : BackgroundWorker, RecordReader.IFindRecordListener
            {
                readonly RecordReader reader;
                readonly Options options;
                readonly Queue<RecordEntry> foundRecords = new Queue<RecordEntry>();
                readonly AutoResetEvent foundRecordAdded = new AutoResetEvent(false);
                readonly Stack<RecordEntry> idleRecords = new Stack<RecordEntry>();
                readonly AutoResetEvent idleRecordAdded = new AutoResetEvent(false);
                bool done = false;

                int timesWaitedForIdleRecord = 0;

                public Worker(RecordReader reader, Options options)
                {
                    if (options.RecordCacheSize < 1)
                    {
                        throw new InvalidOperationException("Record cache size must be at least 1");
                    }

                    this.reader = reader;
                    this.options = options;

                    int recordsToCreate = options.RecordCacheSize;
                    while (recordsToCreate-- > 0)
                    {
                        idleRecords.Push(new RecordEntry());
                    }
                }

                protected override void OnDoWork(DoWorkEventArgs e)
                {
                    try
                    {
                        reader.FindRecords(this);
                    }
                    finally
                    {
                        done = true;
                        foundRecordAdded.Set();
                    }
                }

                protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
                {
                    //Log.Fine("Times waited for idle record: {0}", timesWaitedForIdleRecord);
                }

                public void OnRecordFound(string singature, RecordReader.FindRecordListenerArgs args)
                {
                    if (CancellationPending)
                    {
                        args.Action = RecordReader.FindRecordListenerAction.Cancel;
                        return;
                    }

                    while (idleRecords.Count == 0)
                    {
                        // Wait for idle recods to become available
                        idleRecordAdded.WaitOne();
                        timesWaitedForIdleRecord++;

                        if (CancellationPending)
                        {
                            args.Action = RecordReader.FindRecordListenerAction.Cancel;
                            return;
                        }
                    }

                    lock (idleRecords)
                    {
                        args.TargetEntry = idleRecords.Pop();
                    }
                }

                public void OnRecordEntry(RecordEntry entry)
                {
                    if (CancellationPending)
                        return;

                    lock (foundRecords)
                    {
                        foundRecords.Enqueue(entry);
                    }
                    foundRecordAdded.Set();
                }

                public RecordEntry GetNextEntry()
                {
                    while (foundRecords.Count == 0)
                    {
                        if (CancellationPending)
                            return null;

                        if (done)
                            return null;

                        foundRecordAdded.WaitOne();
                    }

                    lock (foundRecords)
                    {
                        return foundRecords.Dequeue();
                    }
                }

                public void ReleaseEntry(RecordEntry entry)
                {
                    lock (idleRecords)
                    {
                        idleRecords.Push(entry);
                    }
                    idleRecordAdded.Set();
                }
            }
        }
    }
}
