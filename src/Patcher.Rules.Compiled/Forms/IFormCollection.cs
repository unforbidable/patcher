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
    /// Represents a collection of forms.
    /// A collection may contain mixed form types or only forms of a specific type.
    /// To filter and convert a mixed collection to a specific form collection use the appropriate extension method. 
    /// </summary>
    /// <typeparam name="TForm"></typeparam>
    public interface IFormCollection<out TForm> : IEnumerable<TForm> where TForm : IForm
    {
        /// <summary>
        /// Gets the number of forms in this form collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds the specified form to this form collection.
        /// </summary>
        /// <param name="form">Form to add to the collection.</param>
        void Add(IForm form);

        /// <summary>
        /// Remove the specified form from this form collection.
        /// </summary>
        /// <param name="form">Form to remove from the collection.</param>
        void Remove(IForm form);

        /// <summary>
        /// Determines whether the specified form is in this form collection.
        /// </summary>
        /// <param name="form">Form to look for in the collection.</param>
        /// <returns></returns>
        bool Contains(IForm form);

        /// <summary>
        /// Adds the form that has the specified Editor ID to this form collection.
        /// </summary>
        /// <param name="editorId">Editor ID of the form to add to the collection.</param>
        void Add(string editorId);

        /// <summary>
        /// Remove the form that has the specified Editor ID from this form collection.
        /// </summary>
        /// <param name="editorId">Editor ID of the form to remove from the collection.</param>
        void Remove(string editorId);

        /// <summary>
        /// Determine whether the form with the specified Editor ID is in this form collection.
        /// </summary>
        /// <param name="editorId">Editor ID of the from to look for in the collection.</param>
        /// <returns></returns>
        bool Contains(string editorId);

        /// <summary>
        /// INTERNAL USE. Filters the collection to the specified kind of forms.
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        IFormCollection<TOther> Of<TOther>() where TOther : IForm;

        /// <summary>
        /// Creates a new form collection based on this form collection that contains only forms that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">Predicate each from in the new form collection has to satisfy.</param>
        /// <returns></returns>
        IFormCollection<TForm> Where(Predicate<TForm> predicate);

        /// <summary>
        /// Tags all forms in this form collection with the specified text.
        /// </summary>
        /// <param name="text"></param>
        void TagAll(string text);

        /// <summary>
        /// Creates a new form collection based on this form collection that contains only forms that have been tagged with the specified text.
        /// </summary>
        /// <param name="text">Text with which each form in the new form collection has been tagged.</param>
        /// <returns></returns>
        IFormCollection<TForm> HavingTag(string text);

        // TODO: Following methods are implemented but are not consistent with Forms.Find
        // as they lack an overload that can be used to specify the plugin name
        // For now Forms.Find() should be used but generally looking up forms by Editor ID should be preferred
        //void Add(uint formId); 
        //void Remove(uint formId);
        //bool Contains(uint formId);
    }
}
