/// Copyright(C) 2018 Unforbidable Works
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

using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Patcher.Data.Models
{
    public class GameModel : IModel
    {
        public string Name { get; private set; }
        public string BasePlugin { get; private set; }
        public int LatestFormVersion { get; private set; }
        public string PluginsFileLocation { get; private set; }
        public string PluginsMatchLine { get; private set; }
        public string ArchivesExtension { get; private set; }
        public string StringsDefaultLanguage { get; private set; }

        public IModel[] Models { get; private set; }

        public GameModel(string name, string basePlugin, int lastFormVersion, string pluginsFileLocation, string pluginsMatchLine, string archivesExtension, string stringsDefaultLanguage, IEnumerable<IModel> models)
        {
            Name = name;
            BasePlugin = basePlugin;
            LatestFormVersion = lastFormVersion;
            PluginsFileLocation = pluginsFileLocation;
            PluginsMatchLine = pluginsMatchLine;
            ArchivesExtension = archivesExtension;
            StringsDefaultLanguage = stringsDefaultLanguage;
            Models = models.ToArray();
        }

        public override string ToString()
        {
            return string.Format("Name={0}", Name);
        }
    }
}
