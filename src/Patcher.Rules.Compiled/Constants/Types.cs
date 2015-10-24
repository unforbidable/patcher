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

namespace Patcher.Rules.Compiled.Constants
{
    /// <summary>
    /// Defines types that may be used in several contexts, e.g. when declaring <b>Script</b> properties and when specifying the type of a <b>Global Variable</b>.
    /// </summary>
    public enum Types
    {
        /// <summary>
        /// Indicates that the type is undefined.
        /// </summary>
        None,
        /// <summary>
        /// Represents an arbitrary object the specific kind of which depends on the context.
        /// </summary>
        Object,
        /// <summary>
        /// Represents a string of characters, i.e. a text.
        /// </summary>
        String,
        /// <summary>
        /// Represents an integer number.
        /// </summary>
        Int,
        /// <summary>
        /// Represents a floating point number.
        /// </summary>
        Float,
        /// <summary>
        /// Represents a boolean value, i.e, <c>true</c> or <c>false</c>.
        /// </summary>
        Bool,
        /// <summary>
        /// Represents a short integer number.
        /// </summary>
        Short,
        /// <summary>
        /// Represents a sequence of arbitrary objects the specific kind of which depends on the context.
        /// </summary>
        ArrayOfObject,
        /// <summary>
        /// Represents a sequence of strings of characters, i.e. a sequence of texts.
        /// </summary>
        ArrayOfString,
        /// <summary>
        /// Represents a sequence of integer numbers.
        /// </summary>
        ArrayOfInt,
        /// <summary>
        /// Represents a sequence of floating point numbers.
        /// </summary>
        ArrayOfFloat,
        /// <summary>
        /// Represents a sequence of boolean values, i.e. <c>true</c> or <c>false</c>.
        /// </summary>
        ArrayOfBool,
        /// <summary>
        /// Represents an array of short integer numbers.
        /// </summary>
        ArrayOfShort
    }
}
