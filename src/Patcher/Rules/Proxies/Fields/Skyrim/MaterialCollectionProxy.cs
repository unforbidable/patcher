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

using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Forms;
using System.Collections;
using Patcher.Data.Plugins.Content.Records.Skyrim;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IMaterialCollection))]
    public sealed class MaterialCollectionProxy : Proxy, IMaterialCollection
    {
        internal Cobj Record { get; set; }

        public int Count
        {
            get
            {
                return Record.Materials == null ? 0 : Record.Materials.Count;
            }
        }

        public void Add(IMaterial material)
        {
            EnsureWritable();

            if (material == null)
            {
                throw new ArgumentNullException("material", "Cannot add NULL to material collection.");
            }

            EnsureMaterialsCreated();

            var proxy = (MaterialProxy)material;
            Record.Materials.Add((Cobj.MaterialData)proxy.MaterialData.CopyField());
        }

        public void Add(IForm item, int count)
        {
            EnsureWritable();

            if (item == null || item.FormId == 0)
            {
                throw new ArgumentNullException("item", "Cannot add NULL to material collection.");
            }

            EnsureMaterialsCreated();

            Record.Materials.Add(new Cobj.MaterialData()
            {
                Item = item.FormId,
                Quantity = (uint)count
            });
        }

        public void Remove(IMaterial material)
        {
            EnsureWritable();

            if (material == null)
            {
                throw new ArgumentNullException("material", "Cannot add NULL to material collection.");
            }

            var proxy = (MaterialProxy)material;
            if (Record.Materials == null || !Record.Materials.Contains(proxy.MaterialData))
            {
                Log.Warning("Item not found in material collection - nothing to remove.");
            }
            else
            {
                Record.Materials.Remove(proxy.MaterialData);
            }
        }

        public void Clear()
        {
            EnsureWritable();

            if (Record.Materials != null)
            {
                Record.Materials.Clear();
            }
        }

        public IEnumerator<IMaterial> GetEnumerator()
        {
            if (Record.Materials != null && Record.Materials.Count > 0)
            {
                // Iterate a copy of the collection so that materials can be removed during the iteration
                // Shallow copy is fine
                foreach (var materialData in Record.Materials.ToArray())
                {
                    var proxy = Provider.CreateProxy<MaterialProxy>(Mode);
                    proxy.MaterialData = materialData;
                    yield return proxy;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void EnsureMaterialsCreated()
        {
            if (Record.Materials == null)
                Record.Materials = new List<Cobj.MaterialData>();
        }
    }
}
