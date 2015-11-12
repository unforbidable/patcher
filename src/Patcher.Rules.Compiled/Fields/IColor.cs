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

namespace Patcher.Rules.Compiled.Fields
{
    /// <summary>
    /// Represents a color definition.
    /// </summary>
    /// <remarks>
    /// The valid value range of color components is <code>0.0f</code> through <code>1.0f</code>.
    /// </remarks>
    public interface IColor 
    {
        /// <summary>
        /// Gets or sets the red component value.
        /// </summary>
        float Red { get; set; }
        /// <summary>
        /// Gets or sets the green component value.
        /// </summary>
        float Green { get; set; }
        /// <summary>
        /// Gets or sets the blue component value.
        /// </summary>
        float Blue { get; set; }

        /// <summary>
        /// Sets each color component to the specified value.
        /// </summary>
        /// <param name="value">Value</param>
        void SetValues(float value);
        /// <summary>
        /// Multiplies each color component with the specified value.
        /// </summary>
        /// <param name="value">Multiplier</param>
        void MultiplyValues(float value);
        /// <summary>
        /// Sets each color component to the respetive specified value. 
        /// </summary>
        /// <param name="red">Red value</param>
        /// <param name="green">Green value</param>
        /// <param name="blue">Blue value</param>
        void SetValues(float red, float green, float blue);
        /// <summary>
        /// Multiplies each color component with the respective specified value.
        /// </summary>
        /// <param name="red">Red multiplier.</param>
        /// <param name="green">Green multiplier</param>
        /// <param name="blue">Blue multiplier</param>
        void MultiplyValues(float red, float green, float blue);
    }
}
