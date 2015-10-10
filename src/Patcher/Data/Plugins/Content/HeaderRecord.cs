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

using System.Collections.Generic;

namespace Patcher.Data.Plugins.Content
{
    public abstract class HeaderRecord : Record
    {
        /// <summary>
        /// Gets or sets the plugin file flags.
        /// </summary>
        public PluginFlags Flags { get { return (PluginFlags)RawFlags; } set { RawFlags = (uint)value; } }

        public abstract string Author { get; set; }
        public abstract string Description { get; set; }
        public abstract uint NextFormId { get; set; }
        public abstract int NumRecords { get; set; }
        public abstract float Version { get; set; }

        internal abstract void AddMasterFile(string filename);
        public abstract IEnumerable<string> GetMasterFiles();
    }
}