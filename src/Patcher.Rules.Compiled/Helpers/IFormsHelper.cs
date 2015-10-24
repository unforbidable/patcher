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

namespace Patcher.Rules.Compiled.Helpers
{
    /// <summary>
    /// Provides methods that can be used to look up and retrieve any available forms.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Method <code>Forms.FindAll()</code> will retrieve a huge number of forms and on its own is not very practical 
    /// and iterating such a huge list is not very effective. This method is used as a starting point in a chain of filtering methods 
    /// that are used to retrieve a more specific set of forms instead.
    /// </p>
    /// <p>
    /// While any form can be retrieved through the <code>Forms</code> helper, keep in mind they cannot be modified. 
    /// They can only be tagged or assigned as a reference or its parts copied to the properties of the <code>Target</code> form.
    /// </p>
    /// </remarks>
    public interface IFormsHelper
    {
        /// <summary>
        /// Retrieves the form which has the specified Form ID, in the main plugin, or returns <c>null</c> if no such form could be found.
        /// </summary>
        /// <param name="formId">Form ID of the retrieved form.</param>
        /// <returns>Returns retrieved form or <c>null</c> if such form could not be found.</returns>
        IForm Find(uint formId);

        /// <summary>
        /// Retrieves the form which has the specified Editor ID, or returns <c>null</c> if no such form could be found. 
        /// If more forms share the specified Editor ID, the form loaded or created last will be retrieved.
        /// </summary>
        /// <param name="editorId">Editor ID of the retreived form.</param>
        /// <returns>Returns retrieved form or <c>null</c> if such form could not be found.</returns>
        IForm Find(string editorId);

        /// <summary>
        /// Retrieves the form which has the specified Form ID and belongs to the specified plugin, or returns <c>null</c> if no such form could be found.
        /// </summary>
        /// <param name="plugin">Plugin where the retrieved from belongs to.</param>
        /// <param name="formId">Form ID of the retrieved form.</param>
        /// <returns>Returns retrieved form or <c>null</c> if such form could not be found.</returns>
        IForm Find(string plugin, uint formId);

        /// <summary>
        /// Retrieves all forms that are currently available.
        /// </summary>
        /// <returns>Returns a mixed form collection of all forms.</returns>
        IFormCollection<IForm> FindAll();

        /// <summary>
        /// Retrieves all forms that have been tagged with the specified text. If no forms have been tagged with the specified text the returned collection will be empty.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Returns a mixed form collection of all forms that have been tagged with the specified text.</returns>
        IFormCollection<IForm> FindAllHavingTag(string text);
    }
}