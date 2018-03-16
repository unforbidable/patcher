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
    /// Model object that represents a member of a structure.
    /// </summary>
    public class MemberModel : IPresentable, IResolvableFrom<EnumModel>, IResolvableFrom<FieldModel>, IResolvableFrom<StructModel>, INamed
    {
        /// <summary>
        /// Name of the member, used as the name for the generated field, property etc.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Name of the member, as will be displayed by the GUI
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Description of the member, used as the name for the generated field, property etc.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Type of the member, describing the way the field is stored in plugins.
        /// </summary>
        public MemberType MemberType { get; private set; }

        /// <summary>
        /// Struct model that represents this memner.
        /// </summary>
        public StructModel Struct { get; private set; }

        /// <summary>
        /// Type of the property generated for this field or member, can be any crt type or generated structure or enumeration
        /// </summary>
        public TargetModel TargetModel { get; private set; }

        /// <summary>
        /// When true, the property is not created and field is not directly accessibly
        /// </summary>
        public bool IsHidden { get; private set; }

        /// <summary>
        /// When true, only the property is created (there is no underlying data) and the property is able access other fields, even private
        /// </summary>
        public bool IsVirtual { get; private set; }

        /// <summary>
        /// When true, member is an array
        /// </summary>
        public bool IsArray { get; private set; }

        /// <summary>
        /// Length of the array, only used when member is an array
        /// </summary>
        public int ArrayLength { get; private set; }

        /// <summary>
        /// Size of the array length prefix, only used when member is an array
        /// </summary>
        public int ArrayPrefixSize { get; private set; }

        public bool IsStruct { get { return Struct != null; } }

        public MemberModel(string name, string displayName, string description, MemberType memberType, TargetModel targetModel, bool isHidden, bool isVirtual, bool isArray, int arrayLength, int arrayPrefixSize)
        {
            Name = name;
            DisplayName = displayName ?? name;
            Description = description;
            MemberType = memberType;
            TargetModel = targetModel;
            IsHidden = isHidden;
            IsVirtual = isVirtual;
            IsArray = isArray;
            ArrayLength = arrayLength;
            ArrayPrefixSize = arrayPrefixSize;
        }

        public void ResolveFrom(EnumModel model)
        {
            MemberType = MemberType.GetKnownMemberType(model.BaseType);
            TargetModel = new TargetModel(model, IsArray, ArrayLength);
        }

        public void ResolveFrom(FieldModel model)
        {
            Name = Name ?? model.Name;
            DisplayName = DisplayName ?? model.DisplayName;
            Description = Description ?? model.Description;
            MemberType = MemberType ?? model.MemberType;
            TargetModel = TargetModel ?? model.TargetModel;
            IsHidden = IsHidden || model.IsHidden;
            IsVirtual = IsVirtual || model.IsVirtual;
            IsArray = model.IsArray;
            ArrayLength = model.ArrayLength;
        }

        public void ResolveFrom(StructModel model)
        {
            Struct = model;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (IsVirtual)
            {
                builder.Append("[Virtual] ");
            }

            if (IsHidden)
            {
                builder.Append("[Hidden] ");
            }

            if (TargetModel != null)
            {
                builder.AppendFormat("[As({0})] ", TargetModel);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                builder.AppendFormat("[Description(\"{0}\")] ", Description);
            }

            if (!string.IsNullOrEmpty(DisplayName))
            {
                builder.AppendFormat("[DisplayName(\"{0}\")] ", DisplayName);
            }

            if (ArrayPrefixSize > 0)
            {
                builder.AppendFormat("[ArrayPrefix({0})] ", ArrayPrefixSize);
            }


            builder.AppendFormat("{0}", MemberType != null ? MemberType.ToString() : Struct != null ? Struct.Name : "<unspecified-type>");

            if (IsArray)
            {
                builder.AppendFormat("[{0}]", ArrayLength > 0 ? ArrayLength.ToString() : string.Empty);
            }

            builder.AppendFormat(" {0}", !string.IsNullOrEmpty(Name) ? Name : "<unspecified-name>");

            return builder.ToString();
        }
    }
}
