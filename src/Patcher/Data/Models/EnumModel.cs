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
using Patcher.Data.Models.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Patcher.Data.Models
{
    public class EnumModel : IPresentable, ITargetable
    {
        /// <summary>
        /// Name of the generated enumeration
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Name of the generated enumeration
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The underlying type of members
        /// </summary>
        public Type BaseType { get; private set; }

        /// <summary>
        /// True when this enumeration is used as flags
        /// </summary>
        public bool IsFlags { get; private set; }

        /// <summary>
        /// Map of member names and values.
        /// </summary>
        public EnumMemberModel[] Members { get; private set; }

        public EnumModel(string name, string description, Type baseType, bool isFlags, IEnumerable<EnumMemberModel> members)
        {
            Name = name;
            Description = description;
            BaseType = baseType;
            IsFlags = isFlags;
            Members = members.ToArray();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (IsFlags)
            {
                builder.Append("[Flags] ");
            }

            builder.Append(Name);
            builder.Append(" { ");
            builder.Append(string.Join(", ", Members.Select(m => m.ToString())));
            builder.Append(" }");

            return builder.ToString();
        }
    }
}
