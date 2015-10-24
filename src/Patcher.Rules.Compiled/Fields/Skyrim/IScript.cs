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
using Patcher.Rules.Compiled.Forms;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a Papyrus script with parameters.
    /// </summary>
    /// <remarks>
    /// <p>
    /// When a new script is added to a script with method <code>Add()</code> or 
    /// created with <see cref="Helpers.Skyrim.IEngineHelper"/> method <code>Engine.CreateScript()</code> properties are not added automatically. 
    /// Properties must be added manually with method <code>AddProperty()</code>.
    /// </p>
    /// <p>
    /// The type of a script property can be any of the following: <code>Types.Int</code>, <code>Types.Float</code>, <code>Types.String</code>, 
    /// <code>Types.Bool</code>, <code>Types.Object</code>, <code>Types.ArrayOfInt</code>, <code>Types.ArrayOfFloat</code>, <code>Types.ArrayOfString</code>, 
    /// <code>Types.ArrayOfBool</code>, <code>Types.ArrayOfObject</code>.
    /// Types <code>Type.Object</code> and <code>Types.ArrayOfObject</code> represent a pair of Alias ID and a Form reference, but currently Alias IDs are not supported.
    /// </p>
    /// <p>
    /// Methods <code>SetProperty()</code> will raise an error if a value of a wrong type is being set to a property. 
    /// A float value can be assigned to a property of <code>Types.Int</code> however the value might get truncated.
    /// When index <code>-1</code> is specified when adding a value to an array property the new value will be added at the end of the array.
    /// </p>
    /// <p>
    /// Methods can be chained together into a single statement in a fluent interface fashion.
    /// </p>
    /// </remarks>
    public interface IScript
    {
        /// <summary>
        /// Gets the name of this script.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Adds a property to this script.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="type">Type of the property.</param>
        /// <returns>Returns this script.</returns>
        IScript AddProperty(string name, Types type);

        /// <summary>
        /// Sets the value of a scalar property with the specific name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, int value);

        /// <summary>
        /// Sets the value of a scalar property with the specific name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, string value);

        /// <summary>
        /// Sets the value of a scalar property with the specific name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, float value);

        /// <summary>
        /// Sets the value of a scalar property with the specific name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, bool value);

        /// <summary>
        /// Sets the value of a scalar property with the specific name.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, IForm value);

        /// <summary>
        /// Sets a value of an array property with the specific name, at the specified given index, or appends the value at the end, if the specified index is <code>-1</code>.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="index">Index in the array property.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, int value, int index);

        /// <summary>
        /// Sets a value of an array property with the specific name, at the specified given index, or appends the value at the end, if the specified index is <code>-1</code>.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="index">Index in the array property.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, string value, int index);

        /// <summary>
        /// Sets a value of an array property with the specific name, at the specified given index, or appends the value at the end, if the specified index is <code>-1</code>.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="index">Index in the array property.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, float value, int index);

        /// <summary>
        /// Sets a value of an array property with the specific name, at the specified given index, or appends the value at the end, if the specified index is <code>-1</code>.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="index">Index in the array property.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, bool value, int index);

        /// <summary>
        /// Sets a value of an array property with the specific name, at the specified given index, or appends the value at the end, if the specified index is <code>-1</code>.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to set.</param>
        /// <param name="index">Index in the array property.</param>
        /// <returns>Returns this script.</returns>
        IScript SetProperty(string name, IForm value, int index);

        /// <summary>
        /// Resets a property so that the default value will be used.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>Returns this script.</returns>
        IScript ResetProperty(string name);
    }
}
