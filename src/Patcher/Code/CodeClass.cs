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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    /// <summary>
    /// Represets a class type.
    /// </summary>
    public sealed class CodeClass : CodeType
    {
        /// <summary>
        /// Gets the collection of types that are nested within this class.
        /// </summary>
        public CodeTypeCollection Types { get; private set; }

        /// <summary>
        /// Gets the collection of members of this class.
        /// </summary>
        public CodeMemberCollection Members { get; private set; }

        /// <summary>
        /// Gets or sets the value that specifies whether this class is static.
        /// </summary>
        public bool IsStatic { get; set; }

        public CodeClass(string name) : base(name)
        {
            Types = new CodeTypeCollection(this);
            Members = new CodeMemberCollection(this);
        }

        public override void BuildCode(CodeBuilder builder)
        {
            builder.AppendComment(Comment);
            builder.Append("public ");
            if (IsStatic)
            {
                builder.Append("static ");
            }
            builder.AppendLine("class {0}", Name);
            builder.AppendLine("{");
            builder.EnterBlock();

            foreach (var field in Members.OfType<CodeField>())
            {
                field.BuildCode(builder);
            }

            foreach (var prop in Members.OfType<CodeProperty>())
            {
                prop.BuildCode(builder);
            }

            // TODO: build class sub-types and members

            builder.LeaveBlock();
            builder.AppendLine("}");
        }

    }
}
