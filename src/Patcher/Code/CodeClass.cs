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
        /// Gets the collection of type names this class implements or inherits.
        /// </summary>
        public ICollection<string> Extends { get; private set; }

        public CodeClass(string name) : base(name)
        {
            Extends = new Collection<string>();
            Types = new CodeTypeCollection(this);
            Members = new CodeMemberCollection(this);

            Modifiers = CodeModifiers.Public;
        }

        public override void BuildCode(CodeBuilder builder)
        {
            base.BuildCode(builder);

            builder.Append("class {0}", Name);

            if (Extends.Count > 0)
            {
                builder.Append(" : " + string.Join(",", Extends) + " ");
            }
            builder.AppendLine();

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

            foreach (var method in Members.OfType<CodeMethod>())
            {
                method.BuildCode(builder);
            }

            foreach (var type in Types)
            {
                type.BuildCode(builder);
            }

            builder.LeaveBlock();
            builder.AppendLine("}");
        }

    }
}
