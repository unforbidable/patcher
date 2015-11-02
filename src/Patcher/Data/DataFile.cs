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

namespace Patcher.Data
{
    public sealed class DataFile
    {
        readonly IDataFileProvider provider;
        readonly FileMode mode;
        readonly string fullPath;
        readonly string requestedPath;
        readonly string name;

        public string RequestedPath { get { return requestedPath; } }
        public string FullPath { get { return fullPath; } }
        public string Name { get { return name; } }

        // Instances created by classes implementing IDataFileProvider
        internal DataFile(IDataFileProvider provider, FileMode mode, string fullPath, string requestedPath)
        {
            this.provider = provider;
            this.mode = mode;
            this.requestedPath = requestedPath;
            this.fullPath = fullPath;

            name = Path.GetFileName(requestedPath);
        }

        public bool Exists()
        {
            return File.Exists(FullPath);
        }

        public FileStream Open()
        {
            // Returns stream depeding on mode
            if (mode == FileMode.Open)
            {
                Log.Fine("Opening file {0} for reading.", fullPath);
                return new ReadOnlyFileStream(FullPath, mode, FileAccess.Read);
            }
            else
            {
                Log.Fine("Opening file {0} for writing.", fullPath);
                // When creating files create path if it does not exist
                EnsureDirectoryExists(Path.GetDirectoryName(FullPath));
                return new FileStream(FullPath, mode, FileAccess.Write);
            }
        }
        
        public void Delete()
        {
            File.Delete(FullPath);
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                EnsureDirectoryExists(Path.GetDirectoryName(path));

                Log.Fine("Creating directory {0}.", path);
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Copies a stream into the current data file, optionally only if does not exists and contains different data.
        /// </summary>
        /// <param name="input">The Stream to copy data from.</param>
        /// <param name="updateOnly">If true, file will not be recreated if already exists and contains the same data.</param>
        /// <returns>Returns true if file has been created or updated.</returns>
        public bool CopyFrom(Stream input, bool updateOnly)
        {
            if (mode == FileMode.Open)
                throw new InvalidOperationException("Cannot write into data file retrieved using the FileMode.Open mode.");

            // Non-data files such as plugin.txt do not have a request path relative to the data folder
            // TODO: Do not provide non-data files via DataFileProvider
            if (RequestedPath == null)
                throw new InvalidOperationException("Cannot write into a special data file.");

            if (updateOnly)
            {
                // Look if file already exists
                // To check whether it needs to be updated
                // Retreive new file from the provider with FileMode.Open 
                var existingFile = provider.GetDataFile(FileMode.Open, RequestedPath);
                if (existingFile.Exists())
                {
                    using (var existingStream = existingFile.Open())
                    {
                        // If input is not memory stream, create one and copy the data from the input stream into it
                        MemoryStream memoryStream = input as MemoryStream;
                        if (memoryStream == null)
                        {
                            // Initialize memory stream as big as the existing stream length if imput steam is not seekable
                            int initialMemoryStreamLength = input.CanSeek ? (int)input.Length : (int)existingStream.Length;
                            memoryStream = new MemoryStream(initialMemoryStreamLength);
                            input.CopyTo(memoryStream);
                            memoryStream.Position = 0;
                        }

                        // Compare existing stream with input stream (or memory stream where input stream was copied into)
                        if (StreamComparer.Compare(existingStream, memoryStream))
                        {
                            Log.Fine("Cached file {0} is up to date.", existingFile.FullPath);
                            return false;
                        }

                        // Reset memeory stream after comparison
                        memoryStream.Position = 0;

                        // Use memory stream (where input stream has been copied into) instead of input stream 
                        // when updating existing file
                        if (input != memoryStream)
                            input = memoryStream;

                        Log.Fine("Cached file {0} exists but is no longer valid and will be updated.", existingFile.FullPath);
                    }
                }
                else
                {
                    Log.Fine("File {0} is not cached and will be created.", existingFile.FullPath);
                }
            }

            using (var newStream = Open())
            {
                input.CopyTo(newStream);
            }

            return true;
        }

        public string GetRelativePath()
        {
            return GetRelativePath(FullPath);
        }

        public static string GetRelativePath(string path)
        {
            string currentDir = Directory.GetCurrentDirectory();
            if (path.StartsWith(currentDir, StringComparison.OrdinalIgnoreCase))
            {
                return "." + path.Substring(currentDir.Length);
            }
            else
            {
                return path;
            }
        }
    }
}
