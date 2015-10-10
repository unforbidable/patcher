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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.IO
{
    public sealed class CustomDeflateStream : DeflateStream
    {
        long position;
        long length;

        public CustomDeflateStream(Stream stream, long decompressedLength)
            : base(stream, CompressionMode.Decompress)
        {
            length = decompressedLength;

            // Consume/ignore header
            stream.ReadByte();
            stream.ReadByte();
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return length;
            }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                Seek(value - position, SeekOrigin.Current);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (offset < 0 || origin != SeekOrigin.Current)
            {
                throw new NotSupportedException("Cannot rewind compressed stream");
            } 
            else
            {
                while (offset-- != 0)
                {
                    if (ReadByte() == -1)
                    {
                        throw new EndOfStreamException("End of compressed stream reached");
                    }
                }
                return position;
            }
        }

        public override int Read(byte[] array, int offset, int count)
        {
            int bytesRead = base.Read(array, offset, count);
            position += bytesRead;
            return bytesRead;
        }
    }
}
