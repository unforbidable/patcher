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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Proxies;
using Patcher.Data.Plugins.Content.Records.Skyrim;

namespace Patcher.Rules.Compiled.Helpers.Skyrim
{
    [Helper("Engine", typeof(IEngineHelper))]
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

        public IMaterial CreateMaterial(IForm item, int count)
        {
            return (IMaterial)context.Rule.Engine.ProxyProvider.CreateFieldProxy(new Cobj.MaterialData()
            {
                Item = item.ToFormId(),
                Quantity = (uint)count
            }, ProxyMode.Target);
        }

        public IScript CreateScript(string name)
        {
            return (IScript)context.Rule.Engine.ProxyProvider.CreateFieldProxy(new VirtualMachineAdapter.Script()
            {
                Name = name
            }, ProxyMode.Target);
        }
    }
}
