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
    public sealed class ObjectBounds : Field
    {
        public short X1 { get; set; }
        public short Y1 { get; set; }
        public short Z1 { get; set; }
        public short X2 { get; set; }
        public short Y2 { get; set; }
        public short Z2 { get; set; }

        internal override void ReadField(RecordReader reader)
        {
            X1 = reader.ReadInt16();
            Y1 = reader.ReadInt16();
            Z1 = reader.ReadInt16();
            X2 = reader.ReadInt16();
            Y2 = reader.ReadInt16();
            Z2 = reader.ReadInt16();
        }

        internal override void WriteField(RecordWriter writer)
        {
            writer.Write(X1);
            writer.Write(Y1);
            writer.Write(Z1);
            writer.Write(X2);
            writer.Write(Y2);
            writer.Write(Z2);
        }

        public override Field CopyField()
        {
            return new ObjectBounds()
            {
                X1 = X1,
                Y1 = Y1,
                Z1 = Z1,
                X2 = X2,
                Y2 = Y2,
                Z2 = Z2
            };
        }

        public override bool Equals(Field other)
        {
            var cast = (ObjectBounds)other;
            return X1 == cast.X1 && Y1 == cast.Y1 && Z1 == cast.Z1 && X2 == cast.X2 && Y2 == cast.Y2 && Z2 == cast.Z2;
        }

        public override string ToString()
        {
            return string.Format("X1={0} Y1={1} Z1={2} X2={3} Y2={4} Z2={5}", X1, Y1, Z1, X2, Y2, Z2);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            yield break;
        }
    }
}
