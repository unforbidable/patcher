
using Patcher.Rules.Compiled.Constants.Skyrim;
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
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a collection of <see cref="IWeatherSound">WeatherSounds</see>.
    /// </summary>
    /// <remarks>
    /// Weather sounds in a collection can be edited or removed within a <c>foreach</c> iteration.
    /// </remarks>
    public interface IWeatherSoundCollection : IEnumerable<IWeatherSound>
    {
        /// <summary>
        /// Gets the number of <see cref="IWeatherSound">WeatherSounds</see> in this collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Creates a new <see cref="IWeatherSound">WeatherSound</see> based on the specified <b>Sound</b> and type and adds it to this collection.
        /// </summary>
        /// <param name="sound">Sound form.</param>
        /// <param name="type">Weather type.</param>
        void Add(IForm sound, WeatherSoundTypes type);

        /// <summary>
        /// Adds an existing <see cref="IWeatherSound">WeatherSound</see> to this collection.
        /// </summary>
        /// <param name="weatherSound">Weather sound to add.</param>
        void Add(IWeatherSound weatherSound);

        /// <summary>
        /// Removes a <see cref="IWeatherSound">WeatherSound</see> from this collection.
        /// </summary>
        /// <param name="weatherSound"></param>
        void Remove(IWeatherSound weatherSound);

        /// <summary>
        /// Removes all <see cref="IWeatherSound">WeatherSounds</see> from this collection.
        /// </summary>
        void Clear();
    }
}
