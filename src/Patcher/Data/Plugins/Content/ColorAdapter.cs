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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Patcher.Data.Plugins.Content
{
    public class ColorAdapter
    {
        internal Func<float> GetRed { get; set; }
        internal Func<float> GetGreen { get; set; }
        internal Func<float> GetBlue { get; set; }
        internal Action<float> SetRed { get; set; }
        internal Action<float> SetGreen { get; set; }
        internal Action<float> SetBlue { get; set; }

        public ColorAdapter(IColorFloatAdaptable target)
        {
            GetRed = () => { return target.Red; };
            GetGreen = () => { return target.Green; };
            GetBlue = () => { return target.Blue; };
            SetRed = (value) => { target.Red = value; };
            SetGreen = (value) => { target.Green = value; };
            SetBlue = (value) => { target.Blue = value; };
        }

        public ColorAdapter(byte[] buffer)
            : this(buffer, 0)
        {
        }

        public ColorAdapter(byte[] buffer, int offset)
        {
            // byte offset + 0 is unused
            GetRed = () => { return buffer[offset + 0] / 255f; };
            GetGreen = () => { return buffer[offset + 1] / 255f; };
            GetBlue = () => { return buffer[offset + 2] / 255f; };
            SetRed = (value) => { buffer[offset + 0] = (byte)(value * 256); };
            SetGreen = (value) => { buffer[offset + 1] = (byte)(value * 256); };
            SetBlue = (value) => { buffer[offset + 2] = (byte)(value * 256); };
        }

        private float LimitRange(float value)
        {
            return Math.Min(Math.Max(value, 0.0f), 1.0f);
        }

        public float Red { get { return GetRed.Invoke(); } set { SetRed.Invoke(LimitRange(value)); } }
        public float Green { get { return GetGreen.Invoke(); } set { SetGreen.Invoke(LimitRange(value)); } }
        public float Blue { get { return GetBlue.Invoke(); } set { SetBlue.Invoke(LimitRange(value)); } }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", (byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255));
        }
    }
}
