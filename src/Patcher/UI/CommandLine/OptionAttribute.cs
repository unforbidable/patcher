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
using System.Text;

namespace Patcher.UI.CommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionAttribute : Attribute
    {
        readonly string longName;
        public string LongName { get { return longName; } }

        readonly char shortName;
        public char ShortName {  get { return shortName; } }

        public OptionAttribute(string longName, char shortName)
        {
            this.longName = longName;
            this.shortName = shortName;
        }

        public OptionAttribute(string longName)
            : this(longName, '\0')
        {
        }
    }
}
