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
using System.Threading.Tasks;

namespace Patcher.Data
{
    sealed class Fallout4PluginListProvider : IPluginListProvider
    {
        readonly List<PluginListEntry> plugins = new List<PluginListEntry>();
        public IEnumerable<PluginListEntry> Plugins { get { return plugins; } }

        public Fallout4PluginListProvider(IDataFileProvider dataFileProvider, string defaultPluginFilePath)
        {
            using (var reader = new StreamReader(dataFileProvider.GetPluginListFile(defaultPluginFilePath).Open()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("#") || line.Length == 0)
                        continue;

                    // In Fallout4 since 1.5 version
                    // active plugin file name lines start with an asterisk
                    if (line.StartsWith("*") && line.Length > 1)
                    {
                        plugins.Add(new PluginListEntry()
                        {
                            Filename = line.Substring(1)
                        });
                    }
                }
            }
        }
    }
}
