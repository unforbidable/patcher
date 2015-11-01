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

namespace Patcher.Rules.Compiled.Forms
{
    /// <summary>
    /// Represents a form the type of which is either unknown, unsupported or unresolved.
    /// To convert an unknown form to a specific form use the appropriate extension method.
    /// Properties and methods provided by this class are available to all forms.
    /// </summary>
    /// <remarks>
    /// <p>
    /// When a variable refers to an unknown or generic form it means that only the common form properties and methods are available. 
    /// In order to access all properties of the form the variable must be cast to the specific form using the appropriate extension method.
    /// </p>
    /// <p>
    /// Unknow or generic forms can be retrieved via the <see cref="Helpers.IFormsHelper"/> helper method <code>Forms.Find()</code>
    /// or by iterating a mixed <see cref="IFormCollection{TForm}"/> such as the collection of forms associated with a <b>Form List</b> form.
    /// </p>
    /// <p>
    /// Unsupported forms are forms that have been loaded but the appliaction does not know anything about such form. 
    /// Unsupported forms can be assigned to properties of supported forms or added to mixed <see cref="IFormCollection{TForm}"/>.
    /// Unsupported forms cannot be converted to a specific form.
    /// </p>
    /// <p>
    /// Unresolved forms are threaded the same way unsupported forms are treated, except that their Editor ID is always <c>null</c>.
    /// Unresolved forms come about from references to forms that do not exist due to an error in a plugin, 
    /// or because the tool deliberately skipped these forms while loading plugin.
    /// Saving plugin that reference unresolved forms will produce a warning, but will work as intended as long as the form really exists.
    /// </p>
    /// </remarks>
    public interface IForm
    {
        /// <summary>
        /// Gets the Form ID associated with this form.
        /// </summary>
        uint FormId { get; }

        /// <summary>
        /// Gets or sets the Editor ID associated with this form.
        /// </summary>
        string EditorId { get; set; }

        /// <summary>
        /// Tags this form with the specified text.
        /// </summary>
        /// <param name="text">Text used to tag the form with.</param>
        void Tag(string text);

        /// <summary>
        /// Determines whether this form has been tagged with the specified text.
        /// </summary>
        /// <param name="text">Text of the tag to determine.</param>
        /// <returns>Returns <c>true</c> if the form has been taged with specified text, otherwise returns <c>false</c>.</returns>
        bool HasTag(string text);

        /// <summary>
        /// Converts a unknown form to a specific form kind.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>() where T: class, IForm;
    }
}
