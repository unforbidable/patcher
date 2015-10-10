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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins
{
    /// <summary>
    /// Represents a 4-letter long string that is used to identify form kinds.
    /// </summary>
    public struct FormKind : IComparable<FormKind>, IEquatable<FormKind>
    {
        public static readonly FormKind None = new FormKind() { value = 0 };

        static IList<string> index = new List<string>() { string.Empty };
        static IDictionary<string, ushort> map = new SortedDictionary<string, ushort>() { { string.Empty, 0 } };

        public static FormKind FromString(string name)
        {
            lock (map)
            {
                if (map.ContainsKey(name))
                {
                    return new FormKind() { value = map[name] };
                }
                else
                {
                    if (index.Count > ushort.MaxValue)
                    {
                        throw new InvalidOperationException("Reached form kind limit");
                    }

                    ushort next = (ushort)index.Count;
                    index.Add(name);
                    map.Add(name, next);
                    return new FormKind() { value = next };
                }
            }
        }

        // Should work as byte as well as long as there is less than 255 kinds of form
        ushort value;

        // Conversion to string is explixit because it's fast and there is no information lost
        public static implicit operator string(FormKind kind)
        {
            return index[kind.value];
        }

        // Conversion from string is explixit to prevent creation of instances any time when compared with a string
        public static explicit operator FormKind(string name)
        {
            return FromString(name);
        }

        public override bool Equals(object obj)
        {
            return obj is FormKind && this == (FormKind)obj;
        }

        public bool Equals(FormKind other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(FormKind x, FormKind y)
        {
            return x.value == y.value;
        }

        public static bool operator !=(FormKind x, FormKind y)
        {
            return !(x == y);
        }

        public int CompareTo(FormKind other)
        {
            return value.CompareTo(other.value);
        }

        public override string ToString()
        {
            return this;
        }
    }
}
