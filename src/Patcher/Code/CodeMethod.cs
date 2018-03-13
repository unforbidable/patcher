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
    /// Represents a method.
    /// </summary>
    public sealed class CodeMethod : CodeMember
    {
        CodeBuilder bodyBuilder = null;

        /// <summary>
        /// Gets the CodeBuilder instance used to build the body of this method.
        /// </summary>
        public CodeBuilder CodeBuilder { get { if (bodyBuilder == null) bodyBuilder = new CodeBuilder(); return bodyBuilder; } }

        /// <summary>
        /// Gets or sets the value indicating whether this method is virtual.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether this method is abstract.
        /// </summary>
        public bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets the parameters of this method.
        /// </summary>
        public string Parameters { get; set; }

        public CodeMethod(string type, string name) : base(type, name)
        {
        }

        public override void BuildCode(CodeBuilder builder)
        {
            // TODO: Build method code
        }
    }
}
