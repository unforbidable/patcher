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
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IWeatherFresnelQuad))]
    public sealed class WeatherFresnelQuadProxy : Proxy, IWeatherFresnelQuad, IDumpabled
    {
        Wthr.FloatQuad target = null;

        internal WeatherFresnelQuadProxy With(Wthr.FloatQuad target)
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
            return string.Format("Sunrise={0} Day={1} Sunset={2} Night={3}", Sunrise, Day, Sunset, Night);
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

        public float Sunrise
        {
            get
            {
                return target.Sunrise;
            }

            set
            {
                EnsureWritable();
                target.Sunrise = value;
            }
        }

        public float Sunset
        {
            get
            {
                return target.Sunset;
            }

            set
            {
                EnsureWritable();
                target.Sunset = value;
            }
        }
    }
}
