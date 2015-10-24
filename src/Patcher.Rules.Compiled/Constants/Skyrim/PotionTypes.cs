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

namespace Patcher.Rules.Compiled.Constants.Skyrim
{
    /// <summary>
    /// Defines <b>Potion</b> types.
    /// </summary>
    public enum PotionTypes
    {
        /// <summary>
        /// Allows the game engine to assume the type of the <b>Potion</b> based on its effects.
        /// </summary>
        Auto,
        /// <summary>
        /// Indicates the <b>Potion</b> is food or a drink.
        /// </summary>
        Food,
        /// <summary>
        /// Indicates the <b>Potion</b> is a medicine.
        /// </summary>
        Medicine,
        /// <summary>
        /// Indicates the <b>Potion</b> is a posion.
        /// </summary>
        Poison,
    }
}
