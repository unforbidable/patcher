// Copyright(C) 2018 Unforbidable Works
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
using Patcher.Code.Building;

namespace Patcher.Code
{
    /// <summary>
    /// Represents a class field.
    /// </summary>
    public sealed class CodeField : CodeMember
    {
        /// <summary>
        /// Gets or sets the value used to initialize this field or null if the field is not initialized.
        /// </summary>
        public string Value { get; set; }

        public CodeField(string type, string name) : base(type, name)
        {
        }

        public override void BuildCode(CodeBuilder builder)
        {
            if (IsPublic)
            {
                builder.Append("public ");
            }
            if (IsStatic)
            {
                builder.Append("static ");
            }
            builder.Append("{0} {1}", Type, Name);

            if (!string.IsNullOrEmpty(Value))
            {
                if (Type == "string")
                {
                    builder.Append(" = \"{0}\"", Value);
                }
                else
                {
                    builder.Append(" = {0}", Value);
                }
            }
            builder.AppendLine(";");
        }
    }
}
