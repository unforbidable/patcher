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

namespace Patcher.Data.Plugins.Content
{
    sealed class RecordMetadata : Metadata
    {
        public override long Length { get; set; }
        public uint Flags { get; set; }
        public uint FormId { get; set; }
        public uint Revision { get; set; }
        public ushort Version { get; set; }
        public ushort Unknown { get; set; }

        public override void ReadMetaData(RecordReader reader)
        {
            if (Signature == "GRUP")
            {
                throw new InvalidOperationException("Group signature was not expected now");
            }

            Length = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            FormId = reader.ReadUInt32();
            Revision = reader.ReadUInt32();
            Version = reader.ReadUInt16();
            Unknown = reader.ReadUInt16();
        }

        public override void WriteMetaData(RecordWriter writer)
        {
            writer.Write((uint)Length);
            writer.Write(Flags);
            writer.Write(FormId);
            writer.Write(Revision);
            writer.Write(Version);
            writer.Write(Unknown);
        }
    }
}
