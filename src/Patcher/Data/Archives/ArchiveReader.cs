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
    internal sealed class ArchiveReader : IDisposable
    {
        const uint BsaVersionOblivion = 0x67;
        const uint BsaVersionSkyrim = 0x68;

        readonly string archivePath;
        uint version;
        ArchiveFlags flags;

        SortedDictionary<string, SortedDictionary<string, FileInfo>> sorted = new SortedDictionary<string, SortedDictionary<string, FileInfo>>();

        public ArchiveReader(string path)
            : this(new FileStream(path, FileMode.Open, FileAccess.Read))
        {           
        }

        public ArchiveReader(FileStream stream)
        {
            archivePath = stream.Name;

            using (CustomBinaryReader reader = new CustomBinaryReader(stream))
            {
                uint signature = reader.ReadUInt32();
                if (signature != 0x415342)
                {
                    throw new InvalidDataException("File is not BSA");
                }

                version = reader.ReadUInt32();
                uint folderOffset = reader.ReadUInt32();
                flags = (ArchiveFlags)reader.ReadUInt32();
                uint folderCount = reader.ReadUInt32();
                uint fileCount = reader.ReadUInt32();
                uint totalFolderNameLength = reader.ReadUInt32();
                uint totalFileNameLength = reader.ReadUInt32();
                uint fileExtensions = reader.ReadUInt32();

                FolderInfo[] folders = new FolderInfo[(int)folderCount];

                // Read folders
                reader.BaseStream.Position = folderOffset;
                for (int i = 0; i < folderCount; i++)
                {
                    ulong hash = reader.ReadUInt64();
                    uint count = reader.ReadUInt32();
                    uint offset = reader.ReadUInt32() - totalFileNameLength;
                    folders[i] = new FolderInfo()
                    {
                        FileCount = count,
                        ContentOffset = offset
                    };
                }

                // Read folder content (name and files)
                foreach (var folder in folders)
                {
                    byte folderNameLength = reader.ReadByte();
                    folder.Path = Encoding.UTF8.GetString(reader.ReadBytes(folderNameLength - 1));
                    byte zero = reader.ReadByte();

                    folder.Files = new FileInfo[folder.FileCount];
                    for (int i = 0; i < folder.FileCount; i++)
                    {
                        ulong hash = reader.ReadUInt64();
                        uint size = reader.ReadUInt32();

                        bool compressed = flags.HasFlag(ArchiveFlags.DefaultCompressed);
                        if ((size & 0xf0000000) != 0)
                        {
                            size &= 0xfffffff;
                            compressed = !compressed;
                        }

                        uint offset = reader.ReadUInt32();
                        folder.Files[i] = new FileInfo()
                        {
                            Size = size,
                            DataOffset = offset,
                            IsCompressed = compressed
                        };
                    }
                }

                long total = fileCount;
                long loaded = 0;
                string filename = Path.GetFileName(archivePath);

                using (var progress = Program.Status.StartProgress("Indexing archive"))
                { 
                    // Read file names
                    foreach (var folder in folders)
                    {
                        foreach (var file in folder.Files)
                        {
                            file.Filename = reader.ReadStringZeroTerminated();
                            loaded++;
                        }
                        progress.Update(loaded, total, filename);
                    }
                }

                // Convert to nested sorted dictionary for fast search
                for (int i = 0; i < folderCount; i++)
                {
                    var files = new SortedDictionary<string, FileInfo>();
                    for (int j = 0; j < folders[i].FileCount; j++)
                    {
                        files.Add(folders[i].Files[j].Filename, folders[i].Files[j]);
                    }
                    sorted.Add(folders[i].Path, files);
                }

                return;
            }
        }

        public bool FileExists(string path)
        {
            return FindFileInfo(path) != null;
        }

        public Stream GetFileStream(string path)
        {
            var file = FindFileInfo(path);
            if (file == null)
            {
                throw new InvalidOperationException("File not found in archive: " + path);
            }

            Stream stream = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
            stream.Position = file.DataOffset;
            long length = file.Size;

            if (flags.HasFlag(ArchiveFlags.FileNameBeforeData))
            {
                // Consume (skip) filename before data
                var singleByteBuffer = new byte[1];
                stream.Read(singleByteBuffer, 0, 1);
                var stringLength = singleByteBuffer[0];
                stream.Seek(stringLength, SeekOrigin.Current);
                // Adjust length according to consumed data
                length -= stringLength + 1;
            }

            if (file.IsCompressed)
            {
                // Read original size
                var originalSizeBuffer = new byte[4];
                stream.Read(originalSizeBuffer, 0, 4);
                uint originalSize = BitConverter.ToUInt32(originalSizeBuffer, 0);
                
                // Deflate stream
                return new CustomDeflateStream(stream, originalSize);
            }
            else
            { 
                return new ArchiveSubstream(stream, length);
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

        class FolderInfo
        {
            public uint FileCount { get; set; }
            public uint ContentOffset { get; set; }
            public string Path { get; set; }
            public FileInfo[] Files { get; set; }

            public override string ToString()
            {
                return Path;
            }
        }

        class FileInfo
        {
            public uint Size { get; set; }
            public uint DataOffset { get; set; }
            public bool IsCompressed { get; set; }
            public string Filename { get; set; }

            public override string ToString()
            {
                return Filename;
            }
        }

        public void Dispose()
        {
        }
    }
}
