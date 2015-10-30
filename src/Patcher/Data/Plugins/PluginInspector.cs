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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins
{
    public sealed class PluginInspector
    {
        IDictionary<long, uint> index = new SortedDictionary<long, uint>();

        IList<RecordInfo> newRecords = new List<RecordInfo>();
        public IEnumerable<RecordInfo> NewRecords { get { return newRecords; } }

        public PluginInspector(DataContext context, DataFile file)
        {
            Log.Info("Inspecting records in plugin {0}", file.Name);
            using (var reader = new RecordReader(file.Open(), context))
            {
                var header = reader.ReadHeader();
                int masters = header.GetMasterFiles().Count();
                byte newRecordPluginNumber = (byte)masters;

                foreach (var entry in reader.FindRecordsAsync())
                {
                    index.Add(entry.FilePosition, entry.FormId);
                }

                long newForms = 0;
                long done = 0;

                using (var progress = Display.StartProgress("Inspecting records"))
                {
                    foreach (var item in index)
                    {
                        if (item.Value >> 24 == newRecordPluginNumber)
                        {
                            // Read only the bare minimum of form data (such as EditorID)
                            var record = new DummyRecord();
                            reader.ReadRecordAt(item.Key, record, true);

                            if (!string.IsNullOrEmpty(record.EditorId))
                            {
                                var formId = item.Value & 0xFFFFFF;

                                Log.Fine("Found new record {0:X8} '{1}'", formId, record.EditorId);

                                newRecords.Add(new RecordInfo()
                                {
                                    FormId = formId,
                                    EditorId = record.EditorId
                                });
                            }
                            newForms++;
                        }
                        done++;
                        progress.Update(done, index.Count, file.Name);
                    }
                }

                Log.Info("Found {0} records out of which {1} are new records.", index.Count, newForms);
            }
        }

        public struct RecordInfo
        {
            public uint FormId;
            public string EditorId;
        }
    }
}
