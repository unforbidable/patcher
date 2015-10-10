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

using Patcher.Data.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Strings
{
    sealed class PluginStringLocator : IStringLocator, IDisposable
    {
        readonly Plugin plugin;

        StringTableReader reader = null;
        StringTableReader dlreader = null;
        StringTableReader ilreader = null;

        // Created and used internally by plugin
        internal PluginStringLocator(Plugin plugin)
        {
            this.plugin = plugin;

            reader = CreateReader(plugin, "strings", false);
            dlreader = CreateReader(plugin, "dlstrings", true);
            ilreader = CreateReader(plugin, "ilstrings", true);
        }

        private static StringTableReader CreateReader(Plugin plugin, string extension, bool ignoreStringLength)
        {
            string pathWithoutExtension = Path.Combine("strings", Path.GetFileNameWithoutExtension(plugin.FileName)) + "_english";
            string path = string.Format("{0}.{1}", pathWithoutExtension, extension);

            // Search data folder first
            var stringsFile = plugin.Context.DataFileProvider.GetDataFile(FileMode.Open, path);
            if (stringsFile.Exists())
            {
                Log.Fine("Indexing strings in " + path);
                return new StringTableReader(stringsFile.Open())
                {
                    MustIgnoreStringLength = ignoreStringLength
                };
            }
            else
            {
                // Search archives (BSA)
                if (plugin.Context.Archives.FileExists(path))
                {
                    Stream stream = plugin.Context.Archives.GetFileStream(path);
                    Log.Fine("Indexing strings in " + plugin.Context.Archives.WhereIsFile(path) + ":" + path);
                    return new StringTableReader(stream)
                    {
                        MustIgnoreStringLength = ignoreStringLength
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetString(uint index)
        {
            return GetStringFromReader(reader, index);
        }

        public string GetDLString(uint index)
        {
            return GetStringFromReader(dlreader, index);
        }

        public string GetILString(uint index)
        {
            return GetStringFromReader(ilreader, index);
        }

        private string GetStringFromReader(StringTableReader reader, uint index)
        {
            if (reader != null)
            {
                return reader.ReadString(index);
            }
            else
            {
                throw new InvalidDataException("One or more locale string file(s) could not be located for plugin: " + plugin.FileName);
            }
        }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
            if (dlreader != null)
            {
                dlreader.Dispose();
                dlreader = null;
            }
            if (ilreader != null)
            {
                ilreader.Dispose();
                ilreader = null;
            }
        }
    }
}
