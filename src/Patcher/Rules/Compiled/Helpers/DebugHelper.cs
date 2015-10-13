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

using Patcher.Rules.Proxies.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    sealed class DebugHelper : IDebugHelper
    {
        readonly CompiledRuleContext context;

        public DebugHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public void Break()
        {
            Debugger.Break();
        }

        public void Message(string text)
        {
            Log.Fine(string.Format("{0}: [MSG] {1}", context.Rule, text));
        }

        public void Assert(bool condition, string text)
        {
            if (!condition)
                throw new CompiledRuleAssertException(text);
        }

        public void Dump(object value)
        {
            foreach (var text in DoDumpObject(0, string.Empty, value))
            {
                Log.Fine(text);
            }
        }

        public void Dump(object value, string name)
        {
            foreach (var text in DoDumpObject(0, name, value))
            {
                Log.Fine(text);
            }
        }

        private IEnumerable<string> DoDumpObject(int depth, string name, object value)
        {
            // Max delpt allowed is maxDepth
            if (depth > maxDepth)
                yield return "...";

            if (value == null)
            {
                // Null values
                yield return DoDumpText(depth, name, "NULL");
            }
            else
            {
                Type type = value.GetType();
                if (type.IsPrimitive || type.IsEnum)
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

                            // Print inherited property Editor ID only if form is loaded and the form record contains (supports) Editor ID
                            if (formProxy.Form != null && formProxy.Form.EditorId != null)
                                yield return DoDumpText(depth, "EditorId", formProxy.Form.EditorId);
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

                    // Get all public instance properties (except inherited)
                    foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == type))
                    {
                        object val = property.GetValue(value, null);
                        foreach (var text in DoDumpObject(depth, property.Name, val))
                            yield return text;
                    }

                    // Print enumerable items
                    var asEnumerable = value as IEnumerable;// type.GetInterfaces().Where(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Any();
                    if (asEnumerable != null)
                    {
                        yield return DoDumpText(depth++, null, "Items = [");
                        foreach (var obj in (IEnumerable)value)
                        {
                            foreach (var text in DoDumpObject(depth, string.Empty, obj))
                                yield return text;
                        }
                        yield return DoDumpText(--depth, null, "]");
                    }

                    yield return DoDumpText(--depth, null, "}");
                }
            }
        }

        private string DoDumpText(int indent, string name, string format, params object[] args)
        {
            return DoDumpText(indent, name, string.Format(format, args));
        }

        private string DoDumpText(int indent, string name, string text)
        {
            string prefix = !string.IsNullOrEmpty(name) ? string.Format("{0} = ", name) : string.Empty;
            return string.Format("{0}: [DUMP] {1}{2}{3}", context.Rule, indents[indent], prefix, text);
        }

        const int maxDepth = 3;
        const int idenentStep = 4;
        static string[] indents =
        {
            string.Empty,
            new string(Enumerable.Range(0, 1 * idenentStep).Select(i => ' ').ToArray()),
            new string(Enumerable.Range(0, 2 * idenentStep).Select(i => ' ').ToArray()),
            new string(Enumerable.Range(0, 3 * idenentStep).Select(i => ' ').ToArray())
        };
    }
}
