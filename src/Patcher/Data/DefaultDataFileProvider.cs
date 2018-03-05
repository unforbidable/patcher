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

namespace Patcher.Data
{
    public sealed class DefaultDataFileProvider : IDataFileProvider
    {
        readonly string dataFolder;
        public string DataFolderPath { get { return dataFolder; } }

        readonly string pluginListFile;

        public DefaultDataFileProvider(string dataFolder, string customPluginListFile)
        {
            this.dataFolder = Path.GetFullPath(dataFolder);

            if (!Directory.Exists(this.dataFolder))
            {
                throw new InvalidDataException("Specified data folder not found: " + this.dataFolder);
            }

            pluginListFile = customPluginListFile;
        }

        public DataFile GetDataFile(FileMode mode, string path)
        {
            string fullPath = Path.Combine(DataFolderPath, path);
            return new DataFile(this, mode, fullPath, path);
        }

        public IEnumerable<DataFile> FindDataFiles(string directory, string searchPattern)
        {
            string fullPath = Path.Combine(dataFolder, directory);
            if (Directory.Exists(fullPath))
            {
                foreach (string file in Directory.EnumerateFiles(fullPath, searchPattern))
                {
                    yield return new DataFile(this, FileMode.Open, file, Path.Combine(directory, Path.GetFileName(file)));
                }
            }
        }

        public DataFile GetPluginListFile(string defaultPluginFilePath)
        {
            // Return default received as argument unless custom path has been provided
            string fullPath = pluginListFile ?? defaultPluginFilePath;
            return new DataFile(this, FileMode.Open, fullPath, null);
        }
    }
}
