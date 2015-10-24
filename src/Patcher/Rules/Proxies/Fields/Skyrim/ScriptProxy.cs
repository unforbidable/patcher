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

using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Forms;
using System.Collections;
using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Rules.Compiled.Constants;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IScript))]
    public class ScriptProxy : FieldProxy<VirtualMachineAdapter.Script>, IScript, IDumpabled
    {
        public string Name { get { return Field.Name; } }

        public IScript AddProperty(string name, Types type)
        {
            EnsureWritable();

            if (Field.Properties.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Property '" + name + "' has been already added.");

            var prop = new VirtualMachineAdapter.ScriptProperty()
            {
                Name = name,
                Type = type.ToScriptPropertType()
            };

            Field.Properties.Add(prop);
            return this;
        }

        public IScript SetProperty(string name, int value)
        {
            SetProperty(name, value, null);
            return this;
        }

        public IScript SetProperty(string name, string value)
        {
            SetProperty(name, value, null);
            return this;
        }

        public IScript SetProperty(string name, float value)
        {
            SetProperty(name, value, null);
            return this;
        }

        public IScript SetProperty(string name, bool value)
        {
            SetProperty(name, value, null);
            return this;
        }

        public IScript SetProperty(string name, IForm value)
        {
            SetProperty(name, value, null);
            return this;
        }

        public IScript SetProperty(string name, int value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
            return this;
        }

        public IScript SetProperty(string name, string value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
            return this;
        }

        public IScript SetProperty(string name, float value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
            return this;
        }

        public IScript SetProperty(string name, bool value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
            return this;
        }

        public IScript SetProperty(string name, IForm value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(-1, value.FormId, index);
            return this;
        }

        public IScript ResetProperty(string name)
        {
            EnsureWritable();
            GetProperty(name).ResetValue();
            return this;
        }

        private VirtualMachineAdapter.ScriptProperty GetProperty(string name)
        {
            var prop = Field.Properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (prop == null)
                throw new ArgumentException("Property '" + name + "' does not exist.");

            return prop;
        }

        void IDumpabled.Dump(ObjectDumper dumper)
        {
            // Dump script properties
            dumper.DumpText("Properties", "{");
            if (dumper.Enter())
            {
                dumper.DumpText("Count", Field.Properties.Count.ToString());
                dumper.DumpText("[");
                if (dumper.Enter())
                {
                    foreach (var prop in Field.Properties)
                    {
                        // Dump property
                        dumper.DumpText(prop.Name, "{");
                        if (dumper.Enter())
                        {
                            dumper.DumpText("Type", prop.Type.ToString());
                            dumper.DumpText("Values", ScriptPropertyValuesToString(prop));

                            dumper.Leave();
                        }
                        dumper.DumpText("}");
                    }
                    dumper.Leave();
                }
                dumper.DumpText("]");
                dumper.Leave();
            }
            dumper.DumpText("}");
        }

        private string ScriptPropertyValuesToString(VirtualMachineAdapter.ScriptProperty prop)
        {
            if (!prop.IsSet)
            {
                return "(not assigned)";
            }
            else if (!prop.IsArray)
            {
                return SingleScriptPropertyValueToString(prop.GetValues().First());
            }
            else
            {
                return string.Format("[ {0} ]", string.Join(",", prop.GetValues().Select(v => SingleScriptPropertyValueToString(v))));
            }
        }

        private string SingleScriptPropertyValueToString(object value)
        {
            if (value.GetType() == typeof(VirtualMachineAdapter.ObjectProperty))
            {
                var prop = (VirtualMachineAdapter.ObjectProperty)value;

                var formProxy = Provider.CreateReferenceProxy<IForm>(prop.FormId);

                if (prop.AliasId != -1)
                {
                    return string.Format("{{ AliasId = {0}, Reference = {1} }}", prop.AliasId, formProxy);
                }
                else
                {
                    return string.Format("{{ Reference = {0} }}", formProxy);
                }
            }
            else
            {
                return value.ToString();
            }
        }
    }

}
