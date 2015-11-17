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
using Patcher.Rules.Compiled.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields
{
    [Proxy(typeof(ITimeColor))]
    public class TimeColorProxy : Proxy, ITimeColor, IDumpabled
    {
        private Imad.TimeColor timeColor;

        internal TimeColorProxy With(Imad.TimeColor timeColor)
        {
            this.timeColor = timeColor;
            return this;
        }

        public IColor Color
        {
            get
            {
                return Provider.CreateProxy<ColorProxy>(Mode).With(timeColor.Color);
            }

            set
            {
                EnsureWritable();
                timeColor.Color.Red = value.Red;
                timeColor.Color.Green = value.Green;
                timeColor.Color.Blue = value.Blue;
            }
        }

        public float Time
        {
            get
            {
                return timeColor.Time;
            }

            set
            {
                EnsureWritable();
                timeColor.Time = value;
            }
        }


        public override string ToString()
        {
            return string.Format("Time={0} Color={1}", Time, Color);
        }

        void IDumpabled.Dump(string name, ObjectDumper dumper)
        {
            dumper.DumpText(name, ToString());
        }
    }
}
