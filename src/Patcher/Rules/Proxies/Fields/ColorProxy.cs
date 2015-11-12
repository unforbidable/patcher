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

using Patcher.Data.Plugins.Content;
using Patcher.Rules.Compiled.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields
{
    [Proxy(typeof(IColor))]
    public class ColorProxy : Proxy, IColor, IDumpabled
    {
        private ColorAdapter adapter;

        internal ColorProxy With(ColorAdapter adapter)
        {
            this.adapter = adapter;
            return this;
        }

        public float Blue
        {
            get
            {
                return adapter.Blue;
            }
            set
            {
                EnsureWritable();
                adapter.Blue = value;
            }
        }

        public float Green
        {
            get
            {
                return adapter.Green;
            }
            set
            {
                EnsureWritable();
                adapter.Green = value;
            }
        }

        public float Red
        {
            get
            {
                return adapter.Red;
            }
            set
            {
                EnsureWritable();
                adapter.Red = value;
            }
        }

        public void SetValues(float value)
        {
            adapter.Red = value;
            adapter.Green = value;
            adapter.Blue = value;
        }

        public void MultiplyValues(float value)
        {
            adapter.Red *= value;
            adapter.Green *= value;
            adapter.Blue *= value;
        }

        public void SetValues(float red, float green, float blue)
        {
            adapter.Red = red;
            adapter.Green = green;
            adapter.Blue = blue;
        }

        public void MultiplyValues(float red, float green, float blue)
        {
            adapter.Red *= red;
            adapter.Green *= green;
            adapter.Blue *= blue;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", (byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255));
        }

        void IDumpabled.Dump(string name, ObjectDumper dumper)
        {
            dumper.DumpText(name, ToString());
        }
    }
}
