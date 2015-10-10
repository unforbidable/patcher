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

namespace Patcher.Data.Plugins.Content.Fields
{
    public sealed class ByteArray : Field
    {
        public byte[] Bytes { get; set; }

        internal override void ReadField(RecordReader reader)
        {
            Bytes = reader.ReadBytesToEnd();
        }

        internal override void WriteField(RecordWriter writer)
        {
            writer.Write(Bytes);
        }

        public override Field CopyField()
        {
            return new ByteArray()
            {
                Bytes = (byte[])Bytes.Clone()
            };
        }

        public override bool Equals(Field other)
        {
            // Length equals?
            if (Bytes.Length != ((ByteArray)other).Bytes.Length)
                return false;

            // TODO: Compare arrays byte by byte in a simple loop
            foreach (var item in Bytes.Zip(((ByteArray)other).Bytes, (n, w) => new { One = n, Other = w }))
            {
                if (item.One != item.Other)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Bytes == null)
                sb.Append("null");

            sb.AppendFormat("Length={0}", Bytes.Length);

            const int max = 32;
            if (Bytes.Length > 0)
            {
                for (int i = 0; i < Bytes.Length; i++)
                {
                    if (i % 4 == 0)
                        sb.Append(" ");

                    if (i == max && Bytes.Length > max + 4)
                    {
                        sb.Append(" ...");
                        break;
                    }

                    sb.AppendFormat("{0:X2}", Bytes[i]);
                }
            }

            return sb.ToString();
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            yield break;
        }
    }
}
