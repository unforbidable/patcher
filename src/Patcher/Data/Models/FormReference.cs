// Copyright(C) 2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Data.Models.Loading;
using Patcher.Data.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Represents a reference to a form of any, one or more specific types
    /// </summary>
    public class FormReference : IModel, ICanRepresentTarget, ICanRepresentFunctionParam, ISerializableAsId
    {
        public string Id { get { return ToString(); } }
        public string Name { get { return ToString(); } }
        public string[] FormTypes { get; private set; }
        public bool IsAny { get { return FormTypes.Length == 0; } }

        static FormReference AnyFormReference = new FormReference(Enumerable.Empty<string>());

        public FormReference(IEnumerable<string> formTypes)
        {
            FormTypes = formTypes.ToArray();
        }

        public static bool TryParse(string id, out FormReference formReference)
        {
            if (id == "ref")
            {
                formReference = AnyFormReference;
                return true;
            }
            else
            {
                // See if identifier ref is followed by brackets
                var match = Regex.Match(id, @"ref\(([^)]*)\)");
                if (match != null && match.Groups.Count > 1)
                {
                    string types = match.Groups[1].Value;
                    formReference = new FormReference(types.Split('|').Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()));
                    return true;
                }
                else
                {
                    formReference = null;
                    return false;
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("ref");
            if (!IsAny)
            {
                builder.AppendFormat("({0})", string.Join("|", FormTypes));
            }
            return builder.ToString();
        }
    }
}
