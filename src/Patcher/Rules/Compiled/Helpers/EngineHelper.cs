using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Proxies;

namespace Patcher.Rules.Compiled.Helpers
{
    sealed class EngineHelper : IEngineHelper
    {
        readonly CompiledRuleContext context;

        public EngineHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public IEffect CreateEffect(IForm baseEffect, float magnitude, int area, int duration)
        {
            return (IEffect)context.Rule.Engine.ProxyProvider.CreateFieldProxy(new Effect()
            {
                BaseEffect = baseEffect.ToFormId(),
                Magnitude = magnitude,
                Area = (uint)area,
                Duration = (uint)duration
            }, ProxyMode.Target);
        }
    }
}
