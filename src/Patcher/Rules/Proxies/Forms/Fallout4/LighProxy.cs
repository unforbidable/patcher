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
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies.Fields;

namespace Patcher.Rules.Proxies.Forms.Fallout4
{
    [Proxy(typeof(ILigh))]
    public sealed class LighProxy : FormProxy<Ligh>, ILigh
    {
        public float Angle
        {
            get
            {
                return record.Angle;
            }

            set
            {
                EnsureWritable();
                record.Angle = value;
            }
        }

        public IColor Color
        {
            get
            {
                return Provider.CreateProxy<ColorProxy>(Mode).With(record.Color);
            }
            set
            {
                EnsureWritable();
                record.Color.Red = value.Red;
                record.Color.Green = value.Green;
                record.Color.Blue = value.Blue;
            }
        }

        public int Duration
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

        public float Fade
        {
            get
            {
                return record.Fade;
            }

            set
            {
                EnsureWritable();
                record.Fade = value;
            }
        }

        public float FallOffExponent
        {
            get
            {
                return record.FallOffExponent;
            }

            set
            {
                EnsureWritable();
                record.FallOffExponent = value;
            }
        }

        public float FlickerIntensity
        {
            get
            {
                return record.FlickerIntensity;
            }

            set
            {
                EnsureWritable();
                record.FlickerIntensity = value;
            }
        }

        public float FlickerMovement
        {
            get
            {
                return record.FlickerMovement;
            }

            set
            {
                EnsureWritable();
                record.FlickerMovement = value;
            }
        }

        public float FlickerPeriod
        {
            get
            {
                return record.FlickerPeriod;
            }

            set
            {
                EnsureWritable();
                record.FlickerPeriod = value;
            }
        }

        public string FullName
        {
            get
            {
                return record.FullName;
            }

            set
            {
                EnsureWritable();
                record.FullName = value;
            }
        }

        public IFormCollection<IKywd> Keywords
        {
            get
            {
                return Provider.CreateFormCollectionProxy<IKywd>(Mode, record.Keywords.Items);
            }
            set
            {
                EnsureWritable();
                record.Keywords.Items = value.ToFormIdList();
            }
        }

        public ILens LensFlare
        {
            get
            {
                return Provider.CreateReferenceProxy<ILens>(record.LensFlare);
            }
            set
            {
                EnsureWritable();
                record.LensFlare = value.ToFormId();
            }
        }

        public float NearClip
        {
            get
            {
                return record.NearClip;
            }

            set
            {
                EnsureWritable();
                record.NearClip = value;
            }
        }

        public IObjectBounds ObjectBounds
        {
            get
            {
                return record.CreateObjectBoundsProxy(this);
            }
            set
            {
                EnsureWritable();
                record.UpdateFromObjectBoundsProxy(value);
            }
        }

        public uint Radius
        {
            get
            {
                return record.Radius;
            }

            set
            {
                EnsureWritable();
                record.Radius = value;
            }
        }

        public string StencilTexture
        {
            get
            {
                return record.StencilTexture;
            }

            set
            {
                EnsureWritable();
                record.StencilTexture = value;
            }
        }

        public float Unknown1
        {
            get
            {
                return record.Unknown1;
            }

            set
            {
                EnsureWritable();
                record.Unknown1 = value;
            }
        }

        public ITrns Unknown10
        {
            get
            {
                return Provider.CreateReferenceProxy<ITrns>(record.Trns);
            }
            set
            {
                EnsureWritable();
                record.Trns = value.ToFormId();
            }
        }

        public float Unknown2
        {
            get
            {
                return record.Unknown2;
            }

            set
            {
                EnsureWritable();
                record.Unknown2 = value;
            }
        }

        public float Unknown3
        {
            get
            {
                return record.Unknown3;
            }

            set
            {
                EnsureWritable();
                record.Unknown3 = value;
            }
        }

        public float Unknown4
        {
            get
            {
                return record.Unknown4;
            }

            set
            {
                EnsureWritable();
                record.Unknown4 = value;
            }
        }

        public int Value
        {
            get
            {
                return (int)record.Value;
            }

            set
            {
                EnsureWritable();
                record.Value = (uint)value;
            }
        }

        public IGdry GodRays
        {
            get
            {
                return Provider.CreateReferenceProxy<IGdry>(record.GodRays);
            }
            set
            {
                EnsureWritable();
                record.GodRays = value.ToFormId();
            }
        }

        public float Weight
        {
            get
            {
                return record.Weight;
            }

            set
            {
                EnsureWritable();
                record.Weight = value;
            }
        }

        public string WorldModel
        {
            get
            {
                return record.WorldModel;
            }

            set
            {
                EnsureWritable();
                record.WorldModel = value;
            }
        }
    }
}
