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
    public abstract class Compound : Field
    {
        public Compound()
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

        internal sealed override void ReadField(RecordReader reader)
        {
            // Method replaced with one that provides the Field name that is being read and the depth as well
            throw new NotImplementedException("Not implemented for compound fields");
        }

        internal virtual void ReadCompoundField(RecordReader reader, string fieldName, int depth)
        {
            var members = InfoProvider.GetCompoundInfo(GetType()).Members;
            if (!members.ContainsKey(fieldName))
                throw new InvalidProgramException("Unexpected field name while reading a compound field: " + fieldName);

            var meminf = members[fieldName];
            reader.ReadField(this, fieldName, meminf, depth);
        }

        internal override void WriteField(RecordWriter writer)
        {
            BeforeWrite(writer);

            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            foreach (var meminfo in compinfo.Members.Values.Distinct())
            {
                writer.WriteField(this, meminfo);
            }

            AfterWrite(writer);
        }

        public override Field CopyField()
        {
            var fldinfo = InfoProvider.GetFieldInfo(GetType());
            var other = fldinfo.CreateInstance();

            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            compinfo.Copy(this, other);
            return other;
        }

        public override bool Equals(Field other)
        {
            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            return compinfo.Equate(this, other);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            return compinfo.GetReferencedFormIds(this);
        }

        internal void OnCreate(RecordReader reader, int depth)
        {
            BeforeRead(reader);

            reader.BeginReadFields(depth);
        }

        internal void OnComplete(RecordReader reader, int depth)
        {
            reader.EndReadFields(depth);

            // Make sure list fields that were supposed to be loaded are not null
            var members = InfoProvider.GetCompoundInfo(GetType()).Members;
            foreach (MemberInfo meminfo in members.Where(m => m.Value.IsListType).Select(m => m.Value))
            {
                meminfo.EnsureListCreated(this);
            }

            AfterRead(reader);
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
    }
}
