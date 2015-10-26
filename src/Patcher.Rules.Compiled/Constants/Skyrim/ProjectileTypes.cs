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
    /// Defines <b>Projectile</b> types.
    /// </summary>
    public enum ProjectileTypes
    {
        /// <summary>
        /// Indicates the <b>Projectile</b> is a missile going in a straigt line.
        /// </summary>
        Missile,
        /// <summary>
        /// Indicates the <b>Projectile</b> is a thrown object with a high arc.
        /// </summary>
        Lobber,
        /// <summary>
        /// Indicates the <b>Projectile</b> is a beam.
        /// </summary>
        Beam,
        /// <summary>
        /// Indicates the <b>Projectile</b> is a flame.
        /// </summary>
        Flame,
        /// <summary>
        /// Indicates the <b>Projectile</b> has the form of a cone.
        /// </summary>
        Cone,
        /// <summary>
        /// Indicates the <b>Projectile</b> is a barrier.
        /// </summary>
        Barrier,
        /// <summary>
        /// Indicates the <b>Projectile</b> is an arrow.
        /// </summary>
        Arrow
    }
}
