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
using Patcher.Data.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Object model that represents a record field.
    /// </summary>
    public class FieldModel : IPresentable, IResolvableFrom<FieldModel>, IResolvableFrom<EnumModel>, INamed, IValidable
    {
        /// <summary>
        /// Field key, four characters long.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Name of the field.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Name of the field as it will be displayed in the GUI.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Description of the field.
        /// </summary>
        public string Description { get; private set; }

        private ICanRepresentField InnerModel { get; set; }

        /// <summary>
        /// Member represented by this field
        /// </summary>
        public MemberType MemberType { get { return InnerModel as MemberType; } }

        /// <summary>
        /// Group represeted by this field
        /// </summary>
        public FieldGroupModel FieldGroup { get { return InnerModel as FieldGroupModel; } }

        /// <summary>
        /// Struct represented by this field
        /// </summary>
        public StructModel Struct { get { return InnerModel as StructModel; } }

        /// <summary>
        /// Type of the property generated for this field, can be any crt type or generated structure or enumeration
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
        /// True, if field can repeat, in which case a List will be generated
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// True, if field represents an array
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Length of the array represented by this field
        /// </summary>
        public int ArrayLength { get; set; }

        public bool IsMember { get { return InnerModel is MemberType; } }
        public bool IsFieldGroup { get { return InnerModel is FieldGroupModel; } }
        public bool IsStruct { get { return InnerModel is StructModel; } }

        public FieldModel(string key, string name, string displayName, string description, ICanRepresentField innerModel, TargetModel targetModel, bool isHidden, bool isVirtual, bool isList, bool isArray, int arrayLength)
        {
            Key = key;
            Name = name;
            DisplayName = displayName ?? name;
            Description = description;
            InnerModel = innerModel;
            TargetModel = targetModel;
            IsHidden = isHidden;
            IsVirtual = isVirtual;
            IsList = isList;
            IsArray = isArray;
            ArrayLength = arrayLength;
        }

        public void ResolveFrom(FieldModel model)
        {
            Key = Key ?? model.Key;
            Name = Name ?? model.Name;
            DisplayName = DisplayName ?? model.DisplayName;
            Description = Description ?? model.Description;
            InnerModel = InnerModel ?? model.InnerModel;
            TargetModel = TargetModel ?? model.TargetModel;
            IsHidden = IsHidden || model.IsHidden;
            IsVirtual = IsVirtual || model.IsVirtual;
            IsList = model.IsList;
            IsArray = model.IsArray;
            ArrayLength = model.ArrayLength;
        }

        public void ResolveFrom(EnumModel model)
        {
            InnerModel = MemberType.GetKnownMemberType(model.BaseType);
            TargetModel = new TargetModel(model, IsArray, ArrayLength);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Key != null)
            {
                builder.AppendFormat("[Key({0})] ", Key);
            }

            if (IsList)
            {
                builder.Append("[List] ");
            }

            if (TargetModel != null)
            {
                builder.AppendFormat("[As({0})] ", TargetModel);
            }

            if (IsMember)
            {
                builder.AppendFormat("[Field] {0} ", MemberType);
            }
            else if (IsStruct)
            {
                builder.AppendFormat("[Struct] {0} ", Struct);
            }
            else if (IsFieldGroup)
            {
                builder.AppendFormat("[Group] {0} ", FieldGroup);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                builder.AppendFormat("[Description(\"{0}\")] ", Description);
            }

            if (!string.IsNullOrEmpty(DisplayName))
            {
                builder.AppendFormat("[DisplayName(\"{0}\")] ", DisplayName);
            }

            builder.AppendFormat("{0}", !string.IsNullOrEmpty(Name) ? Name : "<unspecified-name>");

            return builder.ToString();
        }

        public void ValidateModel(ModelValidator validator)
        {
            validator.AssertWithError(IsFieldGroup || !string.IsNullOrEmpty(Key), "Key is a required property of a field model that isn't a field group.");
        }
    }
}
