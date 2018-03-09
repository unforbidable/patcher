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
    public sealed class ModOrganizerDataFileProvider : IDataFileProvider
    {
        readonly string dataFolder;
        readonly string moOverwritePath;
        readonly string pluginListFilePath;

        public string DataFolderPath { get { return dataFolder; } }

        List<string> searchPaths = new List<string>();

        public ModOrganizerDataFileProvider(string dataFolder, string customPluginListFile, string moProfilePath, string moModPath)
        {
            this.dataFolder = Path.GetFullPath(dataFolder);

            moProfilePath = Path.GetFullPath(moProfilePath);

            // Ensure plugin.txt exists
            pluginListFilePath = Path.Combine(moProfilePath, "plugins.txt");
            if (!File.Exists(pluginListFilePath))
                throw new InvalidDataException("File plugins.txt not found in Mod Organizer profile folder.");
            //Log.Fine("Plugin list file from Mod Organize will be used: {0}", pluginListFilePath);

            if (!string.IsNullOrEmpty(customPluginListFile))
            {
                // Override if provided
                pluginListFilePath = customPluginListFile;
            }

            // Validate or assume modpath
            if (string.IsNullOrEmpty(moModPath))
            {
                moModPath = Path.GetFullPath(Path.Combine(moProfilePath, "..", "..", "mods"));
                //Log.Fine("Assumed Mod Organizer 'mods' folder: {0}", moModPath);
            }
            else
            {
                moModPath = Path.GetFullPath(moModPath);
            }

            // Assume overwrite folder
            moOverwritePath = Path.GetFullPath(Path.Combine(moProfilePath, "..", "..", "overwrite"));
            if (!Directory.Exists(moOverwritePath))
                throw new InvalidDataException("Could not find Mod Organizer 'overwrite' folder: " + moOverwritePath);
            //Log.Fine("Assumed Mod Organizer 'overwrite' folder: {0}", moOverwritePath);

            // Overwrite folder will be search first
            searchPaths.Add(moOverwritePath);

            // Read modlist.txt
            string modListFilePath = Path.Combine(moProfilePath, "modlist.txt");
            using (var reader = new StreamReader(modListFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Ignore lines that do not start with '+' 
                    // '+' means a mod is active in given profile
                    if (line.StartsWith("+"))
                    {
                        string modName = line.Substring(1);
                        string path = Path.Combine(moModPath, modName);
                        if (!Directory.Exists(path))
                            throw new InvalidDataException("Could not find Mod Organizer mod folder: " + path);

                        searchPaths.Add(path);
                    }
                }
            }

            // Data Folder will be searched last
            searchPaths.Add(dataFolder);
        }

        public IEnumerable<DataFile> FindDataFiles(string path, string searchPattern, bool recursive)
        {
            // Search all paths when opening existing files
            foreach (var searchPath in searchPaths)
            {
                string tryPath = Path.Combine(searchPath, path);
                if (Directory.Exists(tryPath))
                {
                    foreach (string file in Directory.EnumerateFiles(tryPath, searchPattern))
                    {
                        yield return new DataFile(this, FileMode.Open, file, Path.Combine(tryPath, Path.GetFileName(file)));
                    }
                }
            }

            //TODO: implement recursion, might not be needed if MO support is abandoned
        }

        public DataFile GetDataFile(FileMode mode, string path)
        {
            if (mode == FileMode.Open)
            {
                // Search all paths when opening existing files
                foreach (var searchPath in searchPaths)
                {
                    string tryPath = Path.Combine(searchPath, path);
                    if (File.Exists(tryPath))
                        return new DataFile(this, mode, tryPath, path);
                }

                // When not found give path to file in data folder
                // Opening the data file will fail as it should because the file does not exist
                return new DataFile(this, mode, Path.Combine(dataFolder, path), path);
            }
            else
            {
                // Every new file will be created in the overwite folder
                return new DataFile(this, mode, Path.Combine(moOverwritePath, path), path);
            }
        }

        public DataFile GetPluginListFile(string defaultPluginFilePath)
        {
            // Ignore the dafault game path
            // always return the plugins.txt in MO profile folder.
            return new DataFile(this, FileMode.Open, pluginListFilePath, null);
        }
    }
}
