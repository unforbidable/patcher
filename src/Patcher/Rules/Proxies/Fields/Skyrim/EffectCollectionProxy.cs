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
using System.Collections;
using Patcher.Data.Plugins.Content.Fields.Skyrim;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IEffectCollection))]
    public class EffectCollectionProxy : FieldCollectionProxy<Effect>, IEffectCollection
    {
        public int Count
        {
            get
            {
                return Fields.Count;
            }
        }

        public void Add(IEffect item)
        {
            EnsureWritable();

            if (item == null)
                throw new ArgumentNullException("item", "Cannot add NULL to the collection.");

            // Add a copy
            Fields.Add((Effect)ProxyToField(item).CopyField());
        }

        public void Clear()
        {
            EnsureWritable();
            Fields.Clear();
        }

        public void Remove(IEffect item)
        {
            EnsureWritable();

            if (item == null)
                throw new ArgumentNullException("item", "Cannot remove NULL from the collection.");

            // Remove by object reference - retrived during an iteration
            if (!Fields.Remove(ProxyToField(item)))
            {
                Log.Warning("Item {0} was not found in the collection and could not be removed.", item);
            }
        }

        public IEnumerator<IEffect> GetEnumerator()
        {
            return GetFieldEnumerator<IEffect>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
