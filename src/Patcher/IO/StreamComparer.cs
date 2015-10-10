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

namespace Patcher.IO
{
    public static class StreamComparer
    {
        private const int BufferSize = 4096;

        public static bool Compare(Stream one, Stream other)
        {
            BufferedStream oneBuffered = new BufferedStream(one, BufferSize);
            BufferedStream otherBuffered = new BufferedStream(other, BufferSize);

            // Compare length first, if seekable
            if (oneBuffered.CanSeek && otherBuffered.CanSeek)
            {
                if (oneBuffered.Length != otherBuffered.Length)
                    return false;
            }
            
            while (true)
            {
                int oneByte = oneBuffered.ReadByte();
                int otherByte = otherBuffered.ReadByte();

                // Both streams ended at the same time
                if (oneByte == -1 && otherByte == -1)
                    return true;

                // Read bytes are not equal
                // ore one stream ended before the other
                if (oneByte != otherByte)
                    return false;
            }
        }
    }
}
