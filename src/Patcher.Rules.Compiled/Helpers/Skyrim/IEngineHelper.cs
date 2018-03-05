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

using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers.Skyrim
{
    /// <summary>
    /// Provides methods that allow new objects be created.
    /// </summary>
    public interface IEngineHelper
    {
        /// <summary>
        /// Creates a new <see cref="IEffect"/> based on a <b>Magic Effect</b> with given magnitude, area and duration, which can be added to an <see cref="IEffectCollection"/>.
        /// </summary>
        /// <param name="baseEffect">Base <b>Magic Effect</b> to use for the new effect.</param>
        /// <param name="magnitude">Magnitude of the new effect.</param>
        /// <param name="area">Area of the new effect.</param>
        /// <param name="duration">Duration of the new effect.</param>
        /// <returns>Returns new effect.</returns>
        IEffect CreateEffect(IMgef baseEffect, float magnitude, int area, int duration);

        /// <summary>
        /// Creates a new <see cref="IMaterial"/> with the specified item and the specified count.
        /// </summary>
        /// <param name="item">Item to use as the material.</param>
        /// <param name="count">Number of items that are needed.</param>
        /// <returns>Returns new material.</returns>
        IMaterial CreateMaterial(IForm item, int count);

        /// <summary>
        /// Creates a new <see cref="IScript"/> with the specified script name.
        /// </summary>
        /// <param name="name">Name of the new script.</param>
        /// <returns>Returns new script.</returns>
        IScript CreateScript(string name);

        /// <summary>
        /// Gets the specified string parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetParam(string name, string defaultValue);
        /// <summary>
        /// Gets the specified integer parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int GetParam(string name, int defaultValue);
        /// <summary>
        /// Gets the specified floating point parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        float GetParam(string name, float defaultValue);

    }
}
