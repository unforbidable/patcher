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

using Patcher.Data.Plugins.Content;
using Patcher.Data.Strings;
using Patcher.IO;
using Patcher.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins
{
    public sealed class Plugin : IDisposable
    {
        readonly DataContext context;
        public DataContext Context { get { return context; } }

        string fileName = null;
        public string FileName { get { return fileName; } }

        //RecordReader reader = null;
        HeaderRecord header = null;

        public IEnumerable<Form> Forms { get { return context.Forms.OfPlugin(this); } }

        public string Author { get { return header.Author; } set { header.Author = value; } }
        public string Description { get { return header.Description; } set { header.Description = value; } }
        public IEnumerable<string> MasterFiles { get { return header.GetMasterFiles(); } }

        ISet<uint> reservedFormIds = new SortedSet<uint>();
        IDictionary<string, uint> reservedEditorIdMap = new SortedDictionary<string, uint>();

        // DataContext creates plugins
        internal Plugin(DataContext context, string fileName, PluginMode mode)
        {
            this.context = context;
            this.fileName = fileName;

            if (mode == PluginMode.Create)
            {
                header = context.CreateHeader();
                header.NextFormId = 0x800;
            }
            else if (mode == PluginMode.Open)
            {
                // Read only the header mainly to fetch the list of masters
                Stream stream = context.DataFileProvider.GetDataFile(FileMode.Open, fileName).Open();
                using (var reader = context.CreateReader(stream))
                {
                    header = reader.ReadHeader();
                }
            } 
            else
            {
                throw new ArgumentException("Illegal plugin mode specified");
            }
        }

        public void Load()
        {
            Log.Info("Indexing forms in plugin {0}", fileName);

            Stream stream = context.DataFileProvider.GetDataFile(FileMode.Open, fileName).Open();
            using (var reader = context.CreateReader(stream))
            {
                // Header needs to be read again
                header = reader.ReadHeader();

                // Prepare reference mapper
                reader.ReferenceMapper = new PluginReferenceMapper(this);

                int before = context.Forms.Count();
                int overriding = 0;
                int added = 0;

                using (var progress = Program.Status.StartProgress("Indexing forms"))
                {

                    if (context.AsyncFormIndexing)
                    {
                        // Retrieve plugin number so that loaded forms can be linked to it
                        byte pluginNumber = context.Plugins.GetPluginNumber(this);

                        foreach (var record in reader.FindRecordsAsync())
                        {
                            Form form = new Form()
                            {
                                PluginNumber = pluginNumber,
                                FilePosition = record.FilePosition,
                                FormKind = (FormKind)record.Signature,
                                FormId = reader.ReferenceMapper.LocalToContext(record.FormId),
                                ParentFormId = reader.ReferenceMapper.LocalToContext(record.ParentRecordFormId)
                            };

                            // Index loaded form
                            context.Forms.Add(form);

                            added++;
                            if (form.IsOverriding)
                                overriding++;

                            progress.Update(reader.TotalRecordsFound, header.NumRecords, "{0}: {1}", fileName, form.FormKind);
                        }
                    }
                    else
                    {
                        FindRecordsListener listener = new FindRecordsListener(this, reader, progress);
                        reader.FindRecords(listener);
                        added = listener.Added;
                        overriding = listener.Overriding;
                    }
                }

                Log.Info("Found {0} forms ({1} overrides)", added, overriding);

                //if (header.NumRecords != reader.TotalRecordsFound)
                //{
                //    Log.Warning("Number of records specified in header {0} does not match number of records found {1}", header.NumRecords, reader.TotalRecordsFound);
                //}
            }
        }

        class FindRecordsListener : RecordReader.IFindRecordListener
        {
            RecordEntry entry = new RecordEntry();
            readonly Plugin plugin;
            readonly byte pluginNumber;
            readonly RecordReader reader;

            public int Added { get; set; }
            public int Overriding { get; set; }

            public FindRecordsListener(Plugin plugin, RecordReader reader, Progress progress)
            {
                this.plugin = plugin;
                this.reader = reader;

                // Prefetch plugin number for indexed forms
                pluginNumber = plugin.context.Plugins.GetPluginNumber(plugin);
            }

            public void OnRecordFound(string signature, RecordReader.FindRecordListenerArgs args)
            {
                args.TargetEntry = entry;
            }

            public void OnRecordEntry(RecordEntry record)
            {
                Form form = new Form()
                {
                    PluginNumber = pluginNumber,
                    FilePosition = record.FilePosition,
                    FormKind = (FormKind)record.Signature,
                    FormId = reader.ReferenceMapper.LocalToContext(record.FormId)
                };

                plugin.Context.Forms.Add(form);
                Added++;
                if (form.IsOverriding)
                    Overriding++;
            }
        }

        public void LoadForms(bool lazyLoading, Func<Form, bool> predicate)
        {
            if (context.AsyncFromLoadingMaxWorkers < 1)
                throw new InvalidOperationException("AsyncFromLoadingMaxWorkers cannot be less than 1");

            Stream stream = context.DataFileProvider.GetDataFile(FileMode.Open, fileName).Open();
            
            // Uncomment this to preload a whole file into memory
            // The loading time is marginaly shorter (around 4%)
            // but only when another background worker is employed
            //Log.Fine("Preloading plugin file {0}.", fileName);
            //var data = new byte[stream.Length];
            //stream.Read(data, 0, (int)stream.Length);
            //stream = new ReadOnlyMemoryStream(data);

            using (var reader = context.CreateReader(stream))
            {
                // Header needs to be read again
                header = reader.ReadHeader();
 
                // Prepare reference mapper
                reader.ReferenceMapper = new PluginReferenceMapper(this);

                // Prepare string locator (if plugin strings are localized)
                if (reader.PluginFlags.HasFlag(PluginFlags.Localized))
                    reader.StringLocator = new PluginStringLocator(this);

                int formsToLoadCount = 0;

                // Apply predicate to the enumeration, if any
                // this will cause the total number of forms to load be unknown
                var formsToLoadEnumeration = context.Forms.OfPlugin(this);
                if (predicate != null)
                {
                    // TODO: Fix the list to make the total count known 
                    formsToLoadEnumeration = formsToLoadEnumeration.Where(predicate);
                    formsToLoadCount = formsToLoadEnumeration.Count();
                }
                else
                {
                    // Get count if enumeration is a list or a collection
                    formsToLoadCount = formsToLoadEnumeration.Count();
                }

                if (formsToLoadEnumeration.Any())
                {
                    var formsToLoad = formsToLoadEnumeration;
                    int formsToLoadCountAtLeast = 0;

                    // Determine number of background jobs, if any
                    int jobs = 0;
                    if (context.AsyncFormLoading)
                    {
                        // Materialize part of the list
                        // to determine the count of background jobs at least
                        if (formsToLoadCount == 0)
                        {
                            int take = context.AsyncFormLoadingWorkerThreshold * context.AsyncFromLoadingMaxWorkers;
                            formsToLoadCountAtLeast = formsToLoad.Take(take).Count();
                            if (formsToLoadCountAtLeast < take)
                                formsToLoadCount = formsToLoadCountAtLeast;
                        }

                        // Determine number of jobs
                        if (formsToLoadCount > 0)
                        {
                            // Use known number of forms to load
                            jobs = Math.Min(formsToLoadCount / context.AsyncFormLoadingWorkerThreshold + 1, context.AsyncFromLoadingMaxWorkers);
                        }
                        else
                        {
                            // Use minimal determined number of forms to load
                            jobs = Math.Min(formsToLoadCountAtLeast / context.AsyncFormLoadingWorkerThreshold + 1, context.AsyncFromLoadingMaxWorkers);
                        }
                    }

                    // Indicate that total number of forms to load will be determined during iteration
                    bool formsToLoadIsUnknown = formsToLoadCount == 0;

                    if (formsToLoadIsUnknown)
                    {
                        Log.Fine("Total number of forms to load is not unknown");
                        if (context.AsyncFormLoading)
                        {
                            Log.Fine("Using {0} background jobs to load more than {1} forms", jobs, formsToLoadCountAtLeast);
                        }
                    }
                    else if (context.AsyncFormLoading)
                    {
                        Log.Fine("Using {0} background jobs to load {1} forms", jobs, formsToLoadCount);
                    }

                    using (var loader = new FormLoader(this, reader, lazyLoading, jobs))
                    {
                        using (var progress = Program.Status.StartProgress("Loading forms"))
                        {
                            foreach (Form form in formsToLoad)
                            {
                                loader.LoadForm(form);

                                // Unknown total number of forms to load
                                // has to be determined during iteration
                                if (formsToLoadIsUnknown)
                                    formsToLoadCount++;

                                progress.Update(loader.Loaded, Math.Max(formsToLoadCount, formsToLoadCountAtLeast), FileName);
                            }

                            if (context.AsyncFormLoading)
                            {
                                // Tell loader no more forms will be loaded
                                loader.Complete();

                                // Show progress while loader is still busy
                                while (loader.IsBusy)
                                {
                                    System.Threading.Thread.Sleep(50);
                                    progress.Update(loader.Loaded, Math.Max(formsToLoadCount, formsToLoadCountAtLeast), FileName);
                                }

                                // Wait for loader to finish completely
                                Log.Fine("Waiting for background jobs to finish");
                                loader.WaitForCompleted();
                            }
                        }

                        Log.Info("Loaded {0} forms from {1} ({2} supported, {3} unsupported)", loader.Loaded, fileName, loader.Supported, loader.Unsupported);

                        if (loader.Skipped > 0)
                            Log.Info("Skipped {0} forms that had been already loaded", loader.Skipped);
                    }
                }
            }
        }

        public void ReserveFormId(uint formId, string editorId)
        {      
            reservedFormIds.Add(formId);
            reservedEditorIdMap.Add(editorId, formId);
        }

        public void AddForms(IEnumerable<Form> formsToAdd)
        {
            // Retrieve plugin number so that added forms can be linked to it
            byte pluginNumber = context.Plugins.GetPluginNumber(this);

            foreach (var newForm in formsToAdd)
            {
                if (newForm.FormId == 0)
                {
                    // Brand new form is being added
                    newForm.PluginNumber = pluginNumber;
                    newForm.FilePosition = -1;

                    if (!string.IsNullOrEmpty(newForm.EditorId) && reservedEditorIdMap.ContainsKey(newForm.EditorId))
                    {
                        // Get reserved Form ID and adjust it to the plugin number
                        newForm.FormId = reservedEditorIdMap[newForm.EditorId] | (uint)((long)pluginNumber) << 24;

                        // Make sure it has not been claimed somehow
                        if (context.Forms.Contains(newForm.FormId))
                        {
                            Log.Warning("Form ID {0:X8} has been reserved for EditorId {1} but somehow it has already been claimed by form {2}.", newForm.FormId, newForm.EditorId, context.Forms[newForm.FormId]);

                            // Unset the Form ID and allow a new one be claimed below normally
                            newForm.FormId = 0;
                        }
                        else
                        {
                            // and remove it from reservation
                            reservedEditorIdMap.Remove(newForm.EditorId);
                            reservedFormIds.Remove(newForm.FormId);

                            Log.Fine("New form claims a reserved Form ID.");
                        }
                    }

                    // Assign new form ID if not reserved or reservation above failed
                    // Keep looking in case that Form ID is reserved or already claimed
                    if (newForm.FormId == 0)
                    {
                        do
                        {
                            // Grab new form ID and adjust it to the plugin number
                            newForm.FormId = header.NextFormId++ | (uint)((long)pluginNumber) << 24;

                            // FormIDs are reserved without the plugin number so mask 0xFFFFFF has to be applied when checking if new FormId has been reserved
                        } while (reservedFormIds.Contains(newForm.FormId & 0xFFFFFF) || context.Forms.Contains(newForm.FormId));

                        Log.Fine("New form claims a new Form ID.");
                    }

                    // Add new form to local and global index
                    context.Forms.Add(newForm);
                    Log.Fine("Form created: {0}", newForm);
                }
                else if (context.Forms.Contains(newForm.FormId))
                {
                    // Form with this FormID already exists, make sure the type matches
                    Form existingForm = context.Forms[newForm.FormId];
                    if (existingForm.FormKind != newForm.FormKind)
                    {
                        throw new InvalidOperationException("Cannot update or override form with another that is of different type");
                    }

                    if (existingForm.PluginNumber == pluginNumber)
                    {
                        newForm.PluginNumber = pluginNumber;
                        newForm.FilePosition = -1;

                        // Form is already in the target plugin so replacing the form
                        context.Forms.Remove(existingForm);
                        context.Forms.Add(newForm);
                        Log.Fine("Form updated: {0}", newForm);
                    }
                    else
                    {
                        newForm.PluginNumber = pluginNumber;
                        newForm.FilePosition = -1;

                        // Tatget plugin is different - the original form will be overriden
                        // New form will be added to local form index
                        // New form will replace original form in global index
                        context.Forms.Add(newForm);
                        Log.Fine("Form overriden: {0}", newForm);
                    }
                }
                else
                {
                    // TODO: Allow injecting
                    // New form has FormId but no form with that FormId exists
                    throw new InvalidOperationException("Cannot update or override form that does not exist");
                }
            }
        }

        public HashSet<uint> GetReferencedFormIds()
        {
            HashSet<uint> referencedFormIds = new HashSet<uint>();
            // Enumerate this plugins forms
            foreach (var form in Forms)
            {
                foreach (var formId in form.GetReferencedFormIds())
                {
                    // Do not add null references or references already added
                    if (formId != 0 && !referencedFormIds.Contains(formId))
                    {
                        referencedFormIds.Add(formId);
                    }
                }
            }
            return referencedFormIds;
        }

        public void Save()
        {
            Log.Info("Saving plugin " + fileName);

            byte thisPluginNumber = context.Plugins.GetPluginNumber(this);

            // Collect all referenced form IDs
            HashSet<uint> referencedFormIds = GetReferencedFormIds();

            // Union with form IDs this plugin is overriding
            var allFormIds = referencedFormIds.Union(Forms.Where(f => f.IsOverriding).Select(f => f.FormId));

            // Collect all masters (Always add the main plugin as master)
            HashSet<byte> collectedMasters = new HashSet<byte>() { 0 };
            foreach (var id in allFormIds)
            {
                if (!context.Forms.Contains(id))
                {
                    //Log.Warning("Unable to determine the master of unresolved reference [0x{0:X8}].", id);
                    // Skip unresolved references, a warning will be issued while writing
                    // TODO: To handle unresolved references as bes as possible, derive plugin number from formId and add it as a master
                    continue;
                }
                var form = context.Forms[id];

                // Add itself of not of the current plugin
                if (form.PluginNumber != thisPluginNumber)
                    if (!collectedMasters.Contains(form.PluginNumber))
                        collectedMasters.Add(form.PluginNumber);

                // Go through all overriden forms until the original form
                var overridenForm = form.OverridesForm;
                while (overridenForm != null)
                {
                    if (!collectedMasters.Contains(overridenForm.PluginNumber))
                        collectedMasters.Add(overridenForm.PluginNumber);

                    // Next overriden form by this form
                    overridenForm = overridenForm.OverridesForm;
                }
            }

            // Add headers (will be sorted by pluginNumber)
            foreach (var number in collectedMasters.OrderBy(i => i))
            {
                header.AddMasterFile(context.Plugins[number].FileName);
            }
            Log.Fine("Added masters: {0}", string.Join(", ", MasterFiles));

            //// Make a backup of existing file
            //string backupPath = null;
            //if (File.Exists(path))
            //{
            //    backupPath = path + "-" + (ulong)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            //    File.Move(path, backupPath);
            //}

            if (!Forms.Any())
            {
                Log.Warning("No forms to write. Empty plugin will be generated.");
            }

            int count = 0;
            long total = Forms.Count();

            using (var progress = Program.Status.StartProgress("Saving"))
            {
                var file = context.DataFileProvider.GetDataFile(FileMode.Create, fileName);
                using (var stream = file.Open())
                {
                    using (var writer = context.CreateWriter(stream))
                    {
                        // Prepare reference mapper
                        writer.ReferenceMapper = new PluginReferenceMapper(this);

                        // Write header
                        header.PluginVersion = context.GetLatestPluginVersion();
                        header.Flags = PluginFlags.None;
                        writer.WriteHeader(header);
                        Log.Fine("Written header record");

                        // Write forms by type
                        foreach (var type in context.Forms.GetFormKinds())
                        {
                            var formOfType = Forms.Where(f => f.FormKind == type);
                            if (formOfType.Any())
                                Log.Fine("Writting {0}", type);

                            foreach (var form in formOfType)
                            {
                                writer.WriteRecord(form.Record, form.FormId);
                                count++;

                                progress.Update(count, total, "{0}:{1}", file.Name, type);
                            }
                        }
                     }
                }
            }

            Log.Fine("Written {0} form(s)", count);
            Log.Info("Plugin saved");
        }

        public void SaveMetaData()
        {

        }

        public void PurgeDirtyEdits()
        {
            Log.Info("Purging dirty forms");

            // Create new list (ToArray) so that the form repository can be modified during the iteration 
            int count = 0;
            foreach (var form in context.Forms.OfPlugin(this).Where(f => f.IsOverriding && f.Record.Equals(f.OverridesForm.Record)).ToArray())
            {
                context.Forms.Remove(form);
                count++;

                Log.Fine("Deleted form: {0}", form);
            }
            
            if (count > 0)
            {
                Log.Info("Purged {0} dirty form(s).", count);
            }
        }

        public void Dispose()
        {
            //if (reader != null)
            //{
            //    reader.Dispose();
            //}
        }

        public override string ToString()
        {
            return string.Format("0x{0},Filename={1}", context.Plugins.Exists(fileName) ? context.Plugins.GetPluginNumber(this).ToString("X2") : "??", fileName);
        }
    }
}
