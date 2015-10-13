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
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.\

using Patcher.Data.Plugins;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Proxies.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.Rules
{
    class ObjectDumper
    {
        const int maxDepth = 12;
        const int idenentStep = 4;
        static string[] indents;

        static ObjectDumper()
        {
            indents = new string[maxDepth];
            indents[0] = string.Empty;

            for (int i = 1; i < maxDepth; i++)
            {
                indents[i] = new string(Enumerable.Range(0, i * idenentStep).Select(e => ' ').ToArray());
            }
        }

        readonly RuleEngine engine;

        public ObjectDumper(RuleEngine engine)
        {
            this.engine = engine;
        }

        public IEnumerable<string> DumpObject(int depth, string name, object value)
        {
            if (value == null)
            {
                // Null values
                yield return DoDumpText(depth, name, "NULL");
            }
            else
            {
                Type type = value.GetType();
                if (type == typeof(VirtualMachineAdapter.ObjectProperty))
                {
                    var prop = (VirtualMachineAdapter.ObjectProperty)value;

                    yield return DoDumpText(depth++, name, "{");
                    // Alias ID
                    if (prop.AliasId != -1)
                    {
                        yield return DoDumpText(depth, "AliasId", prop.AliasId.ToString());
                    }

                    // Form
                    if (prop.FormId == 0)
                    {
                        yield return DoDumpText(depth, "Form", Form.NullFormString);
                    }
                    else if (engine.Context.Forms.Contains(prop.FormId))
                        yield return DoDumpText(depth, "Form", engine.Context.Forms[prop.FormId].ToString());
                    else
                        yield return DoDumpText(depth, "Form", Form.GetUnresolvedFormString(prop.FormId));

                    yield return DoDumpText(--depth, name, "}");
                }
                else if (type.IsPrimitive || type.IsEnum)
                {
                    // Primitive value - single line
                    yield return DoDumpText(depth, name, value.ToString());
                }
                else if (type == typeof(string))
                {
                    // String - single line, quotes
                    yield return DoDumpText(depth, name, "'{0}'", value);
                }
                else
                {
                    if (typeof(FormProxy).IsAssignableFrom(type))
                    {
                        var formProxy = (FormProxy)value;
                        if (depth == 0)
                        {
                            // Print the content of the root form only
                            yield return DoDumpText(depth++, name, "{0} {{", formProxy);
                        }
                        else
                        {
                            // Print just the form if reference
                            yield return DoDumpText(depth++, name, "{0}", formProxy);
                            yield break;
                        }
                    }
                    else
                    {
                        // Some arbitrary object
                        yield return DoDumpText(depth++, name, "{");
                    }

                    // Get all public instance properties (skip inherited and indexers)
                    foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == type && p.GetIndexParameters().Length == 0))
                    {
                        object val = property.GetValue(value, null);
                        foreach (var text in DumpObject(depth, property.Name, val))
                            yield return text;
                    }

                    // Print enumerable items
                    var asEnumerable = value as IEnumerable;
                    if (asEnumerable != null)
                    {
                        yield return DoDumpText(depth++, null, "[");
                        foreach (var obj in (IEnumerable)value)
                        {
                            foreach (var text in DumpObject(depth, string.Empty, obj))
                                yield return text;
                        }
                        yield return DoDumpText(--depth, null, "]");
                    }

                    yield return DoDumpText(--depth, null, "}");
                }
            }
        }

        private string DoDumpText(int depth, string name, string format, params object[] args)
        {
            return DoDumpText(depth, name, string.Format(format, args));
        }

        private string DoDumpText(int depth, string name, string text)
        {
            // Too deep
            if (depth >= maxDepth - 1)
            {
                return string.Format("{0}...", indents[maxDepth - 1]);
            }
            else
            {
                string prefix = !string.IsNullOrEmpty(name) ? string.Format("{0} = ", name) : string.Empty;
                return string.Format("{0}{1}{2}", indents[depth], prefix, text);
            }
        }
    }
}
