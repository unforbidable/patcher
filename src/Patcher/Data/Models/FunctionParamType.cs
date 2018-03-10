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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    public class FunctionParamType : IModel, ICanRepresentFunctionParam
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        static Dictionary<string, FunctionParamType> knownTypes = new Dictionary<string, FunctionParamType>()
        {
            { "int", new FunctionParamType() { Name = "int", Type = typeof(int) } },
            { "float", new FunctionParamType() { Name = "float", Type = typeof(float) } },
            { "string", new FunctionParamType() { Name = "string", Type = typeof(string) } },
        };

        public static FunctionParamType GetKnownTargetType(Type type)
        {
            return knownTypes.Where(p => p.Value.Type == type).Select(p => p.Value).Single();
        }

        public static bool TryFindKnownTargetType(string id, out FunctionParamType type)
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
