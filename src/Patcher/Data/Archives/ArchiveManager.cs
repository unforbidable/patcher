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
    public sealed class ArchiveManager : IDisposable
    {
        readonly DataContext context;
        List<ArchiveFile> archives = new List<ArchiveFile>();

        // Created by DataContext
        internal ArchiveManager(DataContext context)
        {
            this.context = context;
        }

        internal void AddArchive(string filename)
        {
            var reader = CreateArchiveReader(filename);
            reader.Open();

            // Insert at the beginning because last added will be searched first (has the highest priority)
            archives.Insert(0, new ArchiveFile()
            {
                Filename = filename,
                Reader = reader
            });
        }

        private ArchiveReader CreateArchiveReader(string filename)
        {
            string extension = Path.GetExtension(filename).ToLower();
            switch (extension)
            {
                case ".bsa":
                    return new SkyrimArchiveReader(filename);

                case ".ba2":
                    return new Fallout4ArchiveReader(filename);

                default:
                    throw new InvalidDataException("Unsupported archive extension: " + extension);
            }
        }

        public bool FileExists(string path)
        {
            foreach (var archive in archives)
            {
                if (archive.Reader.FileExists(path))
                    return true;
            }
            return false;
        }

        public string WhereIsFile(string path)
        {
            foreach (var archive in archives)
            {
                if (archive.Reader.FileExists(path))
                    return archive.Filename;
            }
            return null;
        }

        public Stream GetFileStream(string path)
        {
            foreach (var archive in archives)
            {
                if (archive.Reader.FileExists(path))
                    return archive.Reader.GetFileStream(path);
            }
            throw new InvalidDataException("File not found in any archive: " + path);
        }

        public void Dispose()
        {
            foreach (var archive in archives)
                archive.Reader.Dispose();
            archives.Clear();
        }

        class ArchiveFile
        {
            public string Filename { get; set; }
            public ArchiveReader Reader { get; set; }
        }
    }
}
