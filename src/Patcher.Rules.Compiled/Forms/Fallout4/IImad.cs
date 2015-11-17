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
    /// Represents an <b>Image Space Adapter</b> form.
    /// </summary>
    public interface IImad : IForm
    {
        /// <summary>
        /// Gets or sets the duration of effect.
        /// </summary>
        float Duration { get; set; }
        /// <summary>
        /// Gets or sets the X-axis offset of the center of the blur effect.
        /// </summary>
        float BlurCenterX { get; set; }
        /// <summary>
        /// Gets or sets the Y-axis offset of the center of the blur effect.
        /// </summary>
        float BlurCenterY { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether the effect is animated.
        /// </summary>
        bool IsAnimatable { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether the blur effect uses a target.
        /// </summary>
        bool IsBlurTargetted { get; set; }

        /// <summary>
        /// Retreives an enumerable collection of colors for Tint.
        /// </summary>
        IEnumerable<ITimeColor> TintColors { get; }

        /// <summary>
        /// Retreives an enumerable collection of colors for Fade.
        /// </summary>
        IEnumerable<ITimeColor> FadeColors { get; }

        /// <summary>
        /// Retreives an enumerable collection of values for Double Vision Strength.
        /// </summary>
        IEnumerable<ITimeFloat> DoubleVisionStrengthValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Blur Radius.
        /// </summary>
        IEnumerable<ITimeFloat> BlurRadiusValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Radial Blur Strength.
        /// </summary>
        IEnumerable<ITimeFloat> RadialBlurStrengthValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Radial Blur Rampup Radius.
        /// </summary>
        IEnumerable<ITimeFloat> RadialBlurRampupValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Radial Blur Rampdown Radius.
        /// </summary>
        IEnumerable<ITimeFloat> RadialBlurRampdownValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Radial Blur Start Radius.
        /// </summary>
        IEnumerable<ITimeFloat> RadialBlurStartValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Radial Blur DownStart Radius.
        /// </summary>
        IEnumerable<ITimeFloat> RadialBlurDownStartValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Depth Of Field Strength.
        /// </summary>
        IEnumerable<ITimeFloat> DepthOfFieldStrengthValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Depth Of Field Distance.
        /// </summary>
        IEnumerable<ITimeFloat> DepthOfFieldDistanceValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Depth Of Field Range.
        /// </summary>
        IEnumerable<ITimeFloat> DepthOfFieldRangeValues { get; }
        /// <summary>
        /// Retreives an enumerable collection of values for Motion Blur.
        /// </summary>
        IEnumerable<ITimeFloat> MotionBlurValues { get; }

        /// <summary>
        /// Retreives an enumerable collection of Eye Adapt multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> EyeAdaptSpeedMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Eye Adapt additives.
        /// </summary>
        IEnumerable<ITimeFloat> EyeAdaptSpeedAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Bloom Blur Radius multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> BloomBlurRadiusMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Bloom Blur Radius additives.
        /// </summary>
        IEnumerable<ITimeFloat> BloomBlurRadiusAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> BloomTresholdMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> BloomTresholdAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> BloomScaleMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> BloomScaleAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> TargetLumMinMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> TargetLumMinAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> TargetLumMaxMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> TargetLumMaxAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> SaturationMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> SaturationAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Brightness multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> BrightnessMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Brightness additives.
        /// </summary>
        IEnumerable<ITimeFloat> BrightnessAdditives { get; }
        /// <summary>
        /// Retreives an enumerable collection of Contrast multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> ContrastMultipliers { get; }
        /// <summary>
        /// Retreives an enumerable collection of Contrast additives.
        /// </summary>
        IEnumerable<ITimeFloat> ContrastAdditives { get; }
    }
}
