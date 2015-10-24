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

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Form List</b> form.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Because property <code>Items</code> represents a mixed <see cref="IFormCollection{IForm}"/> each form which is retrieved from 
    /// the collection must be cast into its specific types before properties and methods specific to each form can be accessed.
    /// </p>
    /// </remarks>
    public interface IFlst : IForm
    {
        /// <summary>
        /// Gets or sets the mixed <see cref="IFormCollection{IForm}"/> associated with this <b>Form List</b>.
        /// </summary>
        IFormCollection<IForm> Items { get; set; }
    }
}
