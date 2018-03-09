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
using System.Diagnostics;
using System.Reflection;

namespace Patcher
{
    class Program
    {
        public readonly static string ProgramFolder = "Patcher";
        public readonly static string ProgramRulesFolder = "rules";
        public readonly static string ProgramCacheFolder = "cache";
        public readonly static string ProgramLogsFolder = "logs";
        public readonly static string ProgramModelsFolder = "models";

        public static string GetProgramVersionInfo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = new Version(fvi.FileVersion);
            return string.Format("{0} {1}.{2}.{3}", assembly.GetName().Name, version.Major, version.Minor, version.Build);
        }

        public static string GetProgramFullVersionInfo()
        {
            return string.Format("{0} [{1}]", GetProgramVersionInfo(), Assembly.GetEntryAssembly().GetName().Version.ToString());
        }
    }
}
