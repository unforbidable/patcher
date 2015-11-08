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
using Patcher.Rules.Compiled.Fields;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IWeatherColorQuad))]
    public sealed class WeatherColorQuadProxy : Proxy, IWeatherColorQuad, IDumpabled
    {
        Wthr.ColorQuad target = null;

        internal WeatherColorQuadProxy With(Wthr.ColorQuad target)
        {
            // Reset cached subproxies
            day = null;
            night = null;
            sunrise = null;
            sunset = null;

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

        ColorProxy day = null;
        ColorProxy night = null;
        ColorProxy sunrise = null;
        ColorProxy sunset = null;

        public IColor Day
        {
            get
            {
                if (day == null)
                    day = Provider.CreateProxy<ColorProxy>(Mode).With(target.Day);
                return day;
            }

            set
            {
                EnsureWritable();
                target.Day.Red = value.Red;
                target.Day.Green = value.Green;
                target.Day.Blue = value.Blue;
            }
        }

        public IColor Night
        {
            get
            {
                if (night == null)
                    night = Provider.CreateProxy<ColorProxy>(Mode).With(target.Night);
                return night;
            }

            set
            {
                EnsureWritable();
                target.Night.Red = value.Red;
                target.Night.Green = value.Green;
                target.Night.Blue = value.Blue;
            }
        }

        public IColor Sunrise
        {
            get
            {
                if (sunrise == null)
                    sunrise = Provider.CreateProxy<ColorProxy>(Mode).With(target.Sunrise);
                return sunrise;
            }

            set
            {
                EnsureWritable();
                target.Sunrise.Red = value.Red;
                target.Sunrise.Green = value.Green;
                target.Sunrise.Blue = value.Blue;
            }
        }

        public IColor Sunset
        {
            get
            {
                if (sunset == null)
                    sunset = Provider.CreateProxy<ColorProxy>(Mode).With(target.Sunset);
                return sunset;
            }

            set
            {
                EnsureWritable();
                target.Sunset.Red = value.Red;
                target.Sunset.Green = value.Green;
                target.Sunset.Blue = value.Blue;
            }
        }
    }
}
