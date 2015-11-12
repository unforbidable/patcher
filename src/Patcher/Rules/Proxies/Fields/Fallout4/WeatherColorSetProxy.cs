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
using Patcher.Rules.Compiled.Fields;

namespace Patcher.Rules.Proxies.Fields.Fallout4
{
    [Proxy(typeof(IWeatherColorSet))]
    public sealed class WeatherColorSetProxy : Proxy, IWeatherColorSet, IDumpabled
    {
        Wthr.ColorOctave target = null;

        internal WeatherColorSetProxy With(Wthr.ColorOctave target)
        {
            // Reset cached subproxies
            day = null;
            night = null;
            dawn = null;
            earlyDawn = null;
            lateDawn = null;
            dusk = null;
            earlyDusk = null;
            lateDusk = null;

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

        ColorProxy day = null;
        ColorProxy night = null;
        ColorProxy dawn = null;
        ColorProxy earlyDawn = null;
        ColorProxy lateDawn = null;
        ColorProxy dusk = null;
        ColorProxy earlyDusk = null;
        ColorProxy lateDusk = null;

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

        public IColor Dawn
        {
            get
            {
                if (dawn == null)
                    dawn = Provider.CreateProxy<ColorProxy>(Mode).With(target.Dawn);
                return dawn;
            }

            set
            {
                EnsureWritable();
                target.Dawn.Red = value.Red;
                target.Dawn.Green = value.Green;
                target.Dawn.Blue = value.Blue;
            }
        }

        public IColor EarlyDawn
        {
            get
            {
                if (earlyDawn == null)
                    earlyDawn = Provider.CreateProxy<ColorProxy>(Mode).With(target.EarlyDawn);
                return earlyDawn;
            }

            set
            {
                EnsureWritable();
                target.EarlyDawn.Red = value.Red;
                target.EarlyDawn.Green = value.Green;
                target.EarlyDawn.Blue = value.Blue;
            }
        }

        public IColor LateDawn
        {
            get
            {
                if (lateDawn == null)
                    lateDawn = Provider.CreateProxy<ColorProxy>(Mode).With(target.LateDawn);
                return lateDawn;
            }

            set
            {
                EnsureWritable();
                target.LateDawn.Red = value.Red;
                target.LateDawn.Green = value.Green;
                target.LateDawn.Blue = value.Blue;
            }
        }

         public IColor Dusk
        {
            get
            {
                if (dusk == null)
                    dusk = Provider.CreateProxy<ColorProxy>(Mode).With(target.Dusk);
                return dusk;
            }

            set
            {
                EnsureWritable();
                target.Dusk.Red = value.Red;
                target.Dusk.Green = value.Green;
                target.Dusk.Blue = value.Blue;
            }
        }

        public IColor EarlyDusk
        {
            get
            {
                if (earlyDusk == null)
                    earlyDusk = Provider.CreateProxy<ColorProxy>(Mode).With(target.EarlyDusk);
                return earlyDusk;
            }

            set
            {
                EnsureWritable();
                target.EarlyDusk.Red = value.Red;
                target.EarlyDusk.Green = value.Green;
                target.EarlyDusk.Blue = value.Blue;
            }
        }

        public IColor LateDusk
        {
            get
            {
                if (lateDusk == null)
                    lateDusk = Provider.CreateProxy<ColorProxy>(Mode).With(target.LateDusk);
                return lateDusk;
            }

            set
            {
                EnsureWritable();
                target.LateDusk.Red = value.Red;
                target.LateDusk.Green = value.Green;
                target.LateDusk.Blue = value.Blue;
            }
        }
    }
}
