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
    /// <summary>
    /// Model object that represents the type of a formal function parameter.
    /// </summary>
    public class FunctionParamModel : IModel, IResolvable, IResolvableFrom<EnumModel>
    {
        /// <summary>
        /// Gets the model representing the type of this function parameter.
        /// </summary>
        public ICanRepresentFunctionParam Type { get; private set; }

        /// <summary>
        /// The name of the parameter, as displayed by the GUI.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the primitive type representing this parameter, or null.
        /// </summary>
        public FunctionParamType FunctionParamType { get { return Type as FunctionParamType; } }

        /// <summary>
        /// Gets the enumeration representing this parameter, or null. 
        /// </summary>
        public EnumModel Enumeration { get { return Type as EnumModel; } }

        /// <summary>
        /// Gets the form reference represeting this parameter, or null.
        /// </summary>
        public FormReference FormReference { get { return Type as FormReference; } }

        public bool IsFunctionParamType { get { return Type is FunctionParamType; } }
        public bool IsEnumeration { get { return Type is EnumModel; } }
        public bool IsFormReference { get { return Type is FormReference; } }

        public FunctionParamModel(ICanRepresentFunctionParam model, string displayName)
        {
            Type = model;
            DisplayName = displayName;
        }

        public void ResolveFrom(EnumModel model)
        {
            Type = model;
        }

        public override string ToString()
        {
            return Type != null ? Type.Name : "<undefined-type>";
        }
    }
}
