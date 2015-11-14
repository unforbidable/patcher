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
    /// Represents an <b>Image Space</b> form.
    /// </summary>
    /// <remarks>
    /// The meaning of unknown properties is not known so use them with caution. 
    /// Unknown properties will be removed and replaced with proper properties as soon as more information is available.
    /// </remarks>
    public interface IImgs : IForm
    {
        /// <summary>
        /// Gets or sets the HDR eye adaptation speed.
        /// </summary>
        float EyeAdaptSpeed { get; set; }
        /// <summary>
        /// Gets or sets the HDR eye adaptation strength.
        /// </summary>
        float EyeAdaptStrength { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom blur radius.
        /// </summary>
        float BloomBlurRadius { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom threshold.
        /// </summary>
        float BloomThreshold { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom scale.
        /// </summary>
        float BloomScale { get; set; }
        /// <summary>
        /// Gets or sets the HDR reveive bloom threshold.
        /// </summary>
        float ReceiveBloomThreshold { get; set; }
        /// <summary>
        /// Gets or sets the HDR white value.
        /// </summary>
        float White { get; set; }
        /// <summary>
        /// Gets or sets the HDR sunlight scale.
        /// </summary>
        float SunlightScale { get; set; }
        /// <summary>
        /// Gets or sets the HDR sky scale.
        /// </summary>
        float SkyScale { get; set; }
        /// <summary>
        /// Gets or sets the cinematic saturation.
        /// </summary>
        float Saturation { get; set; }
        /// <summary>
        /// Gets or sets the cinematic brightness.
        /// </summary>
        float Brightness { get; set; }
        /// <summary>
        /// Gets or sets the conematic contrast.
        /// </summary>
        float Contrast { get; set; }
        /// <summary>
        /// Gets or sets the tint amount.
        /// </summary>
        float TintAmount { get; set; }
        /// <summary>
        /// Gets or sets the tint color.
        /// </summary>
        IColor TintColor { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field strength.
        /// </summary>
        float DepthOfFieldStrength { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field distance.
        /// </summary>
        float DepthOfFieldDistance { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field range.
        /// </summary>
        float DepthOfFieldRange { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field blur radius.
        /// </summary>
        float DepthOfFieldBlurRadius { get; set; }
        /// <summary>
        /// Gets or sets the value that indicates whether the sky is affected by the Depth of Field effect.
        /// </summary>
        bool IsDepthOfFieldSkyDisabled { get; set; }
        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        float DepthOfFieldUnknown1 { get; set; }
        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        float DepthOfFieldUnknown2 { get; set; }
        /// <summary>
        /// Gets or sets the LUT texture.
        /// </summary>
        string LookupTexture { get; set; }
    }
}
