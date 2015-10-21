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
using System.Collections;
using Patcher.Data.Plugins.Content;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IScriptCollection))]
    public class ScriptCollectionProxy : FieldCollectionProxy<VirtualMachineAdapter.Script>, IScriptCollection
    {
        internal IFeaturingScripts Target { get; set; }

        protected override List<VirtualMachineAdapter.Script> Fields
        {
            get
            {
                return Target.VirtualMachineAdapter == null ? null : Target.VirtualMachineAdapter.Scripts;
            }
        }

        public int Count
        {
            get
            {
                return GetFieldCount();
            }
        }

        public IScript this[string name] { get { return GetField<IScript>(f => f.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)); } }

        public bool Contains(string name)
        {
            return Fields != null && Fields.Where(f => f.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).Any();
        }

        public void Add(IScript script)
        {
            EnsureWritable();

            // Ensure script name has not been already added
            if (Contains(script.Name))
                throw new InvalidOperationException("Script '" + script.Name + "' has been already added to this object.");

            // Make a deep copy of the other existing script and add it to the backing VMA
            EnsureVirtualMachineAdapterCreated();
            AddField(script.ToField(), true);
        }

        public void Add(string name)
        {
            EnsureWritable();

            // Ensure script name has not been already added
            if (Contains(name))
                throw new InvalidOperationException("Script '" + name + "' has been already added to this object.");

            // Add new script with specified name
            EnsureVirtualMachineAdapterCreated();
            AddField(new VirtualMachineAdapter.Script()
            {
                Name = name
            }, false);
        }

        public void Remove(string name)
        {
            EnsureWritable();

            // Ensure script name has not been already added
            if (!Contains(name))
            {
                Log.Warning("Script '{0}' not found in script collection - nothing to remove.");
            }
            else
            {
                Fields.RemoveAll(s => s.Name == name);
            }
        }

        public void Clear()
        {
            EnsureWritable();
            ClearFields();
        }

        public IEnumerator<IScript> GetEnumerator()
        {
            return GetFieldEnumerator<IScript>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureVirtualMachineAdapterCreated()
        {
            // Create scripts on demand
            if (Target.VirtualMachineAdapter == null)
            {
                Target.VirtualMachineAdapter = new VirtualMachineAdapter();
            }
        }      
    }
}
