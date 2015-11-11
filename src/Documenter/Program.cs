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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Documenter
{
    class Program
    {
        public const string RootNamespace = "Patcher.Rules.Compiled";
        public const string RootFolder = "reference";
        public const string TargetPath = @".";

        static void Main(string[] args)
        {
            var assembly = Assembly.GetAssembly(typeof(Patcher.Rules.Compiled.Forms.IForm));
            var xmlFilePath = Path.GetFileNameWithoutExtension(assembly.GetName().CodeBase) + ".xml";

            var games = new string[] { "Skyrim", "Fallout4" };
            foreach (var game in games)
            {
                XDocument source = XDocument.Load(xmlFilePath);
                PageGenerator generator = new PageGenerator(source, assembly, game);
                generator.GeneratePages();
            }
        }
    }
}
