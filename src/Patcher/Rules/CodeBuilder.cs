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

namespace Patcher.Rules
{
    sealed class CodeBuilder
    {
        readonly StringBuilder builder = new StringBuilder();

        List<string> usings = new List<string>();
        public List<string> Usings { get { return usings; } }

        readonly string namespaceName;
        readonly string className;
        readonly string comment;

        public CodeBuilder(string namespaceName, string className, string comment)
        {
            this.namespaceName = namespaceName;
            this.className = className;
            this.comment = comment;
        }

        public void WriteCode(string code)
        {
            builder.AppendLine(code);
        }

        public void WriteCodeAsReturn(string code)
        {
            builder.AppendFormat("return\n{0};\n\n", code);
        }

        public void BeginMethod(string name, Type sourceProxyType)
        {
            BeginMethod(name, sourceProxyType, null);
        }

        public void BeginMethod(string name, Type sourceProxyType, Type targetProxyType)
        {
            builder.AppendFormat("public static bool {0}(object __Source{1})\n{{\n", name,
                targetProxyType == null ? string.Empty : ", object __Target");
            builder.AppendFormat("{0} Source = ({0})__Source;\n{1}\n", sourceProxyType,
                targetProxyType == null ? string.Empty : string.Format("{0} Target = ({0})__Target;\n", targetProxyType));
        }

        public void ReturnTrue()
        {
            builder.Append("\n;\nreturn true;\n");
        }

        public void EndMethod()
        {
            builder.Append("}\n\n");
        }

        public override string ToString()
        {
            return Beautify(string.Format("{0}\n{1}\nnamespace {2}\n{{\npublic class {3}\n{{\n{4}}}\n}}\n",
                comment,
                string.Join("", usings.Select(u => string.Format("using {0};\n", u))),
                namespaceName, className,
                builder.ToString()));
        }

        private string Beautify(string source)
        {
            var builder = new StringBuilder();
            var reader = new StringReader(source);

            int indent = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                indent -= line.Where(c => c == '}').Count();

                if (indent > 0)
                {
                    var spaces = new string(Enumerable.Range(0, indent * 4).Select(i => ' ').ToArray());
                    builder.Append(spaces);
                }

                builder.Append(line);
                builder.Append("\r\n");

                indent += line.Where(c => c == '{').Count();
            }

            return builder.ToString();
        }
    }
}
