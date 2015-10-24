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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a collection of <see cref="ICondition">Conditions</see>.
    /// </summary>
    /// <remarks>
    /// Use any of the <see cref="Helpers.Skyrim.IFunctionsHelper"/> helper methods to create new conditions.
    /// Conditions can be removed from a collection using the <code>Remove()</code> method during a <c>foreach</c> iteration.
    /// </remarks>
    public interface IConditionCollection : IEnumerable<ICondition>
    {
        /// <summary>
        /// Gets the number of <see cref="ICondition">Conditions</see> in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a <see cref="ICondition"/> to this collection.
        /// </summary>
        /// <param name="item"></param>
        void Add(ICondition item);

        /// <summary>
        /// Removes a <see cref="ICondition"/> from this collection.
        /// </summary>
        /// <param name="item"></param>
        void Remove(ICondition item);

        /// <summary>
        /// Removes all <see cref="ICondition">Conditions</see> from this collection.
        /// </summary>
        void Clear();
    }
}
