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
using Patcher.Data.Plugins.Content;
using Patcher.Data.Plugins.Content.Records.Skyrim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.Data
{
    /// <summary>
    /// Represents an implementation of DataContext specific to game The Elder Scrolls: Skyrim.
    /// </summary>
    [DataContext("Skyrim.esm")]
    public sealed class SkyrimDataContext : DataContext
    {
        readonly static string[] hiddenFormTypes = new string[] { "CELL", "WRLD" };

        // Skyrim - Interface.bsa should be always loaded because it contains localized text files
        readonly static string[] defaultArchives = new string[] { };
        //readonly static string[] defaultArchives = new string[] { "Skyrim - Misc.bsa" , "Skyrim - Shaders.bsa", "Skyrim - Textures.bsa", "Skyrim - Interface.bsa",
        //   "Skyrim - Animations.bsa", "Skyrim - Meshes.bsa", "Skyrim - Sounds.bsa", "Skyrim - Voices.bsa", "Skyrim - VoicesExtra.bsa" };

        protected override IEnumerable<string> GetDefaultArchives()
        {
            return defaultArchives;
        }

        protected override IEnumerable<FormKind> GetIgnoredFormKinds()
        {
            // Convert string[] to FormType[] only now when needed as not to allocate FormType values needlessly
            return hiddenFormTypes.Select(t => (FormKind)t).ToArray();
        }

        protected override IPluginListProvider GetPluginListProvider()
        {
            // Create plugin list provider that will use this context specific path to plugins.txt
            // unless DataFileProvider overrides the path
            return new DefaultPluginListProvider(DataFileProvider, 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Skyrim\plugins.txt"));
        }

        protected override string GetGameTitle()
        {
            return "Skyrim";
        }

        protected override string GetGameInstallPath()
        {
            return "Skyrim";
        }

        protected override string GetArchiveExtension()
        {
            return "bsa";
        }

        protected override IEnumerable<Form> GetHardcodedForms(byte pluginNumber)
        {
            if (pluginNumber > 0)
                yield break;

            // PlayerRef
            yield return new Form()
            {
                FormId = 0x14,
                FormKind = FormKind.FromName(Names.REFR),
                Record = new DummyRecord()
                {
                    EditorId = "PlayerRef"
                }
            };
        }

        protected override string GetDefaultLanguage()
        {
            return "english";
        }

        public override ushort GetLatestFormVersion()
        {
            return 0x2C;
        }

    }
}
