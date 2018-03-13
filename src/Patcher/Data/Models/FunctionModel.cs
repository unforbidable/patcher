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
using Patcher.Data.Models.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Model object that represents a function protorype.
    /// </summary>
    public class FunctionModel : IModel, IPresentable, INamed
    {
        /// <summary>
        /// Index of the function.
        /// </summary>
        /// 
        public short Index { get; private set; }

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the function.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Formal parameters of the function.
        /// </summary>
        public FunctionParamModel[] Params { get; private set; }

        public FunctionModel(short index, string name, string description, IEnumerable<FunctionParamModel> parameters)
        {
            Index = index;
            Name = name;
            Description = description;
            Params = parameters.ToArray();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Name);
            builder.Append("(");
            builder.Append(string.Join(", ", Params.Select(p => p.ToString())));
            builder.Append(")");

            return builder.ToString();
        }
    }
}
