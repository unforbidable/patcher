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

namespace Patcher.Rules.Compiled.Fields.Fallout4
{
    /// <summary>
    /// Represents a set of weather fresnel power for each of the eight parts of the day: early dawn, dawn, late dawn, day, early dusk, late dusk and night.
    /// </summary>
    public interface IWeatherFresnelSet
    {
        /// <summary>
        /// Gets or sets the weather fresnel power during the early dawn.
        /// </summary>
        float EarlyDawn { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the dawn.
        /// </summary>
        float Dawn { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the late dawn.
        /// </summary>
        float LateDawn { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the day.
        /// </summary>
        float Day { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the early dusk.
        /// </summary>
        float EarlyDusk { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the dusk.
        /// </summary>
        float Dusk { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the late dusk.
        /// </summary>
        float LateDusk { get; set; }
        /// <summary>
        /// Gets or sets the weather fresnel power during the night.
        /// </summary>
        float Night { get; set; }
    }
}
