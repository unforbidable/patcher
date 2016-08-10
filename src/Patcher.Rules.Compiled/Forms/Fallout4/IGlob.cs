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

namespace Patcher.Rules.Compiled.Forms.Fallout4
{
    /// <summary>
    /// Represents a <b>Global Variable</b> form.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Valid types of a <b>Global Variable</b> are <code>Types.Short</code>, <code>Types.Int</code> and <code>Types.Float</code>. 
    /// When creating a new <b>Global Variable</b> form property <code>Type</code> should be set before property <code>Value</code> is be set.
    /// </p>
    /// <p>
    /// Type <code>Types.None</code> is also valid and variables with this type have the same behavior as <code>Types.Float</code>.
    /// In fact, this is the default type for <b>Global Variables</b> in Fallout 4 Creation Kit and it cannot be changed.
    /// </p>
    /// <p>
    /// Value should be set to a value of the correct type as specified by property <code>Type</code>. 
    /// The engine will try to convert the value if necessary to avoid run-time errors 
    /// but large numbers may get truncated or precision may get lost if the type cannot sustain the value.
    /// </p>
    /// <p>
    /// Changing the value of a constant <b>Global Variable</b> value results in a warning.
    /// </p>
    /// </remarks>
    public interface IGlob : IForm
    {
        /// <summary>
        /// Gets or sets the value which indicates whether this <b>Global Variable</b> is a constant.
        /// </summary>
        bool IsConstant { get; set; }

        /// <summary>
        /// Gets or sets the type of this <b>Global Variable</b>.
        /// </summary>
        Types Type { get; set; }

        /// <summary>
        /// Gets or sets the value of this <b>Global Variable</b>.
        /// </summary>
        dynamic Value { get; set; }
    }
}
