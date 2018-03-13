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
    /// Model object that represents a form record.
    /// </summary>
    public class RecordModel : IPresentable
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }

        public FieldModel[] Fields { get; private set; }

        public RecordModel(string key, string displayName, string name, string description, IEnumerable<FieldModel> fields)
        {
            Key = key;
            Name = name;
            DisplayName = displayName;
            Description = description;
            Fields = fields.ToArray();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("[Key({0})] ", Key);

            builder.Append(!string.IsNullOrEmpty(Name) ? Name : "<unspecified-name>");
            builder.Append(" \n{ \n");
            builder.Append(ModelLoadingHelper.IndentLines(string.Join(", \n", Fields.Select(f => f.ToString()))));
            builder.Append(" \n}");

            return builder.ToString();
        }
    }
}
