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
    /// Represents a collection of <see cref="IEffect">Effects</see> that can be used to specify effects of items such as <b>Potions</b> or <b>Scrolls</b>.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="Helpers.Skyrim.IEngineHelper"/> helper method <code>Engine.CreateEffect()</code> to create new effects.
    /// </remarks>
    public interface IEffectCollection : IEnumerable<IEffect>
    {
        /// <summary>
        /// Gets the number of effects in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an <see cref="IEffect"/> to this collection.
        /// </summary>
        /// <param name="effect"></param>
        void Add(IEffect effect);

        /// <summary>
        /// Removes an <see cref="IEffect"/> from this collection.
        /// </summary>
        /// <param name="effect"></param>
        void Remove(IEffect effect);

        /// <summary>
        /// Removes all <see cref="IEffect">Effects</see> from this collection.
        /// </summary>
        void Clear();
    }
}
