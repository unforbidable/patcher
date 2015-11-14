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

using Patcher.Data.Plugins.Content.Records.Fallout4;
using Patcher.Rules.Compiled.Forms.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Proxies.Fields;

namespace Patcher.Rules.Proxies.Forms.Fallout4
{
    [Proxy(typeof(IImgs))]
    public class ImgsProxy : FormProxy<Imgs>, IImgs
    {
        public float BloomBlurRadius
        {
            get
            {
                return record.BloomBlurRadius;
            }

            set
            {
                EnsureWritable();
                record.BloomBlurRadius = value;
            }
        }

        public float BloomScale
        {
            get
            {
                return record.BloomScale;
            }

            set
            {
                EnsureWritable();
                record.BloomScale = value;
            }
        }

        public float BloomThreshold
        {
            get
            {
                return record.BloomThreshold;
            }

            set
            {
                EnsureWritable();
                record.BloomThreshold = value;
            }
        }

        public float Brightness
        {
            get
            {
                return record.Brightness;
            }

            set
            {
                EnsureWritable();
                record.Brightness = value;
            }
        }

        public float Contrast
        {
            get
            {
                return record.Contrast;
            }

            set
            {
                EnsureWritable();
                record.Contrast = value;
            }
        }

        public float DepthOfFieldBlurRadius
        {
            get
            {
                return record.DepthOfFieldBlurRadius;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldBlurRadius = value;
            }
        }

        public float DepthOfFieldDistance
        {
            get
            {
                return record.DepthOfFieldDistance;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldDistance = value;
            }
        }

        public float DepthOfFieldRange
        {
            get
            {
                return record.DepthOfFieldRange;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldRange = value;
            }
        }

        public float DepthOfFieldStrength
        {
            get
            {
                return record.DepthOfFieldStrength;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldStrength = value;
            }
        }

        public float DepthOfFieldUnknown1
        {
            get
            {
                return record.DepthOfFieldUnknown1;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldUnknown1 = value;
            }
        }

        public float DepthOfFieldUnknown2
        {
            get
            {
                return record.DepthOfFieldUnknown2;
            }

            set
            {
                EnsureWritable();
                record.DepthOfFieldUnknown2 = value;
            }
        }

        public float EyeAdaptSpeed
        {
            get
            {
                return record.EyeAdaptSpeed;
            }

            set
            {
                EnsureWritable();
                record.EyeAdaptSpeed = value;
            }
        }

        public float EyeAdaptStrength
        {
            get
            {
                return record.EyeAdaptStrength;
            }

            set
            {
                EnsureWritable();
                record.EyeAdaptStrength = value;
            }
        }

        public bool IsDepthOfFieldSkyDisabled
        {
            get
            {
                return record.IsDepthOfFieldSkyDisabled;
            }

            set
            {
                EnsureWritable();
                record.IsDepthOfFieldSkyDisabled = value;
            }
        }

        public string LookupTexture
        {
            get
            {
                return record.LookupTexture;
            }

            set
            {
                EnsureWritable();
                record.LookupTexture = value;
            }
        }

        public float ReceiveBloomThreshold
        {
            get
            {
                return record.ReceiveBloomThreshold;
            }

            set
            {
                EnsureWritable();
                record.ReceiveBloomThreshold = value;
            }
        }

        public float Saturation
        {
            get
            {
                return record.Saturation;
            }

            set
            {
                EnsureWritable();
                record.Saturation = value;
            }
        }

        public float SkyScale
        {
            get
            {
                return record.SkyScale;
            }

            set
            {
                EnsureWritable();
                record.SkyScale = value;
            }
        }

        public float SunlightScale
        {
            get
            {
                return record.SunlightScale;
            }

            set
            {
                EnsureWritable();
                record.SunlightScale = value;
            }
        }

        public float TintAmount
        {
            get
            {
                return record.TintAmount;
            }

            set
            {
                EnsureWritable();
                record.TintAmount = value;
            }
        }

        public IColor TintColor
        {
            get
            {
                return Provider.CreateProxy<ColorProxy>(Mode).With(record.TintColor);
            }

            set
            {
                EnsureWritable();

                // Simply copy colors from one adapter to the other
                record.TintColor.Red = value.Red;
                record.TintColor.Green = value.Green;
                record.TintColor.Blue = value.Blue;
            }
        }

        public float White
        {
            get
            {
                return record.White;
            }

            set
            {
                EnsureWritable();
                record.White = value;
            }
        }
    }
}
