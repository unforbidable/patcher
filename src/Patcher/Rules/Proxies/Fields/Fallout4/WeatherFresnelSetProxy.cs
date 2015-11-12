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
using Patcher.Rules.Compiled.Fields.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields.Fallout4
{
    [Proxy(typeof(IWeatherFresnelSet))]
    public sealed class WeatherFresnelSetProxy : Proxy, IWeatherFresnelSet, IDumpabled
    {
        Wthr.FloatOctave target = null;

        internal WeatherFresnelSetProxy With(Wthr.FloatOctave target)
        {
            this.target = target;
            return this;
        }

        void IDumpabled.Dump(string name, ObjectDumper dumper)
        {
            dumper.DumpText(name, "{{ {0} }}", ToString());
        }

        public override string ToString()
        {
            return string.Format("EarlyDawn={0} Dawn={1} LateDawn={2} Day={3} EarlyDusk={4} Dusk={5} LateDusk={6} Night={7}", EarlyDawn, Dawn, LateDawn, Day, EarlyDusk, Dusk, LateDusk, Night);
        }

        public float Day
        {
            get
            {
                return target.Day;
            }

            set
            {
                EnsureWritable();
                target.Day = value;
            }
        }

        public float Night
        {
            get
            {
                return target.Night;
            }

            set
            {
                EnsureWritable();
                target.Night = value;
            }
        }

        public float Dawn
        {
            get
            {
                return target.Dawn;
            }

            set
            {
                EnsureWritable();
                target.Dawn = value;
            }
        }

        public float EarlyDawn
        {
            get
            {
                return target.EarlyDawn;
            }

            set
            {
                EnsureWritable();
                target.EarlyDawn = value;
            }
        }

        public float LateDawn
        {
            get
            {
                return target.LateDawn;
            }

            set
            {
                EnsureWritable();
                target.LateDawn = value;
            }
        }

        public float Dusk
        {
            get
            {
                return target.Dusk;
            }

            set
            {
                EnsureWritable();
                target.Dusk = value;
            }
        }

        public float EarlyDusk
        {
            get
            {
                return target.EarlyDusk;
            }

            set
            {
                EnsureWritable();
                target.EarlyDusk = value;
            }
        }

        public float LateDusk
        {
            get
            {
                return target.LateDusk;
            }

            set
            {
                EnsureWritable();
                target.LateDusk = value;
            }
        }
    }
}
