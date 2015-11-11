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

using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Documenter
{
    static class Utility
    {
        public static string GetLocalNamespace(this Type type)
        {
            if (type.Namespace.StartsWith(Program.RootNamespace))
            {
                return type.Namespace.Substring(Program.RootNamespace.Length).TrimStart('.');
            }
            else
            {
                return type.Namespace;
            }
        }

        public static string GetLocalName(this Type type)
        {
            if (typeof(Delegate).IsAssignableFrom(type))
            {
                return "delegate";
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return "IEnumerable";
            }
            else if (type.FullName.Contains(".Helpers."))
            {
                return type.Name.TrimStart('I').Replace("Helper", string.Empty);
            }
            else if (type.Namespace.Contains(".Forms") || type.Namespace.Contains(".Fields"))
            {
                return type.Name.Substring(1).Replace("`1", string.Empty);
            }
            else if (type.FullName.StartsWith(Program.RootNamespace))
            {
                return type.Name;
            }
            else
            {
                switch (type.Name)
                {
                    case "Boolean":
                        return "bool";
                    case "Int32":
                        return "int";
                    case "UInt32":
                        return "uint";
                    case "Int16":
                        return "short";
                    case "UInt16":
                        return "ushort";
                    case "Single":
                        return "float";
                    default:
                        return type.Name.ToLower();
                }
            }
        }

        public static string GetLocalFullName(this Type type)
        {
            return string.Format("{0}.{1}", type.GetLocalNamespace(), type.GetLocalName());
        }

        public static string GetLocalPath(this Type type, string ext)
        {
            return type.GetLocalFullName().ToLower().Replace('.', Path.DirectorySeparatorChar) + ext;
        }

        public static string GetMethodSignature(this MethodInfo method)
        {
            return string.Format("{0}({1})", method.Name, 
                string.Join(",", method.GetParameters().Select(p => GetParameterSignature(p)))
            ).Replace("()", string.Empty);
        }

        public static string GetPropertySignature(this PropertyInfo property)
        {
            if (property.GetIndexParameters().Length > 0)
            {
                return string.Format("Item({0})", string.Join(",", property.GetIndexParameters().Select(p => GetParameterSignature(p))));
            }
            else
            {
                return property.Name;
            }
        }

        private static string GetParameterSignature(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            if (!type.IsGenericType)
            {
                return type.FullName;
            }
            else
            {
                string typeName = type.Name;

                // Replace each type argument to the generated XML representation
                for (int i = 0; i < type.GenericTypeArguments.Length; i++)
                {
                    string find = string.Format("`{0}", i + 1);

                    var param = type.GenericTypeArguments[i];
                    if (param.IsGenericParameter)
                        typeName = typeName.Replace(find, string.Format("{{`{0}}}", i));
                    else
                        typeName = typeName.Replace(find, string.Format("{{{0}}}", param.FullName));
                }

                return string.Format("{0}.{1}", type.Namespace, typeName);
            }
        }

        public static string GetTypeReference(this Type type)
        {
            string generic = string.Empty;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericArguments()[0];
                if (!genericType.IsGenericParameter && genericType != typeof(IForm))
                    generic = string.Format("&lt;{0}&gt;", GetTypeReference(genericType));
            }

            if (type.Namespace.Contains(Program.RootNamespace))
            {
                return string.Format("<see cref='{0}' />{1}", type.GetLocalFullName(), generic);
            }
            else
            {
                return string.Format("<c>{0}</c>{1}", type.GetLocalName(), generic);
            }
        }

        public static string GetCategory(this Type type)
        {
            var namespaceParts = type.GetLocalNamespace().Split('.');
            return namespaceParts.Length > 1 ? string.Format("{0} ({1})", namespaceParts) : namespaceParts[0];
        }

        public static string GetGameTitle(this Type type)
        {
            var namespaceParts = type.GetLocalNamespace().Split('.');
            return namespaceParts.Length > 1 ? namespaceParts[1] : string.Empty;
        }
    }
}
