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

using Patcher.Code.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    /// <summary>
    /// Represents a property.
    /// </summary>
    public sealed class CodeProperty : CodeMember
    {
        /// <summary>
        /// Gets the StringBuilder instance used to build the body of the getter of this method.
        /// </summary>
        public CodePropertyAccessor Getter { get; set; }

        /// <summary>
        /// Gets the StringBuilder instance used to build the body of the setter of this method.
        /// </summary>
        public CodePropertyAccessor Setter { get; set; }

        public CodeProperty(string type, string name) : base(type, name)
        {
            Modifiers = CodeModifiers.Public;
        }

        public override void BuildCode(CodeBuilder builder)
        {
            base.BuildCode(builder);

            builder.Append("{0} {1}", Type, Name);

            builder.Append(" { ");
            if (Getter != null)
            {
                builder.Append(CodeBuilderHelper.ModifiersToString(Getter.Modifiers));
                builder.Append("get");
                string body = Getter.Body.ToString();
                if (body.Length == 0)
                {
                    builder.Append("; ");
                }
                else
                {
                    builder.Append(" {{ {0} }} ", body);
                }
            }
            if (Setter != null)
            {
                builder.Append(CodeBuilderHelper.ModifiersToString(Setter.Modifiers));
                builder.Append("set");
                string body = Setter.Body.ToString();
                if (body.Length == 0)
                {
                    builder.Append("; ");
                }
                else
                {
                    builder.Append(" {{ {0} }} ", body);
                }
            }
            builder.AppendLine(" }");
        }
    }
}
