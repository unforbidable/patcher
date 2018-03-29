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

using Patcher.Code.Building;
using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    /// <summary>
    /// Represets an enum type.
    /// </summary>
    public sealed class CodeEnum : CodeType
    {
        /// <summary>
        /// Gets or sets the underlying type of this enum.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets the collection of members of this enum.
        /// </summary>
        public CodeEnumMemberCollection Members { get; private set; }

        public CodeEnum(string name) : base(name)
        {
            Members = new CodeEnumMemberCollection(this);

            Modifiers = CodeModifiers.Public;
        }

        public override void BuildCode(CodeBuilder builder)
        {
            base.BuildCode(builder);

            builder.AppendLine("enum {0} : {1}", Name, ModelLoadingHelper.GetEnumTypeName(Type));
            builder.AppendLine("{");
            builder.EnterBlock();

            bool first = true;
            foreach (var member in Members)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendLine(",");
                }

                member.BuildCode(builder);
            }
            builder.AppendLine("");

            builder.LeaveBlock();
            builder.AppendLine("}");
        }

    }
}
