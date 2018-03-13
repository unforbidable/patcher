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
    /// Represents a collection of types.
    /// </summary>
    public sealed class CodeTypeCollection : Collection<CodeType>
    {
        /// <summary>
        /// Gets the namespace or parent type that owns this collection of types.
        /// </summary>
        public CodeItem Owner { get; private set; }

        public CodeTypeCollection(CodeItem owner)
        {
            Owner = owner;
        }

        protected override void InsertItem(int index, CodeType item)
        {
            CodeBuilderHelper.ValidateNameNotEqualToOwner(item.Name, Owner);

            base.InsertItem(index, item);
        }
    }
}
