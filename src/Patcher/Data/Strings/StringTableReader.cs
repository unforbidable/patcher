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

using Patcher.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Strings
{
    internal sealed class StringTableReader : IDisposable
    {
        readonly CustomBinaryReader reader;

        public bool MustIgnoreStringLength { get; set; }

        SortedDictionary<uint, long> table = new SortedDictionary<uint, long>();

        public StringTableReader(string filename)
            : this(new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
        }

        public StringTableReader(Stream stream)
        {
            reader = new CustomBinaryReader(stream);

            uint count = reader.ReadUInt32();
            uint dataSize = reader.ReadUInt32();

            long dataPosition = 8 + count * 8;

            for (int i = 0; i < count; i++)
            {
                uint index = reader.ReadUInt32();
                uint offset = reader.ReadUInt32();
                table.Add(index, dataPosition + offset);
            }
        }

        public string ReadString(uint index)
        {
            if (!table.ContainsKey(index))
            {
                Log.Warning("String not found in string table: " + index);
                return string.Empty;
            }

            lock (reader)
            {
                reader.BaseStream.Seek(table[index], SeekOrigin.Begin);

                if (MustIgnoreStringLength)
                    reader.BaseStream.Seek(4, SeekOrigin.Current);

                return reader.ReadStringZeroTerminated();
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
