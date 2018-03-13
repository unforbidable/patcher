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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    /// <summary>
    /// Represents a collection of namespaces.
    /// </summary>
    public sealed class CodeNamespaceCollection : Collection<CodeNamespace>
    {
        /// <summary>
        /// Gets the code base that owns this collection of namespaces.
        /// </summary>
        public CodeBase Owner { get; private set; }

        public CodeNamespaceCollection(CodeBase owner)
        {
            Owner = owner;
        }

        protected override void InsertItem(int index, CodeNamespace item)
        {
            base.InsertItem(index, item);
        }
    }
}
