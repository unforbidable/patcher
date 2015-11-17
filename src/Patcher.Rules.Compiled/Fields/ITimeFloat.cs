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
    /// Represents a floating point value at a certain point in time.
    /// </summary>
    public interface ITimeFloat
    {
        /// <summary>
        /// Gets or sets the which indicates when the value is valid represented as a value between 0.0f and 1.0f.
        /// </summary>
        float Time { get; set; }
        /// <summary>
        /// Gets or sets the floating point value.
        /// </summary>
        float Value { get; set; }
    }
}
