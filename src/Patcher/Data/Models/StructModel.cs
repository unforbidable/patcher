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
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    public class StructModel : IPresentable, ITargetable, ICanRepresentField
    {
        /// <summary>
        /// Name of the generated class that represents this structure
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the generated class that represents this structure
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Members of this structure
        /// </summary>
        public MemberModel[] Members { get; private set; }

        public StructModel(string name, string description, IEnumerable<MemberModel> members)
        {
            Name = name;
            Description = description;
            Members = members.ToArray();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(!string.IsNullOrEmpty(Name) ? Name : "<anonymous>");
            builder.Append(" \n{ \n");
            builder.Append(ModelLoadingHelper.IndentLines(string.Join(", \n", Members.Select(m => m.ToString()))));
            builder.Append(" \n}");

            return builder.ToString();
        }
    }
}
