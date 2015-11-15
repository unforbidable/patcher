/// Copyright(C) 2015 Unforbidable Works
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Patcher.Data.Plugins.Content
{
    sealed class MemberInfo
    {
        readonly Type fieldType;
        public Type FieldType { get { return fieldType; } }

        readonly private PropertyInfo property;

        readonly HashSet<string> fieldNames;
        public HashSet<string> FieldNames {  get { return fieldNames; } }

        readonly LocalizedStringGroups localizedStringGroup = LocalizedStringGroups.None;
        public LocalizedStringGroups LocalizedStringGroup { get { return localizedStringGroup; } }

        public int Order { get; set; }
        public bool IsLazy { get; private set; }
        public bool IsFakeFloat { get; private set; }
        public bool Initialize { get; private set; }
        public bool IsPrimitiveType { get; private set; }
        public bool IsListType { get; private set; }
        public bool IsNullableType { get; private set; }
        public bool IsReference { get; private set; }
        public bool IsRequired { get; private set; }
        public bool IsDynamic { get; set; }
        public FormKindSet ReferencedFormKinds { get; private set; }

        public Regex DynamicArrayRegex { get; set; }

        readonly MethodInfo addValueToListMethod;
        readonly ConstructorInfo createListCtor;

        static Type[] paramlessTypes = new Type[] { };
        static object[] paramlessArgs = new object[] { };

        public MemberInfo(MemberAttribute attribute, PropertyInfo property)
        {
            this.property = property;

            fieldNames = attribute.FieldNames;

            IsLazy = property.GetCustomAttributes(typeof(LazyAttribute), false).Length > 0;
            IsFakeFloat = property.GetCustomAttributes(typeof(FakeFloatAttribute), false).Length > 0;
            Initialize = property.GetCustomAttributes(typeof(InitializeAttribute), false).Length > 0;
            IsRequired = property.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            var orderAttribute = (OrderAttribute)property.GetCustomAttributes(typeof(OrderAttribute), false).FirstOrDefault();
            if (orderAttribute != null)
            {
                Order = orderAttribute.Order;
            }

            var localizedStringAttribute = (LocalizedStringAttribute)property.GetCustomAttributes(typeof(LocalizedStringAttribute), false).FirstOrDefault();
            if (localizedStringAttribute != null)
            {
                localizedStringGroup = localizedStringAttribute.LocalizedStringGroup;
            }

            var referenceAttribute = (ReferenceAttribute)property.GetCustomAttributes(typeof(ReferenceAttribute), false).FirstOrDefault();
            if (referenceAttribute != null)
            {
                IsReference = true;
                ReferencedFormKinds = referenceAttribute.ReferenceFormKinds;
            }

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                fieldType = property.PropertyType.GetGenericArguments()[0];
                IsListType = true;

                addValueToListMethod = typeof(ICollection<>).MakeGenericType(property.PropertyType.GetGenericArguments()).GetMethod("Add");
                createListCtor = typeof(List<>).MakeGenericType(property.PropertyType.GetGenericArguments()).GetConstructor(paramlessTypes);
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                fieldType = property.PropertyType.GetGenericArguments()[0];
                IsNullableType = true;
            }
            else
            {
                fieldType = property.PropertyType;
                IsListType = false;
            }

            IsPrimitiveType = !typeof(Field).IsAssignableFrom(fieldType);

            // Field is dynamic if any of its field names contains a .
            IsDynamic = fieldNames.Where(n => n.Contains('.')).Any();

            if (IsDynamic)
            {
                // Replace . with (.|\n) to cover also new line
                var pattern = string.Join("|", fieldNames).Replace(".", "(.|\n)");
                DynamicArrayRegex = new Regex(pattern);
            }

            //Enums treat enums as if the where the underlaying primitive type.
            if (fieldType.IsEnum)
            {
                fieldType = fieldType.GetEnumUnderlyingType();
            }
        }

        public IEnumerable<uint> GetReferencedFormIds(object target)
        {
            if (IsReference)
            {
                if (IsListType)
                {
                    foreach (object item in (IEnumerable)GetValue(target))
                    {
                        yield return (uint)item;
                    }
                }
                else
                {
                    yield return (uint)GetValue(target);
                }
            }
            else if (!IsPrimitiveType)
            {
                if (IsListType)
                {
                    // No references if list is null
                    var value = GetValue(target);
                    if (value == null)
                        yield break;

                    foreach (object item in (IEnumerable)value)
                    {
                        var field = (Field)item;
                        if (field != null)
                        {
                            foreach (var formId in field.GetReferencedFormIds())
                            {
                                yield return formId;
                            }
                        }
                    }
                }
                else
                {
                    var field = (Field)GetValue(target);
                    if (field != null)
                    {
                        foreach (var formId in field.GetReferencedFormIds())
                        {
                            yield return formId;
                        }
                    }
                }
            }
        }

        public void SetOrAddValue(object target, object value)
        {
            if (IsListType)
            {
                EnsureListCreated(target);
                AddValue(target, value);
            }
            else
            {
                SetValue(target, value);
            }
        }

        public void SetValue(object target, object value)
        {
            if (IsListType)
                throw new InvalidOperationException("Cannot set value to field that is a list");

            property.SetValue(target, value, null);
        }

        public void SetList(object target, object value)
        {
            if (!IsListType)
                throw new InvalidOperationException("Cannot set list to field that is not a list");

            property.SetValue(target, value, null);
        }

        public void AddValue(object target, object value)
        {
            if (!IsListType)
                throw new InvalidOperationException("Cannot add values to field that is not a list");

            object list = property.GetValue(target, null);
            addValueToListMethod.Invoke(list, new object[] { value });
        }

        public void EnsureListCreated(object target)
        {
            if (!IsListType)
                throw new InvalidOperationException("Cannot create list for field that is not a list");

            object list = property.GetValue(target, null);
            if (list == null)
            {
                list = createListCtor.Invoke(paramlessArgs);
                property.SetValue(target, list, null);
            }
        }

        public object GetValue(object target)
        {
            return property.GetValue(target, null);
        }

        public object CopyValue(object from)
        {
            if (IsListType)
            {
                // If list is null, nothing to copy
                var value = GetValue(from);
                if (value == null)
                    return null;

                // create new list instance and copy values one by one
                object list = createListCtor.Invoke(paramlessArgs);
                var addValueToListParams = new object[1];
                foreach (object item in (IEnumerable)value)
                {
                    if (IsPrimitiveType)
                    {
                        addValueToListParams[0] = item;
                    }
                    else
                    {
                        addValueToListParams[0] = ((Field)item).CopyField();
                    }
                    addValueToListMethod.Invoke(list, addValueToListParams);
                }
                return list;
            }
            else if (IsPrimitiveType)
            {
                // Simpy copy value of primitive types by assignement
                return GetValue(from);
            }
            else
            {
                // Let complex properies copy themselves, unless they are null
                if (GetValue(from) == null)
                    return null;
                else
                    return ((Field)GetValue(from)).CopyField();
            }
        }

        public bool Equate(object one, object other)
        {
            if (IsListType)
            {
                var listOne = (IList)GetValue(one);
                var listOther = (IList)GetValue(other);

                // Both null means equal
                if (listOne == null && listOther == null)
                    return true;

                // Only one of them null means not equal
                if (listOne == null || listOther == null)
                    return false;

                // Different lengths means different lists
                if (listOne.Count != listOther.Count)
                    return false;

                var itOne = listOne.GetEnumerator();
                var itOther = listOther.GetEnumerator();

                while (itOne.MoveNext() && itOther.MoveNext())
                {
                    if (IsPrimitiveType)
                    {
                        object oneValue = itOne.Current;
                        object otherValue = itOther.Current;
                        if (!oneValue.Equals(otherValue))
                            return false;
                    }
                    else
                    {
                        var fieldOne = (Field)itOne.Current;
                        var fieldOther = (Field)itOther.Current;
                        if (!fieldOne.Equals(fieldOther))
                            return false;
                    }
                }
            }
            else
            {
                if (IsPrimitiveType)
                {
                    object oneValue = GetValue(one);
                    object otherValue = GetValue(other);

                    // Both values are null means equal
                    if (oneValue == null && otherValue == null)
                        return true;

                    // One of the values is null means not equal
                    if (oneValue == null || otherValue == null)
                        return false;

                    // Both not null, consider each value
                    if (!oneValue.Equals(otherValue))
                        return false;
                }
                else
                {
                    var oneField = (Field)GetValue(one);
                    var otherField = (Field)GetValue(other);

                    // Both fields do not exists means equal
                    if (oneField == null && otherField == null)
                        return true;

                    // One of the fiels does not exist means not equal
                    if (oneField == null || otherField == null)
                        return false;

                    // Both fields exist, consider the content
                    if (!oneField.Equals(otherField))
                        return false;
                }
            }

            return true;
        }
    }
}
