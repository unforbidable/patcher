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
    sealed class FieldMetadata : Metadata
    {
        public override long Length { get; set; }

        public override void ReadMetaData(RecordReader reader)
        {
            if (Signature == "XXXX")
            {
                // Segment length is always 4 which not interesting so skip 2 bytes
                reader.Seek(2);

                // Read lenght of the next segment
                uint nextSegmentLength = reader.ReadUInt32();

                // Load the actual signature (segment after XXXX)
                Signature = reader.ReadStringFixedLength(4);

                // Length is ZERO so skip 2 bytes and use value from the previous segment
                reader.Seek(2);
                Length = nextSegmentLength;
            }
            else
            {
                Length = reader.ReadUInt16();
            }
            
        }

        public override void WriteMetaData(RecordWriter writer)
        {
            // TODO: write XXXX segment when current segment length exceeds max ushort
            writer.Write((ushort)Length);
        }
    }
}
