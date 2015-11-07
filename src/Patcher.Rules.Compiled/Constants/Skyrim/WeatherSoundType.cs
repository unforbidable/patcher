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
    /// Defines the types of weather sounds.
    /// </summary>
    public enum WeatherSoundTypes
    {
        /// <summary>
        /// Represents the default weather sound.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Represents the sound of the rain.
        /// </summary>
        Precipitation = 1,
        /// <summary>
        /// Represents the sound of the wind.
        /// </summary>
        Wind = 2,
        /// <summary>
        /// Represents the sound of a thunder.
        /// </summary>
        Thunder = 3,
    }
}
