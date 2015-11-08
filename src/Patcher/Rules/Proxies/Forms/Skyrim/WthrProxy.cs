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

using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies.Fields.Skyrim;
using Patcher.Rules.Proxies.Fields;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IWthr))]
    public sealed class WthrProxy : FormProxy<Wthr>, IWthr
    {
        protected override void OnRecordChanged()
        {
            // Reset cached subproxies
            ambientColorsX1 = null;
            ambientColorsX2 = null;
            ambientColorsY1 = null;
            ambientColorsY2 = null;
            ambientColorsZ1 = null;
            ambientColorsZ2 = null;
            specularColors = null;
            fresnelPowers = null;
            lightningColor = null;

            base.OnRecordChanged();
        }

        WeatherColorQuadProxy ambientColorsX1 = null;
        WeatherColorQuadProxy ambientColorsX2 = null;
        WeatherColorQuadProxy ambientColorsY1 = null;
        WeatherColorQuadProxy ambientColorsY2 = null;
        WeatherColorQuadProxy ambientColorsZ1 = null;
        WeatherColorQuadProxy ambientColorsZ2 = null;
        WeatherColorQuadProxy specularColors = null;
        WeatherFresnelQuadProxy fresnelPowers = null;
        ColorProxy lightningColor = null;

        public IWeatherColorQuad AmbientColorsX1
        {
            get
            {
                if (ambientColorsX1 == null)
                    ambientColorsX1 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorX1());
                return ambientColorsX1;
            }
        }

        public IWeatherColorQuad AmbientColorsX2
        {
            get
            {
                if (ambientColorsX2 == null)
                    ambientColorsX2 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorX2());
                return ambientColorsX2;
            }

        }

        public IWeatherColorQuad AmbientColorsY1
        {
            get
            {
                if (ambientColorsY1 == null)
                    ambientColorsY1 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorY1());
                return ambientColorsY1;
            }

        }

        public IWeatherColorQuad AmbientColorsY2
        {
            get
            {
                if (ambientColorsY2 == null)
                    ambientColorsY2 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorY2());
                return ambientColorsY2;
            }

        }

        public IWeatherColorQuad AmbientColorsZ1
        {
            get
            {
                if (ambientColorsZ1 == null)
                    ambientColorsZ1 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorZ2());
                return ambientColorsZ1;
            }

        }

        public IWeatherColorQuad AmbientColorsZ2
        {
            get
            {
                if (ambientColorsZ2 == null)
                    ambientColorsZ2 = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetAmbientColorZ2());
                return ambientColorsZ2;
            }

        }

        public bool AuroraFollowsSun
        {
            get
            {
                return record.AuroraFollowsSun;
            }

            set
            {
                EnsureWritable();
                record.AuroraFollowsSun = value;
            }
        }

        public IImgs DayImageSpace
        {
            get
            {
                return Provider.CreateReferenceProxy<IImgs>(record.DayImageSpace);
            }

            set
            {
                EnsureWritable();
                record.DayImageSpace = value.ToFormId();
            }
        }

        public bool EffectsAlwaysVisible
        {
            get
            {
                return record.EffectsAlwaysVisible;
            }

            set
            {
                EnsureWritable();
                record.EffectsAlwaysVisible = value;
            }
        }

        public float FogDayFar
        {
            get
            {
                return record.FogDayFar;
            }

            set
            {
                EnsureWritable();
                record.FogDayFar = value;
            }
        }

        public float FogDayMax
        {
            get
            {
                return record.FogDayMax;
            }

            set
            {
                EnsureWritable();
                record.FogDayMax = value;
            }
        }

        public float FogDayNear
        {
            get
            {
                return record.FogDayNear;
            }

            set
            {
                EnsureWritable();
                record.FogDayNear = value;
            }
        }

        public float FogDayPow
        {
            get
            {
                return record.FogDayPow;
            }

            set
            {
                EnsureWritable();
                record.FogDayPow = value;
            }
        }

        public float FogNightFar
        {
            get
            {
                return record.FogNightFar;
            }

            set
            {
                EnsureWritable();
                record.FogNightFar = value;
            }
        }

        public float FogNightMax
        {
            get
            {
                return record.FogNightMax;
            }

            set
            {
                EnsureWritable();
                record.FogNightMax = value;
            }
        }

        public float FogNightNear
        {
            get
            {
                return record.FogNightNear;
            }

            set
            {
                EnsureWritable();
                record.FogNightNear = value;
            }
        }

        public float FogNightPow
        {
            get
            {
                return record.FogNightPow;
            }

            set
            {
                EnsureWritable();
                record.FogNightPow = value;
            }
        }

        public IWeatherFresnelQuad FresnelPowers
        {
            get

            {
                if (fresnelPowers == null)
                    fresnelPowers = Provider.CreateProxy<WeatherFresnelQuadProxy>(Mode).With(record.GetFresnelPower());
                return fresnelPowers;
            }
        }

        public bool IsCloudy
        {
            get
            {
                return record.IsCloudy;
            }

            set
            {
                EnsureWritable();
                record.IsCloudy = value;
            }
        }

        public bool IsPleasant
        {
            get
            {
                return record.IsPleasant;
            }

            set
            {
                EnsureWritable();
                record.IsPleasant = value;
            }
        }

        public bool IsRainy
        {
            get
            {
                return record.IsRainy;
            }

            set
            {
                EnsureWritable();
                record.IsRainy = value;
            }
        }

        public bool IsSnowy
        {
            get
            {
                return record.IsSnowy;
            }

            set
            {
                EnsureWritable();
                record.IsSnowy = value;
            }
        }

        public IColor LightningColor
        {
            get
            {
                if (lightningColor == null)
                    lightningColor = Provider.CreateProxy<ColorProxy>(Mode).With(record.LightningColor);
                return lightningColor;
            }

            set
            {
                EnsureWritable();
                record.LightningColor.Red = value.Red;
                record.LightningColor.Green = value.Green;
                record.LightningColor.Blue = value.Blue;
            }
        }

        public IImgs NightImageSpace
        {
            get
            {
                return Provider.CreateReferenceProxy<IImgs>(record.NightImageSpace);
            }

            set
            {
                EnsureWritable();
                record.NightImageSpace = value.ToFormId();
            }
        }

        public float PrecipitationBeginFadeIn
        {
            get
            {
                return record.PrecipitationBeginFadeIn;
            }

            set
            {
                EnsureWritable();
                record.PrecipitationBeginFadeIn = value;
            }
        }

        public float PrecipitationEndFadeOut
        {
            get
            {
                return record.PrecipitationEndFadeOut;
            }

            set
            {
                EnsureWritable();
                record.PrecipitationEndFadeOut = value;
            }
        }

        public ISpgd PrecipitationParticle
        {
            get
            {
                return Provider.CreateReferenceProxy<ISpgd>(record.PrecipitationParticle);
            }

            set
            {
                EnsureWritable();
                record.PrecipitationParticle = value.ToFormId();
            }
        }

        public IFormCollection<IStat> SkyStatics
        {
            get
            {
                return Provider.CreateFormCollectionProxy<IStat>(Mode, record.SkyStatics);
            }
            set
            {
                EnsureWritable();
                record.SkyStatics = value.ToFormIdList();
            }
        }

        public IWeatherSoundCollection Sounds
        {
            get
            {
                var proxy = Provider.CreateProxy<WeatherSoundCollectionProxy>(Mode);
                proxy.Target = record;
                return proxy;
            }

            set
            {
                EnsureWritable();

                if (value == null)
                    throw new ArgumentNullException("value", "Cannot set material collection to null.");

                var otherCollection = (WeatherSoundCollectionProxy)value;

                if (otherCollection == null)
                {
                    // Nothing to copy
                }
                else if (record.Sounds == otherCollection.Target.Sounds)
                {
                    // Same collection?
                    Log.Warning("Cannot copy a collection into itself.");
                }
                else
                {
                    // Copy sounds
                    record.Sounds = otherCollection.CopyFieldCollection();
                }
            }
        }

        public IWeatherColorQuad SpecularColors
        {
            get
            {
                if (specularColors == null)
                    specularColors = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetSpecularColor());
                return specularColors;
            }
        }

        public float SunDamage
        {
            get
            {
                return record.SunDamage;
            }

            set
            {
                EnsureWritable();
                record.SunDamage = value;
            }
        }

        public float SunGlare
        {
            get
            {
                return record.SunGlare;
            }

            set
            {
                EnsureWritable();
                record.SunGlare = value;
            }
        }

        public IImgs SunriseImageSpace
        {
            get
            {
                return Provider.CreateReferenceProxy<IImgs>(record.SunriseImageSpace);
            }

            set
            {
                EnsureWritable();
                record.SunriseImageSpace = value.ToFormId();
            }
        }

        public IImgs SunsetImageSpace
        {
            get
            {
                return Provider.CreateReferenceProxy<IImgs>(record.SunsetImageSpace);
            }

            set
            {
                EnsureWritable();
                record.SunsetImageSpace = value.ToFormId();
            }
        }

        public float ThunderBeginFadeIn
        {
            get
            {
                return record.ThunderBeginFadeIn;
            }

            set
            {
                EnsureWritable();
                record.ThunderBeginFadeIn = value;
            }
        }

        public float ThunderEndFadeOut
        {
            get
            {
                return record.ThunderEndFadeOut;
            }

            set
            {
                EnsureWritable();
                record.ThunderEndFadeOut = value;
            }
        }

        public float ThunderFrequency
        {
            get
            {
                return record.ThunderFrequency;
            }

            set
            {
                EnsureWritable();
                record.ThunderFrequency = value;
            }
        }

        public float TransDelta
        {
            get
            {
                return record.TransDelta;
            }

            set
            {
                EnsureWritable();
                record.TransDelta = value;
            }
        }

        public IRfct VisualEffect
        {
            get
            {
                return Provider.CreateReferenceProxy<IRfct>(record.VisualEffect);
            }

            set
            {
                EnsureWritable();
                record.VisualEffect = value.ToFormId();
            }
        }

        public float WindDirection
        {
            get
            {
                return record.WindDirection;
            }

            set
            {
                EnsureWritable();
                record.WindDirection = value;
            }
        }

        public float WindDirectionRange
        {
            get
            {
                return record.WindDirectionRange;
            }

            set
            {
                EnsureWritable();
                record.WindDirectionRange = value;
            }
        }

        public float WindSpeed
        {
            get
            {
                return record.WindSpeed;
            }

            set
            {
                EnsureWritable();
                record.WindSpeed = value;
            }
        }

        public IEnumerable<IWeatherCloudLayer> CloudLayers
        {
            get
            {
                return record.GetCloudLayers().Select(l => Provider.CreateProxy<WeatherCloudLayerProxy>(Mode).With(l));
            }
        }

        public IEnumerable<IWeatherColorQuad> Colors
        {
            get
            {
                return record.GetColors().Select(c => Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(c));
            }
        }

        public IWeatherCloudLayer GetCloudLayer(int layer)
        {
            return Provider.CreateProxy<WeatherCloudLayerProxy>(Mode).With(record.GetCloudLayer(layer));
        }

        public IWeatherColorQuad GetWeatherColors(int component)
        {
            return Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(record.GetColor(component));
        }


    }
}
