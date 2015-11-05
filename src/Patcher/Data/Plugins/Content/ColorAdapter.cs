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

        private float LimitRange(float value)
        {
            return Math.Min(Math.Max(value, 0.0f), 1.0f);
        }

        public float Red { get { return GetRed.Invoke(); } set { SetRed.Invoke(LimitRange(value)); } }
        public float Green { get { return GetGreen.Invoke(); } set { SetGreen.Invoke(LimitRange(value)); } }
        public float Blue { get { return GetBlue.Invoke(); } set { SetBlue.Invoke(LimitRange(value)); } }
    }
}
