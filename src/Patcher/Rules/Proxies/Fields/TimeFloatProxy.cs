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
    [Proxy(typeof(ITimeFloat))]
    public class TimeFloatProxy : Proxy, ITimeFloat, IDumpabled
    {
        private Imad.TimeFloat timeFloat;

        internal TimeFloatProxy With(Imad.TimeFloat timeFloat)
        {
            this.timeFloat = timeFloat;
            return this;
        }

        public float Value
        {
            get
            {
                return timeFloat.Value;
            }

            set
            {
                EnsureWritable();
                timeFloat.Value = value;
            }
        }

        public float Time
        {
            get
            {
                return timeFloat.Time;
            }

            set
            {
                EnsureWritable();
                timeFloat.Time = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Time={0} Value={1}", Time, Value);
        }

        void IDumpabled.Dump(string name, ObjectDumper dumper)
        {
            dumper.DumpText(name, ToString());
        }
    }
}
