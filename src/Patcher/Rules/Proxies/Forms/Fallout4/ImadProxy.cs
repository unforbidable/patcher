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

using Patcher.Data.Plugins.Content.Records;
using Patcher.Rules.Compiled.Forms.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Proxies.Fields;

namespace Patcher.Rules.Proxies.Forms.Fallout4
{
    [Proxy(typeof(IImad))]
    public sealed class ImadProxy : FormProxy<Imad>, IImad
    {
        public IEnumerable<ITimeFloat> BloomBlurRadiusAdditives
        {
            get
            {
                return record.GetBloomBlurRadiusAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BloomBlurRadiusMultipliers
        {
            get
            {
                return record.GetBloomBlurRadiusMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BloomScaleAdditives
        {
            get
            {
                return record.GetBloomScaleAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BloomScaleMultipliers
        {
            get
            {
                return record.GetBloomScaleMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BloomTresholdAdditives
        {
            get
            {
                return record.GetBloomTresholdAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BloomTresholdMultipliers
        {
            get
            {
                return record.GetBloomTresholdMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public float BlurCenterX
        {
            get
            {
                return record.BlurCenterX;
            }

            set
            {
                EnsureWritable();
                record.BlurCenterX = value;
            }
        }

        public float BlurCenterY
        {
            get
            {
                return record.BlurCenterY;
            }

            set
            {
                EnsureWritable();
                record.BlurCenterY = value;
            }
        }

        public IEnumerable<ITimeFloat> BlurRadiusValues
        {
            get
            {
                return record.GetBlurRadius().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BrightnessAdditives
        {
            get
            {
                return record.GetBrightnessAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BrightnessMultipliers
        {
            get
            {
                return record.GetBrightnessMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> ContrastAdditives
        {
            get
            {
                return record.GetContrastAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> ContrastMultipliers
        {
            get
            {
                return record.GetContrastMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> DepthOfFieldDistanceValues
        {
            get
            {
                return record.GetDepthOfFieldDistance().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> DepthOfFieldRangeValues
        {
            get
            {
                return record.GetDepthOfFieldRange().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> DepthOfFieldStrengthValues
        {
            get
            {
                return record.GetDepthOfFieldStrength().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> DoubleVisionStrengthValues
        {
            get
            {
                return record.GetDoubleVisionStrength().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public float Duration
        {
            get
            {
                return record.Duration;
            }

            set
            {
                EnsureWritable();
                record.Duration = value;
            }
        }

        public IEnumerable<ITimeFloat> EyeAdaptSpeedAdditives
        {
            get
            {
                return record.GetEyeAdaptAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> EyeAdaptSpeedMultipliers
        {
            get
            {
                return record.GetEyeAdaptMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeColor> FadeColors
        {
            get
            {
                return record.GetFade().Select(i => Provider.CreateProxy<TimeColorProxy>(Mode).With(i));
            }
        }

        public bool IsAnimatable
        {
            get
            {
                return record.IsAnimatable;
            }

            set
            {
                EnsureWritable();
                record.IsAnimatable = value;
            }
        }

        public bool IsBlurTargetted
        {
            get
            {
                return record.IsBlurTargetted;
            }

            set
            {
                EnsureWritable();
                record.IsBlurTargetted = value;
            }
        }

        public IEnumerable<ITimeFloat> MotionBlurValues
        {
            get
            {
                return record.GetMotionBlur().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> RadialBlurDownStartValues
        {
            get
            {
                return record.GetRadialBlurDownStart().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> RadialBlurRampdownValues
        {
            get
            {
                return record.GetRadialBlurRampdown().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> RadialBlurRampupValues
        {
            get
            {
                return record.GetRadialBlurRampup().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> RadialBlurStartValues
        {
            get
            {
                return record.GetRadialBlurStart().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> RadialBlurStrengthValues
        {
            get
            {
                return record.GetRadialBlurStrength().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> SaturationAdditives
        {
            get
            {
                return record.GetSaturationAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> SaturationMultipliers
        {
            get
            {
                return record.GetSaturationMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> TargetLumMaxAdditives
        {
            get
            {
                return record.GetTargetLumMaxAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> TargetLumMaxMultipliers
        {
            get
            {
                return record.GetTargetLumMaxMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> TargetLumMinAdditives
        {
            get
            {
                return record.GetTargetLumMinAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> TargetLumMinMultipliers
        {
            get
            {
                return record.GetTargetLumMinMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeColor> TintColors
        {
            get
            {
                return record.GetTint().Select(i => Provider.CreateProxy<TimeColorProxy>(Mode).With(i));
            }
        }
    }
}
