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

using System.Collections.Generic;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a collection of <see cref="IScript">Scripts</see> attached to an object.
    /// </summary>
    /// <remarks>
    /// <p>
    /// A script cannot appear twice in a single script collection.
    /// </p>
    /// <p>
    /// Property <code>this[string]</code> represents an <code>indexer</code> that can be used to retrieve a <see cref="IScript"/> with a specific name. 
    /// </p>
    /// </remarks>
    public interface IScriptCollection : IEnumerable<IScript>
    {
        /// <summary>
        /// Gets the number of <see cref="IScript">Scripts</see> in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the <see cref="IScript"/> which has the specified name from this collection or returns <c>null</c> if no such script could be found.
        /// </summary>
        /// <param name="name">Name of the script.</param>
        /// <returns></returns>
        IScript this[string name] { get; }

        /// <summary>
        /// Adds an existing <see cref="IScript"/> to this collection.
        /// </summary>
        /// <param name="script">Script to add.</param>
        void Add(IScript script);

        /// <summary>
        /// Adds a new <see cref="IScript"/> with specified name to this collection.
        /// </summary>
        /// <param name="name">Name of the new script to add.</param>
        void Add(string name);

        /// <summary>
        /// Removes the <see cref="IScript"/> with the specified name from this collection.
        /// </summary>
        /// <param name="name">Name of the script to remove.</param>
        void Remove(string name);

        /// <summary>
        /// Determines whether a <see cref="IScript"/> with specified name exists in this collection.
        /// </summary>
        /// <param name="name">Name of the script to find.</param>
        /// <returns></returns>
        bool Contains(string name);

        /// <summary>
        /// Removes all <see cref="IScript">Scripts</see> from this collection.
        /// </summary>
        void Clear();
    }
}
