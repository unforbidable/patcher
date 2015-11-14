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
    sealed class ObjectDumper
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

        string prefix;

        public ObjectDumper(string prefix)
        {
            this.prefix = prefix;
        }

        void DoDump(string text)
        {
            Log.Fine("{0} {1}", prefix, text);
        }

        public void DumpObject(object value)
        {
            DumpObject(null, value);
        }

        public void DumpObject(string name, object value)
        {
            if (value == null)
            {
                // Null values
                DumpText(name, "NULL");
            }
            else
            {
                Type type = value.GetType();
                if (type.IsPrimitive || type.IsEnum)
                {
                    // Primitive value - single line
                    DumpText(name, value.ToString());
                }
                else if (type == typeof(string))
                {
                    // String - single line, quotes
                    DumpText(name, "'{0}'", value);
                }
                else
                {
                    var formProxy = value as FormProxy;
                    var dumpable = value as IDumpabled;

                    if (dumpable != null)
                    {
                        // Call custom method of custom dumpable objects
                        dumpable.Dump(name, this);
                    }
                    else if (formProxy != null && currentDepth > 0)
                    {
                        // Print just the form if reference
                        DumpText(name, "{0}", formProxy);
                    }
                    else
                    {
                        if (formProxy != null)
                        {
                            // Print the content of the root form only
                            DumpText(name, "{0} {{", formProxy);
                        }
                        else
                        {
                            // Print the content of an arbitrary object
                            DumpText(name, "{");
                        }

                        if (Enter())
                        {
                            bool anyOutput = false;

                            // Get all public instance properties (skip inherited and indexers)
                            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == type && p.GetIndexParameters().Length == 0).OrderBy(p => p.Name))
                            {
                                object val = property.GetValue(value, null);
                                DumpObject(property.Name, val);
                                anyOutput = true;
                            }

                            // Print enumerable items
                            var asEnumerable = value as IEnumerable;
                            if (asEnumerable != null && asEnumerable.GetEnumerator().MoveNext())
                            {
                                DumpText("[");
                                if (Enter())
                                {
                                    foreach (var obj in (IEnumerable)value)
                                    {
                                        DumpObject(obj);
                                    }
                                    Leave();
                                }
                                DumpText("]");

                                anyOutput = true;
                            }

                            if (!anyOutput && CurrentDepth >= 2)
                            {
                                // ToString if no properties and not an enumerable, unless current depth < 2
                                DumpText(value.ToString());
                            }

                            Leave();
                        }
                        DumpText("}");
                    }
                }
            }
        }

        public void DumpText(string text)
        {
            DumpText(null, text);
        }

        public void DumpText(string name, string format, params object[] args)
        {
            DumpText(name, string.Format(format, args));
        }

        public void DumpText(string name, string text)
        {
            string prefix = !string.IsNullOrEmpty(name) ? string.Format("{0} = ", name) : string.Empty;
            DoDump(string.Format("{0}{1}{2}", indents[currentDepth], prefix, text));
        }

        int currentDepth = 0;

        public int CurrentDepth { get { return currentDepth; } }

        public bool Enter()
        {
            if (currentDepth >= maxDepth - 1)
            {
                DoDump(string.Format("{0}...", indents[maxDepth - 1]));
                return false;
            }

            currentDepth++;
            return true;
        }

        public void Leave()
        {
            currentDepth--;
        }
    }
}
