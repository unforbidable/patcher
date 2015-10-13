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

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IScript))]
    public class ScriptProxy : Proxy, IScript
    {
        internal VirtualMachineAdapter.Script Script { get; set; }

        public string Name { get { return Script.Name; } }
        public ScriptPropertyCollection Properties { get { return new ScriptPropertyCollection(Script); } }

        public void AddProperty(string name, Compiled.Constants.Type type)
        {
            EnsureWritable();

            if (Script.Properties.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Property '" + name + "' has been already added.");

            var prop = new VirtualMachineAdapter.ScriptProperty()
            {
                Name = name,
                Type = type.ToScriptPropertType()
            };

            Script.Properties.Add(prop);
        }

        public void SetProperty(string name, int value)
        {
            SetProperty(name, value, null);
        }

        public void SetProperty(string name, string value)
        {
            SetProperty(name, value, null);
        }

        public void SetProperty(string name, float value)
        {
            SetProperty(name, value, null);
        }

        public void SetProperty(string name, bool value)
        {
            SetProperty(name, value, null);
        }

        public void SetProperty(string name, IForm value)
        {
            SetProperty(name, value, null);
        }

        public void SetProperty(string name, int value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
        }

        public void SetProperty(string name, string value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
        }

        public void SetProperty(string name, float value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
        }

        public void SetProperty(string name, bool value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(value, index);
        }

        public void SetProperty(string name, IForm value, int? index)
        {
            EnsureWritable();
            GetProperty(name).SetValue(-1, value.FormId, index);
        }

        public void ResetProperty(string name)
        {
            EnsureWritable();
            GetProperty(name).ResetValue();
        }

        private VirtualMachineAdapter.ScriptProperty GetProperty(string name)
        {
            var prop = Script.Properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (prop == null)
                throw new ArgumentException("Property '" + name + "' does not exist.");

            return prop;
        }

        /// <summary>
        /// Represets enumerable list of script properties used for debugging.
        /// </summary>
        public class ScriptPropertyCollection : IEnumerable<ScriptProperty>
        {
            readonly VirtualMachineAdapter.Script script;

            public int Count { get { return script.Properties.Count; } }

            public ScriptPropertyCollection(VirtualMachineAdapter.Script script)
            {
                this.script = script;
            }

            public IEnumerator<ScriptProperty> GetEnumerator()
            {
                foreach (var prop in script.Properties)
                {
                    yield return new ScriptProperty(prop);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Represents a script property with enumerable values used for debugging.
        /// </summary>
        public class ScriptProperty : IEnumerable<object>
        {
            readonly VirtualMachineAdapter.ScriptProperty property;

            public string Name { get { return property.Name; } }
            public ScriptPropertyType Type { get { return property.Type; } }

            public ScriptProperty(VirtualMachineAdapter.ScriptProperty property)
            {
                this.property = property;
            }

            public IEnumerator<object> GetEnumerator()
            {
                return property.GetValues().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

}
