using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    public interface IEffectCollection : IEnumerable<IEffect>
    {
        int Count { get; }
        void Add(IEffect effect);
        void Remove(IEffect effect);
        void Clear();
    }
}
