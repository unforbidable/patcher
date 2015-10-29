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
    /// Defines <b>Weapon</b> types.
    /// </summary>
    public enum WeaponTypes
    {
        /// <summary>
        /// Represents a hand to hand weapon.
        /// </summary>
        HandToHand = 0,
        /// <summary>
        /// Represents a one-handed sword weapon.
        /// </summary>
        OneHandSword = 1,
        /// <summary>
        /// Represents a one-handed dagger weapon.
        /// </summary>
        OneHandDagger = 2,
        /// <summary>
        /// Represents a one-handed axe weapon.
        /// </summary>
        OneHandAxe = 3,
        /// <summary>
        /// Represents a one-handed mace weapon.
        /// </summary>
        OneHandMace = 4,
        /// <summary>
        /// Represents a two-handed sword weapon.
        /// </summary>
        TwoHandSword = 5,
        /// <summary>
        /// Represents a two-handed axe weapon.
        /// </summary>
        TwoHandAxe = 6,
        /// <summary>
        /// Represents a bow.
        /// </summary>
        Bow = 7,
        /// <summary>
        /// Represents a staff.
        /// </summary>
        Staff = 8,
        /// <summary>
        /// Represents a crossbox.
        /// </summary>
        Crossbow = 9
    }
}
