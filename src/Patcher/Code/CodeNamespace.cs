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
    /// Represents a namespace.
    /// </summary>
    public sealed class CodeNamespace : CodeItem
    {
        /// <summary>
        /// Gets the collection of types in this namespace.
        /// </summary>
        public CodeTypeCollection Types { get; private set; }

        public CodeNamespace(string name) : base(name)
        {
            CodeBuilderHelper.ValidateNamespaceName(name);

            Types = new CodeTypeCollection(this);
        }

        public override void BuildCode(CodeBuilder builder)
        {
            builder.AppendComment(Comment);
            builder.AppendLine(Name);
            builder.AppendLine("{");
            builder.EnterBlock();

            // TODO: Build types of this namespace

            builder.LeaveBlock();
            builder.AppendLine("}");
        }
    }
}
