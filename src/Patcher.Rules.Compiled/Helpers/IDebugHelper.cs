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

namespace Patcher.Rules.Compiled.Helpers
{
    /// <summary>
    /// Provides methods that may assist during development and troubleshooting.
    /// </summary>
    /// <remarks>
    /// These methods work only if the tool is run in debug mode. See option <code>--debug</code> form more details.
    /// </remarks>
    public interface IDebugHelper
    {
        /// <summary>
        /// Signals a breakpoint to the attached debugger. If no debugger is attached this method has no effect.
        /// </summary>
        void Break();

        /// <summary>
        /// Checks the specified condition and if the specified condition is false, raises an error that will abort the execution of the current rule.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        /// <param name="text">Message that describes the error that will be raised.</param>
        void Assert(bool condition, string text);

        /// <summary>
        /// Prints the specified message in the console.
        /// </summary>
        /// <param name="text"></param>
        void Message(string text);

        /// <summary>
        /// Prints the content of the specified <c>object</c> in the console, including all its properties, be it scalars, collections and other data structures.
        /// </summary>
        /// <param name="value"></param>
        void Dump(object value);

        /// <summary>
        /// Prints the explixitly given name and the content of the specified <c>object</c> in the console, including all its properties, be it scalars, collections and other data structures.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        void Dump(object value, string name);
    }
}