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
    /// Provides common properties to all code artifacts.
    /// </summary>
    public abstract class CodeItem
    {
        /// <summary>
        /// The name of this code artifact.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The comment printed above this artifact.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the modifiers of this artefact.
        /// </summary>
        public CodeModifiers Modifiers { get; set; }

        public CodeItem(string name)
        {
            Name = name;
        }

        public virtual void BuildCode(CodeBuilder builder)
        {
            builder.AppendComment(Comment);
            builder.Append(CodeBuilderHelper.ModifiersToString(Modifiers));
        }
    }
}
