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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    sealed class GroupMetadata : Metadata
    {
        public override long Length { get; set; }
        public FormKind FormKind { get; set; }
        public GroupType GroupType { get; set; }
        public byte Day { get; set; }
        public byte TotalMonths { get; set; }
        public ushort Unknown1 { get; set; }
        public ushort Version { get; set; }
        public ushort Unknown2 { get; set; }

        private GroupPropertiesUnion properties;

        public override void ReadMetaData(RecordReader reader)
        {
            if (Signature != "GRUP")
            {
                throw new InvalidOperationException("Signature GRUP was expected");
            }

            Length = reader.ReadUInt32() - 24;
            properties = new GroupPropertiesUnion() { FormId = reader.ReadUInt32() };
            GroupType = (GroupType)reader.ReadUInt32();
            Day = reader.ReadByte();
            TotalMonths = reader.ReadByte();
            Unknown1 = reader.ReadUInt16();
            Version = reader.ReadUInt16();
            Unknown2 = reader.ReadUInt16();

            // Covert properties to FormType
            if (GroupType == GroupType.Top)
                FormKind = (FormKind)Encoding.UTF8.GetString(BitConverter.GetBytes(properties.FormId));
        }

        public override void WriteMetaData(RecordWriter writer)
        {
            writer.Write((uint)Length + 24);

            // Write either FormType or properties union
            if (GroupType == GroupType.Top)
                writer.WriteStringFixedLength(FormKind);
            else
                writer.Write(properties.FormId);

            writer.Write((uint)GroupType);
            writer.Write(Day);
            writer.Write(TotalMonths);
            writer.Write(Unknown1);
            writer.Write(Version);
            writer.Write(Unknown2);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct GroupPropertiesUnion
        {
            [FieldOffset(0)]
            public short Y;
            [FieldOffset(2)]
            public short X;
            [FieldOffset(0)]
            public uint FormId;
            [FieldOffset(0)]
            public int Number;
        }
    }
}
