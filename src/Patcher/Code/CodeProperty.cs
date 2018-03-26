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
        CodeBuilder getterBuilder = null;
        CodeBuilder setterBuilder = null;

        /// <summary>
        /// Gets the StringBuilder instance used to build the body of the getter of this method.
        /// </summary>
        public CodeBuilder GetterBodyBuilder { get { if (getterBuilder == null) getterBuilder = new CodeBuilder(); return getterBuilder; } }

        /// <summary>
        /// Gets the StringBuilder instance used to build the body of the setter of this method.
        /// </summary>
        public CodeBuilder SetterBodyBuilder { get { if (setterBuilder == null) setterBuilder = new CodeBuilder(); return setterBuilder; } }

        public CodeProperty(string type, string name) : base(type, name)
        {
        }

        public override void BuildCode(CodeBuilder builder)
        {
            builder.AppendComment(Comment);
            if (IsPublic)
            {
                builder.Append("public ");
            }
            else
            {
                builder.Append("private ");
            }
            if (IsStatic)
            {
                builder.Append("static ");
            }
            builder.Append("{0} {1}", Type, Name);

            builder.AppendLine(" { get" + (getterBuilder != null ? " { " + getterBuilder.ToString() + " }" : ";") + " set" + (setterBuilder != null ? " { " + setterBuilder.ToString() + " }" : ";") + " }");
        }
    }
}
