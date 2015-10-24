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
using Patcher.Data.Plugins.Content;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IMaterialCollection))]
    public sealed class MaterialCollectionProxy : FieldCollectionProxy<Cobj.MaterialData>, IMaterialCollection
    {
        internal Cobj Target { get; set; }

        public int Count
        {
            get
            {
                return GetFieldCount();
            }
        }

        protected override List<Cobj.MaterialData> Fields
        {
            get
            {
                return Target.Materials;
            }
        }

        public void Clear()
        {
            EnsureWritable();
            ClearFields();
        }

        public void Add(IForm item, int count)
        {
            EnsureWritable();

            AddField(new Cobj.MaterialData()
            {
                Item = item.ToFormId(),
                Quantity = (ushort)count
            }, false);
        }

        public void Add(IMaterial item)
        {
            EnsureWritable();

            // Add a copy
            AddField(item.ToField(), true);
        }

        public void Remove(IMaterial item)
        {
            EnsureWritable();

            // Remove by object reference - retrived during an iteration
            RemoveField(item.ToField());
        }

        public IEnumerator<IMaterial> GetEnumerator()
        {
            return GetFieldEnumerator<IMaterial>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
