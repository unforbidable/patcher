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

        public RecordModel[] Records { get; private set; }
        public StructModel[] Structures { get; private set; }
        public FieldGroupModel[] FieldGroups { get; private set; }
        public EnumModel[] Enumerations { get; private set; }

        public void ReadFromXml(XElement element)
        {
            var name = element.Element("name");
            if (name != null)
            {
                Name = name.Value;
            }

            var basePlugin = element.Element("base-plugin");
            if (basePlugin != null)
            {
                BasePlugin = basePlugin.Value;
            }

            var latestFormVersion = element.Element("latest-form-version");
            if (latestFormVersion != null)
            {
                int parsed;
                if (int.TryParse(latestFormVersion.Value, out parsed))
                {
                    LatestFormVersion = parsed;
                }
                else
                {
                    throw new ModelLoadingException("Expected integer value in <latest-form-version>");
                }
            }

            var plugins = element.Element("plugins");
            if (plugins != null)
            {
                var fileLocation = plugins.Element("file-location");
                if (fileLocation != null)
                {
                    PluginsFileLocation = fileLocation.Value;
                }

                var matchLine = plugins.Element("match-line");
                if (matchLine != null)
                {
                    PluginsMatchLine = matchLine.Value;
                }
            }

            var archives = element.Element("archives");
            if (archives != null)
            {
                var extension = archives.Element("extension");
                if (extension != null)
                {
                    ArchivesExtension = extension.Value;
                }
            }

            var strings = element.Element("strings");
            if (strings != null)
            {
                var defaultLanguage = strings.Element("default-language");
                if (defaultLanguage != null)
                {
                    StringsDefaultLanguage = defaultLanguage.Value;
                }
            }
        }

        public void LoadModelFiles(IEnumerable<string> files)
        { 
            var loader = new ModelLoader(this);
            loader.LoadFiles(files);

            // TODO: Add model objects from loader to Game model
        }
    }
}
