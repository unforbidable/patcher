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
    /// Defines body nodes.
    /// </summary>
    [Flags]
    public enum BodyNodes : uint
    {
        /// <summary>
        /// Represents no body node.
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents the <b>Head</b> body node.
        /// </summary>
        Head = 0x1,
        /// <summary>
        /// Represents the <b>Hair</b> body node.
        /// </summary>
        Hair = 0x2,
        /// <summary>
        /// Represents the <b>Body</b> body node.
        /// </summary>
        Body = 0x4,
        /// <summary>
        /// Represents the <b>Hands</b> body node.
        /// </summary>
        Hands = 0x8,
        /// <summary>
        /// Represents the <b>Forearms</b> body node.
        /// </summary>
        Forearms = 0x10,
        /// <summary>
        /// Represents the <b>Amulet</b> body node.
        /// </summary>
        Amulet = 0x20,
        /// <summary>
        /// Represents the <b>Ring</b> body node.
        /// </summary>
        Ring = 0x40,
        /// <summary>
        /// Represents the <b>Feet</b> body node.
        /// </summary>
        Feet = 0x80,
        /// <summary>
        /// Represents the <b>Calves</b> body node.
        /// </summary>
        Calves = 0x100,
        /// <summary>
        /// Represents the <b>Shield</b> body node.
        /// </summary>
        Shield = 0x200,
        /// <summary>
        /// Represents the <b>Tail</b> body node.
        /// </summary>
        Tail = 0x400,
        /// <summary>
        /// Represents the <b>LongHair</b> body node.
        /// </summary>
        LongHair = 0x800,
        /// <summary>
        /// Represents the <b>Circlet</b> body node.
        /// </summary>
        Circlet = 0x1000,
        /// <summary>
        /// Represents the <b>Ears</b> body node.
        /// </summary>
        Ears = 0x2000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed14 = 0x4000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed15 = 0x8000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed16 = 0x10000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed17 = 0x20000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed18 = 0x40000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed19 = 0x80000,
        /// <summary>
        /// Represents the <b>DecapitatedHead</b> body node.
        /// </summary>
        DecapitatedHead = 0x100000,
        /// <summary>
        /// Represents the <b>Decapitated</b> body node.
        /// </summary>
        Decapitated = 0x200000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed22 = 0x400000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed23 = 0x800000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed24 = 0x1000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed25 = 0x2000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed26 = 0x4000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed27 = 0x8000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed28 = 0x10000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed29 = 0x20000000,
        /// <summary>
        /// Represents an unnamed body node.
        /// </summary>
        Unnamed30 = 0x40000000,
        /// <summary>
        /// Represents the <b>FX01</b> body node.
        /// </summary>
        FX01 = 0x80000000
    }
}
