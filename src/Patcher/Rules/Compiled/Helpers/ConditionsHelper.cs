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
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Proxies.Fields.Skyrim;
using Patcher.Rules.Proxies;
using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Rules.Proxies.Forms;
using Patcher.Data.Plugins.Content.Functions.Skyrim;

namespace Patcher.Rules.Compiled.Helpers
{
    sealed class ConditionsHelper : IConditionsHelper
    {
        readonly CompiledRuleContext context;

        public ConditionsHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public ICondition GenericFunction(int number)
        {
            return CreateConditionProxy((Function)number).CheckParams();
        }

        public ICondition GenericFunction(int number, object paramA)
        {
            return CreateConditionProxy((Function)number).SetParam(0, paramA).CheckParams();
        }

        public ICondition GenericFunction(int number, object paramA, object paramB)
        {
            return CreateConditionProxy((Function)number).SetParam(0, paramA).SetParam(1, paramB).CheckParams();
        }

        private ConditionProxy CreateConditionProxy(Function number)
        {
            if (!Enum.IsDefined(typeof(Function), number))
                Log.Warning("Unrecognized function '{0}' will be treated as a parameterless function and assigning arguments will produce warnings.", number);

            // Create proxy in Target mode so that it can be modified
            var proxy = context.Rule.Engine.ProxyProvider.CreateProxy<ConditionProxy>(ProxyMode.Target);
            proxy.Field = new Condition()
            {
                Function = number
            };
            return proxy;
        }
    }
}
