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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    public abstract class Record
    {
        public Record()
        {
            // Initialize fields that should not be null
            var members = InfoProvider.GetCompoundInfo(GetType()).Members;
            foreach (var meminfo in members.Values.Distinct().Where(m => m.Initialize))
            {
                if (meminfo.IsListType)
                {
                    // Instantiate list field
                    meminfo.EnsureListCreated(this);
                }
                else if (!meminfo.IsPrimitiveType)
                {
                    // Instantiate complex fields
                    var fieldinfo = InfoProvider.GetFieldInfo(meminfo.FieldType);
                    meminfo.SetValue(this, fieldinfo.CreateInstance());
                }
            }
        }

        internal void ReadRecord(RecordReader reader)
        {
            ReadRecord(reader, false);
        }

        internal void ReadRecord(RecordReader reader, bool lazyLoading)
        {
            var members = InfoProvider.GetCompoundInfo(GetType()).Members;

            BeforeRead(reader);

            reader.BeginReadFields(0);

            HashSet<string> remainingPropertiesToLoad = lazyLoading ? new HashSet<string>(members.Values.Where(m => m.IsLazy).SelectMany(m => m.FieldNames)) : null;

            foreach (string propertyName in reader.FindFields())
            {
                if (members.ContainsKey(propertyName))
                {
                    MemberInfo meminf = members[propertyName];
                    reader.ReadField(this, propertyName, meminf, 0);

                    if (remainingPropertiesToLoad != null)
                    {
                        if (!meminf.IsListType)
                            remainingPropertiesToLoad.Remove(propertyName);

                        if (!remainingPropertiesToLoad.Any())
                        {
                            // Loaded all properties that were supposed to
                            reader.CancelFindFields();
                            break;
                        }
                    }
                }
                else if (lazyLoading || GetType() == typeof(DummyRecord))
                {
                    // Prevent segment errors when not all properties were expected to be consumed
                    reader.SeekEndOfSegment();
                }
                else
                {
                    Log.Warning("Unexpected record property: {0}", propertyName);
                }
            }

            reader.EndReadFields(0);

            //// Make sure list properties that were supposed to be loaded are not null
            //foreach (MemberInfo meminfo in members.Where(m => m.Value.IsListType && (fieldsToLoad == null || fieldsToLoad.Contains(m.Key))).Select(m => m.Value))
            //{
            //    meminfo.EnsureListCreated(this);
            //}

            AfterRead(reader);
        }

        internal void WriteRecord(RecordWriter writer)
        {
            BeforeWrite(writer);

            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            foreach (var meminfo in compinfo.Members.Values.Distinct().OrderBy(m => m.Order))
            {
                writer.WriteField(this, meminfo);
            }

            AfterWrite(writer);
        }

        protected virtual void BeforeRead(RecordReader reader)
        {
        }

        protected virtual void AfterRead(RecordReader reader)
        {
        }

        protected virtual void BeforeWrite(RecordWriter writer)
        {
        }

        protected virtual void AfterWrite(RecordWriter writer)
        {
        }

        /// <summary>
        /// Represents record version.
        /// </summary>
        internal ushort Version { get; set; }

        /// <summary>
        /// Represents record flags in the raw format, i.e. not projected as an Enum.
        /// </summary>
        internal uint RawFlags { get; set; }

        /// <summary>
        /// Determines whether a flag has been set to the record.
        /// </summary>
        /// <typeparam name="T">The try of Enum.</typeparam>
        /// <param name="value">The flag to determine.</param>
        /// <returns></returns>
        protected bool HasFlag(Enum value)
        {
            return (RawFlags & Convert.ToUInt32(value)) != 0;
        }

        /// <summary>
        /// Sets or clears a flag to or from the record.
        /// </summary>
        /// <param name="value">The flag to set or clear.</param>
        /// <param name="set">Indicates whether the flag should be set or cleared.</param>
        protected void SetFlag(Enum value, bool set) 
        {
            if (set)
                RawFlags = RawFlags | Convert.ToUInt32(value);
            else
                RawFlags = RawFlags & ~Convert.ToUInt32(value);
        }

    }
}
