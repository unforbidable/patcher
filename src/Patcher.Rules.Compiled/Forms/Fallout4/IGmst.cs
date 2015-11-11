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

using Patcher.Rules.Compiled.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Fallout4
{
    /// <summary>
    /// Represents a <b>Game Setting</b> form.
    /// </summary>
    /// <remarks>
    /// <p>
    /// The type of value is inferred from the first letter of the Editor ID that is assigned to the form. 
    /// For this reason when creating a new <b>Game Setting</b> form it in necessary to set Editor ID first 
    /// (with the appropriate first letter) so that the type is known when the value is set.
    /// </p>
    /// <p>
    /// Value should be set to a value of the correct type as specified by property <code>Type</code>. 
    /// The engine will try to convert the value if necessary to avoid run-time errors 
    /// but large numbers may get truncated or precision may get lost if the type cannot sustain the value.
    /// </p>
    /// </remarks>
    public interface IGmst : IForm
    {
        /// <summary>
        /// Gets or sets the value of this <b>Game Setting</b>.
        /// </summary>
        dynamic Value { get; set; }

        /// <summary>
        /// Gets the type of this <b>Game Setting</b>.
        /// </summary>
        Types Type { get; }
    }
}
