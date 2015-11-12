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
    /// Represents one of the 32 layers of clouds.
    /// </summary>
    public interface IWeatherCloudLayer
    {
        /// <summary>
        /// Gets or sets the value that indicates whether this cloud layer is enabled.
        /// </summary>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets the texture of this cloud layer.
        /// </summary>
        string Texture { get; set; }
        /// <summary>
        /// Gets or sets the X-axis speed of this could layer.
        /// </summary>
        float SpeedX { get; set; }
        /// <summary>
        /// Gets or sets the Y-axis speed of this could layer.
        /// </summary>
        float SpeedY { get; set; }
        /// <summary>
        /// Gets or sets the colors of this could layer.
        /// </summary>
        IWeatherColorSet Colors { get; }
        /// <summary>
        /// Gets or sets the alpha channels of this cloud layer.
        /// </summary>
        IWeatherAlphaSet Alphas { get; }
    }
}
