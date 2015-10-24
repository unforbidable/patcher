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

using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Constructible Object</b> form.
    /// </summary>
    public interface ICobj : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IMaterialCollection"/> contaning <see cref="IMaterial">Materials</see> for this <b>Constructible Object</b>.
        /// </summary>
        IMaterialCollection Materials { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IConditionCollection"/> contaning <see cref="ICondition">Conditions</see> that detemine when this <b>Constructible Object</b> is available.
        /// </summary>
        IConditionCollection Conditions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IForm"/> that represents the item that will be produced by this <b>Constructible Object</b>.
        /// </summary>
        IForm Result { get; set; }

        /// <summary>
        /// Gets or sets the number of items this <b>Constructible Object</b> will produce.
        /// </summary>
        int ResultCount { get; set; }

        /// <summary>
        /// Gets or sets the <b>Keyword</b> that represents the crafting station or bench where this <b>Constructible Object</b> can be used.
        /// </summary>
        IKywd Workbench { get; set; }
    }
}
