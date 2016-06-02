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
    internal abstract class ArchiveReader : IDisposable
    {
        readonly string archivePath;
        bool opened = false;

        public string ArchivePath { get { return archivePath; } }

        public ArchiveReader(string path)
        { 
            archivePath = path;
        }

        protected abstract void DoOpen();
        protected abstract bool DoFileExists(string path);
        protected abstract Stream DoGetFileStream(string path);

        public void Open()
        {
            DoOpen();
            opened = true;
        }

        public bool FileExists(string path)
        {
            if (!opened)
                throw new InvalidOperationException("Archive not opened");

            return DoFileExists(path);
        }

        public Stream GetFileStream(string path)
        {
            if (!opened)
                throw new InvalidOperationException("Archive not opened");

            return DoGetFileStream(path);
        }

        public void Dispose()
        {
            opened = false;
        }
    }
}
