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
    /// </summary>
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
        /// Tags this form with a text.
        /// </summary>
        /// <param name="text">Text used to tag the form with.</param>
        void Tag(string text);

        /// <summary>
        /// Determines whether a form has been tagged with a text.
        /// </summary>
        /// <param name="text">Text of the tag to determine.</param>
        /// <returns>Returns <c>true</c> if the form has been taged with specified text, otherwise returns <c>false</c>.</returns>
        bool HasTag(string text);
    }
}
