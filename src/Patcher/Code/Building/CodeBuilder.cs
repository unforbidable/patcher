// Copyright(C) 2015,1016,2017,2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Code.Building
{
    public class CodeBuilder
    {
        readonly StringBuilder builder = new StringBuilder();
        readonly CodeBuilderOptions options = new CodeBuilderOptions();

        public CodeBuilderOptions Options { get { return options; } }

        int indent = 0;

        public void EnterBlock()
        {
            indent++;
        }

        public void LeaveBlock()
        {
            indent--;
        }

        public void AppendComment(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (string line in text.Trim().Split( new string[] { "\r\n", "\n" }, StringSplitOptions.None))
                {
                    AppendLine(string.Format("// {0}", line));
                }
            }
        }

        public void AppendLine()
        {
            AppendLine(string.Empty);
        }

        public void AppendLine(string format, params string[] args)
        {
            AppendLine(string.Format(format, args));
        }

        public void Append(string format, params string[] args)
        {
            Append(string.Format(format, args));
        }

        bool newLine = true;

        public void AppendLine(string text)
        {
            Append(text);
            builder.AppendLine();
            newLine = true;
        }

        public void Append(string text)
        {
            if (newLine)
            {
                string spaces = new string(' ', indent * 4);
                builder.Append(spaces);
                newLine = false;
            }

            builder.Append(text);
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
