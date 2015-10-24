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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data
{
    public class PluginIndex : IEnumerable<Plugin>
    {
        public const byte MaxPluginNumber = 254;
        public const byte InvalidPluginNumber = 255;

        IList<Plugin> indexByOrder = new List<Plugin>();
        IDictionary<string, byte> indexByFilename = new SortedDictionary<string, byte>(StringComparer.CurrentCultureIgnoreCase);

        public int Count { get { return indexByOrder.Count; } }

        public Plugin this[string fileName] { get { return indexByOrder[indexByFilename[fileName]]; } }
        public Plugin this[byte index] { get { return indexByOrder[index]; } }

        public byte GetPluginNumber(string fileName)
        {
            if (indexByFilename.ContainsKey(fileName))
                return indexByFilename[fileName];
            else
                return InvalidPluginNumber;
        }

        public byte GetPluginNumber(Plugin plugin)
        {
            return GetPluginNumber(plugin.FileName);
        }

        internal byte AddPlugin(Plugin plugin)
        {
            if (indexByFilename.ContainsKey(plugin.FileName))
                throw new InvalidOperationException("Plugin filename is already used");

            if (indexByOrder.Count > MaxPluginNumber)
                throw new InvalidOperationException("Plugin index limit reached");

            byte index = (byte)indexByOrder.Count;

            indexByOrder.Add(plugin);
            indexByFilename.Add(plugin.FileName, index);

            return index;
        }

        public bool Exists(string filename)
        {
            return indexByFilename.ContainsKey(filename);
        }

        public bool Exists(byte index)
        {
            return index < indexByOrder.Count;
        }

        public IEnumerator<Plugin> GetEnumerator()
        {
            return indexByOrder.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return indexByOrder.GetEnumerator();
        }
    }
}
