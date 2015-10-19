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

        public void Add(IEffect effect)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Remove(IEffect effect)
        {
            throw new NotImplementedException();
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
