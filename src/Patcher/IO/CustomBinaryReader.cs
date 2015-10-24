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
    public class CustomBinaryReader : BinaryReader
    {
        const int internalBufferSize = 256; // 256 - 4096
        byte[] internalBuffer = new byte[internalBufferSize];

        public CustomBinaryReader(Stream stream)
            : base(stream)
        {
        }

        public CustomBinaryReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        public string ReadStringFixedLength(int length)
        {
            if (length > internalBufferSize)
            {
                StringBuilder sb = new StringBuilder(length);
                int remainingBytesToRead = length;
                while (remainingBytesToRead > 0)
                {
                    // Read chunk
                    int chunkBytesToRead = Math.Min(internalBufferSize, remainingBytesToRead);
                    int bytesRead = Read(internalBuffer, 0, chunkBytesToRead);
                    if (bytesRead != chunkBytesToRead)
                    {
                        // Exception if not enough data would be read
                        throw new EndOfStreamException();
                    }
                    // Append chunk bytes
                    sb.Append(InternalBufferToString(bytesRead));
                    // Adjdust remaining bytes
                    remainingBytesToRead -= bytesRead;
                }
                return sb.ToString();
            }
            else
            {
                // String will fit the internal buffer - do not use string reader
                int totalBytesRead = Read(internalBuffer, 0, length);
                if (totalBytesRead < length)
                {
                    throw new EndOfStreamException();
                }
                return InternalBufferToString(length);
            }
        }

        public string ReadStringZeroTerminated()
        {
            StringBuilder sb = null;
            int i = 0;
            byte b;
            while ((b = ReadByte()) != '\0')
            {
                internalBuffer[i++] = b;
                if (i >= internalBufferSize)
                {
                    // Create string builder
                    // The final string length will be at least internalBufferSize, so lets double it
                    if (sb == null)
                        sb = new StringBuilder(internalBufferSize * 2);

                    // Append part of the string
                    sb.Append(InternalBufferToString(i));
                    i = 0;
                }
            }
            if (sb != null)
            {
                // Append the rest if anything in the buffer
                if (i > 0)
                    sb.Append(InternalBufferToString(i));
                // Return string builder
                return sb.ToString();
            }
            else
            {
                // Return string from internal buffer
                return InternalBufferToString(i);
            }
        }

        private string InternalBufferToString(int length)
        {
            return Encoding.UTF8.GetString(internalBuffer, 0, length);
        }

    }
}
