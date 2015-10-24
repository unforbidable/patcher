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
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Archives
{
    sealed class ArchiveSubstream : Stream
    {
        readonly Stream stream;
        readonly long length;
        readonly long start;

        public ArchiveSubstream(Stream stream, long length)
        {
            this.stream = stream;
            this.length = length;
            start = stream.Position;
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return false; } }

        public override long Length { get { return length; } }
        public override long Position { get { return stream.Position - start; } set { stream.Position = value + start; } }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            count = Math.Min(count, (int)(Length - Position));

            return stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long pos;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    pos = stream.Seek(offset + start, origin);
                    break;

                case SeekOrigin.End:
                    pos = stream.Seek(offset - start - length, origin);
                    break;

                default: // case SeekOrigin.Current:
                    pos = stream.Seek(offset, origin);
                    break;
            }

            if (pos < start || pos >= start + length)
                throw new IOException("Attempted to seek past the boundaries");

            return pos;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
