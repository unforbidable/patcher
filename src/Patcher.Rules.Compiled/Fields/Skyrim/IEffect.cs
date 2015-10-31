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
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Represents a custom effect based on a <b>Magic Effect</b>.
    /// </summary>
    /// <remarks>
    /// New effects can be created using the <see cref="Helpers.Skyrim.IEngineHelper"/> helper method <code>Engine.CreateEffect()</code>. 
    /// When a new custom effect has been created, conditions can be added to it, if desired. 
    /// And finally, the new effect can be added to an <see cref="IEffectCollection"/> associated with the <code>Target</code> form.
    /// </remarks>
    public interface IEffect
    {
        /// <summary>
        /// Gets or sets the base <b>Magic Effect</b> of this <b>Effect</b>.
        /// </summary>
        IMgef BaseEffect { get; set; }

        /// <summary>
        /// Gets or sets the magnitude of this <b>Effect</b>.
        /// </summary>
        float Magnitude { get; set; }

        /// <summary>
        /// Gets or sets the radius of the area covered with this <b>Effect</b>.
        /// </summary>
        int Area { get; set; }

        /// <summary>
        /// Gets or sets the duration of this <b>Effect</b>.
        /// </summary>
        int Duration { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IConditionCollection"/> contaning <see cref="ICondition">Conditions</see> that detemine when this <b>Effect</b> is active.
        /// </summary>
        IConditionCollection Conditions { get; set; }
    }
}
