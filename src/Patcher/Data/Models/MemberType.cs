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
    public class MemberType : ICanRepresentField
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public bool IsZeroTerminatedString { get; private set; }
        public int StringPrefixSize { get; private set; }
        public StringLocalization StringLocalization { get; private set; }

        static Dictionary<string, MemberType> knownTypes = new Dictionary<string, MemberType>()
        {
            { "bool", new MemberType() { Name = "boolean", Type = typeof(bool) } },
            { "byte", new MemberType() { Name = "byte", Type = typeof(byte) } },
            { "sbyte", new MemberType() { Name = "sbyte", Type = typeof(sbyte) } },
            { "short", new MemberType() { Name = "short", Type = typeof(short) } },
            { "ushort", new MemberType() { Name = "ushort", Type = typeof(ushort) } },
            { "int", new MemberType() { Name = "int", Type = typeof(int) } },
            { "uint", new MemberType() { Name = "uint", Type = typeof(uint) } },
            { "long", new MemberType() { Name = "long", Type = typeof(long) } },
            { "ulong", new MemberType() { Name = "ulong", Type = typeof(ulong) } },
            { "float", new MemberType() { Name = "float", Type = typeof(float) } },
            { "bstring", new MemberType() { Name = "string", Type = typeof(string), StringPrefixSize = 1 } },
            { "bzstring", new MemberType() { Name = "string", Type = typeof(string), StringPrefixSize = 1, IsZeroTerminatedString = true } },
            { "dlstring", new MemberType() { Name = "string", Type = typeof(string), IsZeroTerminatedString = true, StringLocalization = StringLocalization.DLStrings } },
            { "ilstring", new MemberType() { Name = "string", Type = typeof(string), IsZeroTerminatedString = true, StringLocalization = StringLocalization.ILStrings } },
            { "lstring", new MemberType() { Name = "string", Type = typeof(string), IsZeroTerminatedString = true, StringLocalization = StringLocalization.LStrings } },
            { "string", new MemberType() { Name = "string", Type = typeof(string) } },
            { "wstring", new MemberType() { Name = "string", Type = typeof(string), StringPrefixSize = 2 } },
            { "wzstring", new MemberType() { Name = "string", Type = typeof(string), IsZeroTerminatedString = true, StringPrefixSize = 2 } },
            { "zstring", new MemberType() { Name = "string", Type = typeof(string), IsZeroTerminatedString = true } },
        };

        public static MemberType GetKnownMemberType(Type type)
        {
            return knownTypes.Where(p => p.Value.Type == type).Select(p => p.Value).Single();
        }

        public static bool TryFindKnownMemberType(string id, out MemberType type)
        {
            return knownTypes.TryGetValue(id, out type);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (IsZeroTerminatedString)
            {
                builder.Append("[ZeroTerminated] ");
            }
            if (StringPrefixSize > 0)
            {
                builder.AppendFormat("[PrefixSize({0})] ", StringPrefixSize);
            }
            if (StringLocalization !=  StringLocalization.None)
            {
                builder.AppendFormat("[Localized(\"{0}\")] ", StringLocalization.ToString().ToLower());
            }
            builder.AppendFormat("{0}", Name);
            return builder.ToString();
        }
    }
}
