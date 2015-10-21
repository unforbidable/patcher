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
using Patcher.Data.Plugins.Content;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IEffectCollection))]
    public class EffectCollectionProxy : FieldCollectionProxy<Effect>, IEffectCollection
    {
        internal IFeaturingEffects Target { get; set; }

        protected override List<Effect> Fields
        {
            get
            {
                return Target.Effects;
            }
        }

        public int Count
        {
            get
            {
                return GetFieldCount();
            }
        }

        public void Add(IEffect item)
        {
            EnsureWritable();
            AddField(item.ToField(), true);
        }

        public void Clear()
        {
            EnsureWritable();
            ClearFields();
        }

        public void Remove(IEffect item)
        {
            EnsureWritable();

            // Remove by object reference - retrived during an iteration
            RemoveField(item.ToField());
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
