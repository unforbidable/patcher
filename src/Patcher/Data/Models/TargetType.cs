/// Copyright(C) 2018 Unforbidable Works
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

using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    public class TargetType : ICanRepresentTarget
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        static Dictionary<string, TargetType> knownTypes = new Dictionary<string, TargetType>()
        {
            { "byte", new TargetType() { Name = "byte", Type = typeof(byte) } },
            { "sbyte", new TargetType() { Name = "sbyte", Type = typeof(sbyte) } },
            { "short", new TargetType() { Name = "short", Type = typeof(short) } },
            { "ushort", new TargetType() { Name = "ushort", Type = typeof(ushort) } },
            { "int", new TargetType() { Name = "int", Type = typeof(int) } },
            { "uint", new TargetType() { Name = "uint", Type = typeof(uint) } },
            { "float", new TargetType() { Name = "float", Type = typeof(float) } },
            { "string", new TargetType() { Name = "string", Type = typeof(string) } },
            { "fn", new TargetType() { Name = "Function", Type = typeof(object) } },
        };

        public static TargetType GetKnownTargetType(Type type)
        {
            return knownTypes.Where(p => p.Value.Type == type).Select(p => p.Value).Single();
        }

        public static bool TryFindKnownTargetType(string id, out TargetType type)
        {
            return knownTypes.TryGetValue(id, out type);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}", Name);
            return builder.ToString();
        }
    }
}
