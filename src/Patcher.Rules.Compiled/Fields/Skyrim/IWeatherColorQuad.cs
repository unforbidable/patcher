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
    /// Represents a set of weather colors for each of the four parts of the day: sunrise, day, sunset and night.
    /// </summary>
    public interface IWeatherColorQuad
    {
        /// <summary>
        /// Gets the weather color during the sunrise.
        /// </summary>
        IColor Sunrise { get; }
        /// <summary>
        /// Gets the weather color during the day.
        /// </summary>
        IColor Day { get; }
        /// <summary>
        /// Gets the weather color during the sunset.
        /// </summary>
        IColor Sunset { get; }
        /// <summary>
        /// Gets the weather color during the night.
        /// </summary>
        IColor Night { get; }
    }
}
