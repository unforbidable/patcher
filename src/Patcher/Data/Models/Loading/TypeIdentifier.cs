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
using System.Text;
using System.Text.RegularExpressions;

namespace Patcher.Data.Models.Loading
{
    public class TypeIdentifier
    {
        public string Identifier { get; private set; }
        public int ArrayLength { get; private set; }
        public bool IsArray { get { return ArrayLength >= 0; } }
        public int ArrayPrefixSize { get; private set; }

        public static TypeIdentifier FromString(string id)
        {
            // See if identifier contains squeare brackets, indicating that it is array
            var match = Regex.Match(id, @"([^[]*)\[(b|w|l|[0-9]*)\]");
            if (match.Groups.Count > 1)
            {
                string lengthValue = match.Groups[2].Value;

                int length = 0;
                if (lengthValue.Length == 0 || int.TryParse(lengthValue, out length))
                {
                    // number or empty
                    return new TypeIdentifier()
                    {
                        Identifier = match.Groups[1].Value,
                        ArrayLength = lengthValue.Length == 0 ? 0 : int.Parse(lengthValue)
                    };
                }
                else
                {
                    // b, w or l
                    return new TypeIdentifier()
                    {
                        Identifier = match.Groups[1].Value,
                        ArrayPrefixSize = lengthValue == "b" ? 1 : lengthValue == "w" ? 2 : 4
                    };
                }
            }
            else
            {
                return new TypeIdentifier()
                {
                    Identifier = id,
                    ArrayLength = -1
                };
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Identifier, IsArray ? string.Format("[{0}]", ArrayLength > 0 ? ArrayLength.ToString() : string.Empty) : string.Empty);
        }
    }
}
