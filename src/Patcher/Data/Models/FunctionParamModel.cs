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
        ICanRepresentFunctionParam ParamInternal { get; set; }

        /// <summary>
        /// Gets the primitive type representing this parameter.
        /// </summary>
        public FunctionParamType FunctionParamType { get { return ParamInternal as FunctionParamType; } }

        /// <summary>
        /// Gets the enumeration representing this parameter. 
        /// </summary>
        public EnumModel Enumeration { get { return ParamInternal as EnumModel; } }

        /// <summary>
        /// Gets the form reference represeting this parameter.
        /// </summary>
        public FormReference FormReference { get { return ParamInternal as FormReference; } }

        public FunctionParamModel(ICanRepresentFunctionParam model)
        {
            ParamInternal = model;
        }

        public void ResolveFrom(EnumModel model)
        {
            ParamInternal = model;
        }

        public override string ToString()
        {
            return ParamInternal != null ? ParamInternal.Name : "<undefined-type>";
        }
    }
}
