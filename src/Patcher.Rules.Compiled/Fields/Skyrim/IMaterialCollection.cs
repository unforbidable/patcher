/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a collection of <see cref="IMaterial">Materials</see>.
    /// </summary>
    /// <remarks>
    /// Materials in a collection can be edited or removed within a <c>foreach</c> iteration.
    /// </remarks>
    public interface IMaterialCollection : IEnumerable<IMaterial>
    {
        /// <summary>
        /// Gets the number of materials in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Creates a new <see cref="IMaterial">Material</see> based on the specified form item and count and adds it to this collection.
        /// </summary>
        /// <param name="item">Item for the new material.</param>
        /// <param name="count">Number of items for the new material.</param>
        void Add(IForm item, int count);

        /// <summary>
        /// Adds an existing <see cref="IMaterial">Material</see> to this collection.
        /// </summary>
        /// <param name="material">Material to add.</param>
        void Add(IMaterial material);

        /// <summary>
        /// Removes a <see cref="IMaterial">Material</see> from this collection.
        /// </summary>
        /// <param name="material"></param>
        void Remove(IMaterial material);

        /// <summary>
        /// Removes all <see cref="IMaterial">Materials</see> from this collection.
        /// </summary>
        void Clear();
    }
}
