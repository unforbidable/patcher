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
    /// <summary>
    /// Model object that represent a group of fields that are grouped together.
    /// </summary>
    public class FieldGroupModel : IPresentable, ICanRepresentField, INamed
    {
        /// <summary>
        /// Name of the generated class that represents this field group
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the generated class that represents this field group
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// List of fields.
        /// </summary>
        public FieldModel[] Fields { get; private set; }

        public FieldGroupModel(string name, string description, IEnumerable<FieldModel> fields)
        {
            Name = name;
            Description = description;
            Fields = fields.ToArray();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(!string.IsNullOrEmpty(Name) ? Name : "<anonymous>");
            builder.Append(" \n{ \n");
            builder.Append(ModelLoadingHelper.IndentLines(string.Join(", \n", Fields.Select(f => f.ToString()))));
            builder.Append(" \n}");

            return builder.ToString();
        }
    }
}
