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
    /// Describes the ambient light during a weather.
    /// </summary>
    public interface IWeatherAmbientLight
    {
        /// <summary>
        /// Gets the colors of the ambient light in the negative X-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsX1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive X-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsX2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Y-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsY1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Y-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsY2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Z-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsZ1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Z-axis direction.
        /// </summary>
        IWeatherColorQuad ColorsZ2 { get; }
        /// <summary>
        /// Gets the specular lighting colors.
        /// </summary>
        IWeatherColorQuad SpecularColors { get; }
        /// <summary>
        /// Gets the Fresnel powers.
        /// </summary>
        IWeatherFresnelQuad FresnelPowers { get; }
    }
}
