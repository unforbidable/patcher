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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Documenter
{
    class PageGenerator
    {
        readonly XDocument source;
        readonly Assembly assembly;

        public PageGenerator(XDocument source, Assembly assembly)
        {
            this.source = source;
            this.assembly = assembly;
        }

        public void GeneratePages()
        {
            var transStructure = new XslCompiledTransform();
            transStructure.Load("TransformStructure.xslt");
            var transIndex = new XslCompiledTransform();
            transIndex.Load("TransformIndex.xslt");
            var transNodes = new XslCompiledTransform();
            transNodes.Load("TransformNodes.xslt");

            var index = new XDocument();
            var indexElement = new XElement("index");
            index.Add(indexElement);

            foreach (var type in assembly.GetTypes()
                .OrderBy(t => t.GetLocalName()))
            {
                // Ignore extensions (extension methods will be added to the respective class they extend)
                if (type.FullName.Contains(".Extensions."))
                    continue;

                var target = new XDocument();
                target.Add(GetTypeXmlElement(type, true));

                string xmlFilePath = Path.Combine(Program.TargetPath, type.GetLocalPath(".xml"));
                string tmpFilePath = Path.Combine(Program.TargetPath, type.GetLocalPath(".tmp"));
                string htmlFilePath = Path.Combine(Program.TargetPath, type.GetLocalPath(".html"));

                Directory.CreateDirectory(Path.GetDirectoryName(xmlFilePath));
                target.Save(xmlFilePath);

                transStructure.Transform(xmlFilePath, tmpFilePath);
                transNodes.Transform(tmpFilePath, htmlFilePath);
                File.Delete(tmpFilePath);

                indexElement.Add(GetTypeXmlElement(type, false));
            }

            string xmlIndexPath = Path.Combine(Program.TargetPath, "Index.xml");
            string tmpIndexPath = Path.Combine(Program.TargetPath, "Index.tmp");
            string htmlIndexPath = Path.Combine(Program.TargetPath, "Index.html");

            Directory.CreateDirectory(Path.GetDirectoryName(xmlIndexPath));
            index.Save(xmlIndexPath);

            transIndex.Transform(xmlIndexPath, tmpIndexPath);
            transNodes.Transform(tmpIndexPath, htmlIndexPath);
            File.Delete(tmpIndexPath);
        }

        private XElement GetTypeXmlElement(Type type, bool details)
        {
            XElement typeElement = new XElement("type");
            typeElement.Add(new XAttribute("name", type.GetLocalName()));
            typeElement.Add(new XAttribute("fullname", type.GetLocalFullName()));
            typeElement.Add(new XAttribute("category", type.GetLocalNamespace().Split('.')[0]));
            typeElement.Add(GetSummaryTextXElement(type));

            if (details)
            {
                var fields = type.GetFields()
                    .Where(f => !f.IsSpecialName)
                    .OrderBy(f => f.Name);

                foreach (var field in fields)
                {
                    typeElement.Add(GetFieldXElement(field));
                }

                var properties = type.GetInterfaces()
                    .Where(t => t.Namespace.StartsWith(Program.RootNamespace))
                    .SelectMany(t => t.GetProperties())
                    .Union(type.GetProperties())
                    .OrderBy(m => m.Name);

                foreach (var property in properties)
                {
                    typeElement.Add(GetPropertyXElement(property));
                }

                var methods = type.GetInterfaces()
                    .Where(t => t.Namespace.StartsWith(Program.RootNamespace))
                    .SelectMany(t => t.GetMethods())
                    .Union(type.GetMethods().Where(m => m.DeclaringType.Namespace.StartsWith(Program.RootNamespace)))
                    .Where(m => !m.IsSpecialName && !m.IsGenericMethod)
                    .OrderBy(m => m.Name);

                foreach (var method in methods)
                {
                    typeElement.Add(GetMethodXElement(method));
                }

                var extensions = assembly.GetTypes()
                    .Where(t => t.FullName.Contains(".Extensions."))
                    .SelectMany(t => t.GetMethods())
                    .Where(m => !m.IsSpecialName && !m.IsGenericMethod)
                    .OrderBy(m => m.Name);

                foreach (var method in extensions)
                {
                    var firstParam = method.GetParameters().FirstOrDefault();
                    if (firstParam != null && (firstParam.ParameterType == type || firstParam.ParameterType.IsGenericType && firstParam.ParameterType.GetGenericTypeDefinition() == type))
                    {
                        typeElement.Add(GetMethodXElement(method));
                    }
                }
            }

            return typeElement;
        }

        private XElement GetFieldXElement(FieldInfo field)
        {
            var element = new XElement("field");
            element.Add(new XAttribute("name", field.Name));
            element.Add(GetSummaryTextXElement(field));
            return element;
        }

        private XElement GetPropertyXElement(PropertyInfo property)
        {
            var element = new XElement("property");
            element.Add(new XAttribute("name", property.Name));
            element.Add(GetSignatureXmlElement(property));
            element.Add(GetSummaryTextXElement(property));
            return element;
        }

        private XElement GetMethodXElement(MethodInfo method)
        {
            var element = new XElement("method");
            element.Add(new XAttribute("name", method.Name));
            element.Add(new XAttribute("extension", method.IsDefined(typeof(ExtensionAttribute), true)));
            element.Add(GetSignatureXmlElement(method));
            element.Add(GetSummaryTextXElement(method));
            return element;
        }

        private XElement GetSummaryTextXElement(Type type)
        {
            return GetSummaryTextXElement("T:" + type.FullName);
        }

        private XElement GetSummaryTextXElement(MethodInfo method)
        {
            return GetSummaryTextXElement("M:" + method.DeclaringType.FullName + "." + method.GetMethodSignature());
        }

        private XElement GetSummaryTextXElement(PropertyInfo property)
        {
            return GetSummaryTextXElement("P:" + property.DeclaringType.FullName + "." + property.Name);
        }

        private XElement GetSummaryTextXElement(FieldInfo field)
        {
            return GetSummaryTextXElement("F:" + field.DeclaringType.FullName + "." + field.Name);
        }

        private XElement GetSummaryTextXElement(string name)
        {
            var summary = source.Descendants()
                .Where(e => e.Name == "member" && e.Attribute("name").Value == name)
                .Select(m => m.Element("summary"))
                .FirstOrDefault();

            if (summary != null)
            {
                summary = XElement.Parse(summary.ToString());

                // Replace <see> links
                foreach (var child in summary.Descendants().Where(c => c.Name == "see"))
                {
                    var cref = child.Attribute("cref");
                    var typeFullName = cref.Value.Substring(2);
                    var type = Type.GetType(typeFullName) ?? assembly.GetType(typeFullName);
                    cref.Value = type.GetLocalFullName();
                }
                return summary;
            }
            else
            {
                return new XElement("summary", "TODO");
            }
        }

        private XElement GetSignatureXmlElement(MethodInfo method)
        {
            string returnType = method.ReturnType.GetTypeReference();
            int skip = method.IsDefined(typeof(ExtensionAttribute), true) ? 1 : 0;
            var signature = string.Format("{0} {1}({2})", returnType, method.Name,
                string.Join(",", method.GetParameters().Skip(skip).Select(p => p.ParameterType.GetTypeReference())));
            return XElement.Parse(string.Format("<signature>{0}</signature>", signature));
        }

        private XElement GetSignatureXmlElement(PropertyInfo property)
        {
            string returnType = property.PropertyType.GetTypeReference();
            var signature = string.Format("{0} {1}", returnType, property.Name);
            return XElement.Parse(string.Format("<signature>{0}</signature>", signature));
        }

    }
}
