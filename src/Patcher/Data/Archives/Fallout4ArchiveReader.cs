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

namespace Patcher.Data.Archives
{
    internal sealed class Fallout4ArchiveReader : ArchiveReader
    {
        uint version;
        ArchiveType type;

        SortedDictionary<string, SortedDictionary<string, FileInfo>> sorted = new SortedDictionary<string, SortedDictionary<string, FileInfo>>();

        public Fallout4ArchiveReader(string path)
            : base(path)
        {
        }

        protected override void DoOpen()
        {
            using (CustomBinaryReader reader = new CustomBinaryReader(new FileStream(ArchivePath, FileMode.Open, FileAccess.Read)))
            {
                uint signature = reader.ReadUInt32();
                if (signature != 0x58445442)
                {
                    throw new InvalidDataException("File is not BA2");
                }

                version = reader.ReadUInt32();
                if (version > 1)
                {
                    throw new InvalidDataException("Unsupported archive file version: " + version);
                }

                type = (ArchiveType)reader.ReadUInt32();
                if (type != ArchiveType.General)
                {
                    Log.Fine("Skipping archive file which is not the general purpose type.");
                    return;
                }

                long baseOffset = reader.BaseStream.Position;
                uint fileCount = reader.ReadUInt32();
                long fileNameTableOffset = reader.ReadInt64();

                FileInfo[] files = new FileInfo[fileCount];
                for (int i = 0; i < fileCount; i++)
                {
                    files[i] = new FileInfo()
                    {
                        NameHash = reader.ReadUInt32(),
                        Type = reader.ReadUInt32(),
                        DirectoryNameHash = reader.ReadUInt32(),
                        Unknown1 = reader.ReadUInt32(),
                        DataOffset = reader.ReadInt64(),
                        DataCompressedSize = reader.ReadUInt32(),
                        DataUncompressedSize = reader.ReadUInt32(),
                        Unknown2 = reader.ReadUInt32()
                    };
                }

                reader.BaseStream.Position = fileNameTableOffset;
                for (int i = 0; i < fileCount; i++)
                {
                    ushort length = reader.ReadUInt16();
                    string path = reader.ReadStringFixedLength(length).ToLower();

                    string dir = Path.GetDirectoryName(path).ToLower();
                    string filename = Path.GetFileName(path).ToLower();
                    if (!sorted.ContainsKey(dir))
                    {
                        sorted.Add(dir, new SortedDictionary<string, FileInfo>());
                    }
                    sorted[dir].Add(filename, files[i]);
                }

            }
        }

        protected override bool DoFileExists(string path)
        {
            return FindFileInfo(path) != null;
        }

        protected override Stream DoGetFileStream(string path)
        {
            var file = FindFileInfo(path);
            if (file == null)
            {
                throw new InvalidOperationException("File not found in archive: " + path);
            }

            Stream stream = new FileStream(ArchivePath, FileMode.Open, FileAccess.Read);
            stream.Position = file.DataOffset;

            if (file.DataCompressedSize > 0)
            {
                return new CustomDeflateStream(stream, file.DataUncompressedSize);
            }
            else
            {
                return new ArchiveSubstream(stream, file.DataUncompressedSize);
            }
        }

        private FileInfo FindFileInfo(string path)
        {
            string dir = Path.GetDirectoryName(path).ToLower();
            string filename = Path.GetFileName(path).ToLower();

            if (sorted.ContainsKey(dir) && sorted[dir].ContainsKey(filename))
            {
                return sorted[dir][filename];
            }
            else
            {
                return null;
            }
        }

        public class FileInfo
        {
            public uint NameHash;
            public uint Type;
            public uint DirectoryNameHash;
            public uint Unknown1;
            public long DataOffset;
            public uint DataCompressedSize;
            public uint DataUncompressedSize;
            public uint Unknown2;
        }

        enum ArchiveType : uint
        {
            General = 0x4C524E47,
            DX10 = 0x30315844
        }
    }
}
