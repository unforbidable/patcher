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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace Patcher.Data.Models.Loading
{
    public static class ModelLoadingHelper
    {
        private static Dictionary<string, Type> enumTypes = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) }
        };

        public static string GetEnumTypeName(Type type)
        {
            return enumTypes.Where(t => t.Value == type).Select(t => t.Key).FirstOrDefault();
        }

        public static Type ParseEnumType(string value)
        {
            if (enumTypes.ContainsKey(value))
            {
                return enumTypes[value];
            }
            else
            {
                throw new FormatException(string.Format("Unknown or forbidden enum type '{0}'", value));
            }
        }

        public static bool TryParseEnumType(string value, out Type type)
        {
            if (enumTypes.ContainsKey(value))
            {
                type = enumTypes[value];
                return true;
            }
            else
            {
                type = null;
                return false;
            }
        }

        public static string ReparseValue(string value, Type type, ReparseOptions options)
        {
            bool valueHasHexadecimalPrefix = value.ToLower().StartsWith("0x");
            string valueWithoutHexadecimalPrefix = valueHasHexadecimalPrefix ? value.Substring(2) : value;

            if (type == typeof(byte))
            {
                byte parsed;
                if (valueHasHexadecimalPrefix && byte.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || byte.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else if (type == typeof(sbyte))
            {
                sbyte parsed;
                if (valueHasHexadecimalPrefix && sbyte.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || sbyte.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else if (type == typeof(short))
            {
                short parsed;
                if (valueHasHexadecimalPrefix && short.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || short.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else if (type == typeof(ushort))
            {
                ushort parsed;
                if (valueHasHexadecimalPrefix && ushort.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || ushort.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else if (type == typeof(int))
            {
                int parsed;
                if (valueHasHexadecimalPrefix && int.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || int.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else if (type == typeof(uint) || type == null)
            {
                uint parsed;
                if (valueHasHexadecimalPrefix && uint.TryParse(valueWithoutHexadecimalPrefix, System.Globalization.NumberStyles.HexNumber, null, out parsed) || uint.TryParse(value, out parsed))
                {
                    return ParsedValueToString(parsed, options);
                }
                else
                {
                    throw new FormatException(string.Format("Value '{0}' is not valid for type {1}", value, type.Name));
                }
            }
            else
            {
                throw new NotImplementedException(string.Format("Value '{0}' cannot be reparsed becuase type '{1}' is not supported", value, type.Name));
            }
        }

        private static string ParsedValueToString(object value, ReparseOptions options)
        {
            if (options.HasFlag(ReparseOptions.Hexadecimal) && options.HasFlag(ReparseOptions.LeadingZeros))
            {
                return string.Format("0x{0:x" + Marshal.SizeOf(value) * 2 + "}", value);
            }
            else if (options.HasFlag(ReparseOptions.Hexadecimal))
            {
                return string.Format("0x{0:x}", value);
            }
            else
            {
                return value.ToString();
            }
        }

        public static string IndentLines(string text)
        {
            string spaces = new string(' ', 4);
            return spaces + string.Join("\n" + spaces, text.Split('\n'));
        }
    }
}
