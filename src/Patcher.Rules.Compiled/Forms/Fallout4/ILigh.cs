
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

using Patcher.Rules.Compiled.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Fallout4
{
    /// <summary>
    /// Represents a <b>Light</b> form. 
    /// </summary>
    public interface ILigh : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Light</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }
        /// <summary>
        /// Gets or sets the in-game name of this <b>Light</b>.
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IKywd}"/> containing <b>Keywords</b> associated with this <b>Light</b>.
        /// </summary>
        IFormCollection<IKywd> Keywords { get; set; }
        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Light</b>.
        /// </summary>
        string WorldModel { get; set; }
        /// <summary>
        /// Gets or sets the weight of this <b>Light</b>.
        /// </summary>
        float Weight { get; set; }
        /// <summary>
        /// Gets or sets the value of this <b>Light</b>.
        /// </summary>
        int Value { get; set; }
        /// <summary>
        /// Gets or sets an unknown record of type <see cref="ITrns"/>.
        /// </summary>
        ITrns Unknown10 { get; set; }
        /// <summary>
        /// Gets or sets the fade value.
        /// </summary>
        float Fade { get; set; }
        /// <summary>
        /// Gets or sets the path to the stencil (GOBO) texture (.nif) file.
        /// </summary>
        string StencilTexture { get; set; }
        /// <summary>
        /// Gets or sets the <b>Lens Flare</b> effect associated with this <b>Light</b>.
        /// </summary>
        ILens LensFlare { get; set; }
        /// <summary>
        /// Gets or sets the <b>God Rays</b> effect associated with this <b>Light</b>.
        /// </summary>
        IGdry GodRays { get; set; }
        /// <summary>
        /// Gets or sets the duration of this <b>Light</b>. Value <c>-1</c> represents infinite duration.
        /// </summary>
        int Duration { get; set; }
        /// <summary>
        /// Gets or sets the radius of this <b>Light</b>.
        /// </summary>
        uint Radius { get; set; }
        /// <summary>
        /// Gets or sets the color of this <b>Light</b>.
        /// </summary>
        IColor Color { get; set; }
        /// <summary>
        /// Gets or sets the Fall Off Exponent of this <b>Light</b>.
        /// </summary>
        float FallOffExponent { get; set; }
        /// <summary>
        /// Gets or sets the angle of this <b>Light</b>.
        /// </summary>
        float Angle { get; set; }
        /// <summary>
        /// Gets or sets the near clip of this <b>Light</b>.
        /// </summary>
        float NearClip { get; set; }
        /// <summary>
        /// Gets or sets the flicker period of this <b>Light</b>.
        /// </summary>
        float FlickerPeriod { get; set; }
        /// <summary>
        /// Gets or sets the flicker intensity of this <b>Light</b>.
        /// </summary>
        float FlickerIntensity { get; set; }
        /// <summary>
        /// Gets or sets the flicker movement of this <b>Light</b>.
        /// </summary>
        float FlickerMovement { get; set; }
        /// <summary>
        /// Gets or sets an unknown value associated with this <b>Light</b>.
        /// </summary>
        float Unknown1 { get; set; }
        /// <summary>
        /// Gets or sets an unknown value associated with this <b>Light</b>.
        /// </summary>
        float Unknown2 { get; set; }
        /// <summary>
        /// Gets or sets an unknown value associated with this <b>Light</b>.
        /// </summary>
        float Unknown3 { get; set; }
        /// <summary>
        /// Gets or sets an unknown value associated with this <b>Light</b>.
        /// </summary>
        float Unknown4 { get; set; }
    }
}
