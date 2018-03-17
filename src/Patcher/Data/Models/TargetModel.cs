﻿/// Copyright(C) 2018 Unforbidable Works
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Model object that represents the type of property as which a field is presented.
    /// </summary>
    public class TargetModel : IModel, IResolvableFrom<EnumModel>, IResolvableFrom<StructModel>
    {
        /// <summary>
        /// Gets the model that represents this target.
        /// </summary>
        public ICanRepresentTarget Type { get; private set; }

        /// <summary>
        /// When true, target is an array
        /// </summary>
        public bool IsArray { get; private set; }

        /// <summary>
        /// Length of the array, only used when target is an array
        /// </summary>
        public int ArrayLength { get; private set; }

        public TargetModel(ICanRepresentTarget type, bool isArray, int arrayLength)
        {
            Type = type;
            IsArray = isArray;
            ArrayLength = arrayLength;
        }

        public void ResolveFrom(EnumModel model)
        {
            Type = model;
        }

        public void ResolveFrom(StructModel model)
        {
            Type = model;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Type != null)
            {
                builder.AppendFormat("{0}", Type.Name);
                if (IsArray)
                {
                    builder.AppendFormat("[{0}]", ArrayLength > 0 ? ArrayLength.ToString() : string.Empty);
                }
            }

            return builder.ToString();
        }

    }
}
