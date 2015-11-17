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
    public class ColorAlphaAdapter : ColorAdapter
    {
        internal Func<float> GetAlpha { get; set; }
        internal Action<float> SetAlpha { get; set; }

        public ColorAlphaAdapter(IColorAlphaFloatAdaptable target)
            : base(target)
        {
            GetAlpha = () => { return target.Alpha; };
            SetAlpha = (value) => { target.Alpha = value; };
        }

        public float Alpha { get { return GetAlpha.Invoke(); } set { SetAlpha.Invoke(LimitRange(value)); } }

        public override string ToString()
        {
            return string.Format("({0},{1},{2},{3})", (byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255), (byte)(Alpha * 255));
        }
    }
}
