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

namespace Patcher.Data.Plugins.Content.Enums.Skyrim
{
    [Flags]
    public enum BodyParts : uint
    {
        Head = 0x1,
        Hair = 0x2,
        Body = 0x4,
        Hands = 0x8,
        Forearms = 0x10,
        Amulet = 0x20,
        Ring = 0x40,
        Feet = 0x80,
        Calves = 0x100,
        Shield = 0x200,
        Tail = 0x400,
        LongHair = 0x800,
        Circlet = 0x1000,
        Ears = 0x2000,
        Unnamed14 = 0x4000,
        Unnamed15 = 0x8000,
        Unnamed16 = 0x10000,
        Unnamed17 = 0x20000,
        Unnamed18 = 0x40000,
        Unnamed19 = 0x80000,
        DecapitatedHead = 0x100000,
        Decapitated = 0x200000,
        Unnamed22 = 0x400000,
        Unnamed23 = 0x800000,
        Unnamed24 = 0x1000000,
        Unnamed25 = 0x2000000,
        Unnamed26 = 0x4000000,
        Unnamed27 = 0x8000000,
        Unnamed28 = 0x10000000,
        Unnamed29 = 0x20000000,
        Unnamed30 = 0x40000000,
        FX01 = 0x80000000
    }
}
