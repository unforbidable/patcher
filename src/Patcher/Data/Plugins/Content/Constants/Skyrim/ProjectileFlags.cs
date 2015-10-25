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

namespace Patcher.Data.Plugins.Content.Constants.Skyrim
{
    [Flags]
    public enum ProjectileFlags : ushort
    {
        Hitscan = 0x0001,
        Explosion = 0x0002,
        AltExplosionTrigger = 0x0004,
        MuzzleFlash = 0x0008,
        Unknown4 = 0x0010,
        CanBeDisabled = 0x0020,
        CanBePickedUp = 0x0040,
        Supersonic = 0x0080,
        PinsLimbsCriticalEffect = 0x0100,
        PassThroughSmallTransparent = 0x0200,
        DisableCombatAimCorrection = 0x0400,
        Rotation = 0x0800,
        Unknown12 = 0x1000,
        Unknown13 = 0x2000,
        Unknown14 = 0x4000,
        Unknown15 = 0x8000
    }
}
