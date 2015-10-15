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
using Patcher.Data.Plugins.Content.Records;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IScriptCollection))]
    public class ScriptCollectionProxy : Proxy, IScriptCollection
    {
        IDictionary<string, ScriptProxy> cache = new SortedDictionary<string, ScriptProxy>();

        internal IHasScripts Record { get; set; }

        public int Count { get { return Record.VirtualMachineAdapter != null ? Record.VirtualMachineAdapter.Scripts.Count : 0; } }

        public IScript this[string name] { get { return GetScriptProxy(name); } }

        public bool Contains(string name)
        {
            return Record.VirtualMachineAdapter != null && Record.VirtualMachineAdapter.Scripts.Where(s => s.Name == name).Any();
        }

        public void Add(IScript script)
        {
            EnsureWritable();
            EnsureVirtualMachineAdapterCreated();

            // Ensure script name has not been already added
            if (Contains(script.Name))
                throw new InvalidOperationException("Script '" + script.Name + "' has been already attached to this object.");

            // Make a deep copy of the other existing script and add it to the backing VMA
            var copy = ((ScriptProxy)script).Script.CopyScript();
            Record.VirtualMachineAdapter.Scripts.Add(copy);
        }

        public void Add(string name)
        {
            EnsureWritable();
            EnsureVirtualMachineAdapterCreated();

            // Ensure script name has not been already added
            if (Contains(name))
                throw new InvalidOperationException("Script '" + name + "' has been already attached to this object.");

            // Add new script with specified name
            Record.VirtualMachineAdapter.Scripts.Add(new VirtualMachineAdapter.Script()
            {
                Name = name
            });
        }

        public void Remove(string name)
        {
            EnsureWritable();
            EnsureVirtualMachineAdapterCreated();

            // Ensure script name has not been already added
            if (!Contains(name))
            {
                Log.Warning("Script '{0}' has not been attached to this object - nothing to remove.");
            }
            else
            {
                Record.VirtualMachineAdapter.Scripts.RemoveAll(s => s.Name == name);
            }
        }

        public void Clear()
        {
            EnsureWritable();

            // No need to remove scripts if there are no scripts
            if (Record.VirtualMachineAdapter != null)
            {
                Record.VirtualMachineAdapter.Scripts.Clear();
            }
        }

        public IEnumerator<IScript> GetEnumerator()
        {
            // Nothing to enumerate if there are no scripts
            if (Record.VirtualMachineAdapter == null)
                yield break;

            // Iterate a copy of script names to allow modification of the collection during iteration
            foreach (var script in Record.VirtualMachineAdapter.Scripts.Select(s => s.Name).ToArray())
            {
                yield return GetScriptProxy(script);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureVirtualMachineAdapterCreated()
        {
            // Create scripts on demand
            if (Record.VirtualMachineAdapter == null)
            {
                Record.VirtualMachineAdapter = new VirtualMachineAdapter();
            }
        }

        private IScript GetScriptProxy(string name)
        {
            if (Record.VirtualMachineAdapter == null)
                throw new InvalidOperationException("There are no scripts attached to this object.");

            var script = Record.VirtualMachineAdapter.Scripts.Where(s => s.Name == name).FirstOrDefault();
            if (script == null)
                throw new ArgumentException("Script '" + name + "' has not been attached to this object.");

            // Also remove from cache
            // The script proxy may not be cached but trying is not an error
            cache.Remove(name);

            return GetScriptProxy(script);
        }

        private IScript GetScriptProxy(VirtualMachineAdapter.Script script)
        {
            if (cache.ContainsKey(script.Name))
            {
                // Pull cached proxy
                return cache[script.Name];
            }
            else
            {
                var proxy = Provider.CreateProxy<ScriptProxy>(Mode);
                proxy.Script = script;

                // Cache proxy for each script
                cache.Add(script.Name, proxy);

                return proxy;
            }
        }
    }
}
