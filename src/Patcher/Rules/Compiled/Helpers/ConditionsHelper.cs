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
            return GenericFunction(number, null, null);
        }

        public ICondition GenericFunction(int number, object paramA)
        {
            return GenericFunction(number, paramA, null);
        }

        public ICondition GenericFunction(int number, object paramA, object paramB)
        {
            var cond = CreateConditionProxy((Function)number);
            var signature = cond.Field.FunctionSignature;

            string func = string.Format("{0}{1}", (Function)number, signature);

            if (signature.HasParameterA)
            {
                if (signature.IsReferenceA)
                {
                    cond.Field.ReferenceParam1 = ParamToReference(func, "first", paramA);
                }
                else if (signature.TypeA == typeof(string))
                {
                    cond.Field.StringParameter1 = ParamToString(func, "first", paramA);
                }
                else if (signature.TypeA == typeof(int))
                {
                    cond.Field.IntParam1 = ParamToInt(func, "first", paramA);
                }
            }
            else if (paramA != null)
            {
                Log.Warning("Unexpected first arguemnt of function {0}.", func);
            }

            if (signature.HasParameterB)
            {
                if (signature.IsReferenceB)
                {
                    cond.Field.ReferenceParam2 = ParamToReference(func, "second", paramB);
                }
                else if (signature.TypeB == typeof(string))
                {
                    cond.Field.StringParameter2 = ParamToString(func, "second", paramB);
                }
                else if (signature.TypeB == typeof(int))
                {
                    cond.Field.IntParam2 = ParamToInt(func, "second", paramB);
                }
            }
            else if (paramB != null)
            {
                Log.Warning("Unexpected second arguemnt of function {0}.", func);
            }

            return cond;
        }

        private int ParamToInt(string func, string label, object param)
        {
            try
            {
                // Try to convert float and string even to int
                return Convert.ToInt32(param);
            }
            catch (Exception)
            {
                Log.Warning("Expected an integer value as the {0} argument of function {1}.", label, func);
                return 0;
            }
        }

        private string ParamToString(string func, string label, object param)
        {
            var text = param as string;
            if (text != null)
            {
                return text;
            }
            else
            {
                Log.Warning("Expected a string as the {0} argument of function {1}.", label, func);
                return string.Empty;
            }
        }

        private uint ParamToReference(string func, string label, object param)
        {
            var formProxy = param as FormProxy;
            if (formProxy != null)
            {
                return formProxy.ToFormId();
            }
            else
            {
                Log.Warning("Expected a form references as the {0} argument of function {1}.", label, func);
                return 0;
            }
        }

        private ConditionProxy CreateConditionProxy(Function number)
        {
            if (!Enum.IsDefined(typeof(Function), number))
                Log.Warning("Unrecognized function '{0}' will be treated as a parameterless function and assigning arguments will produce warnings.", number);

            var proxy = context.Rule.Engine.ProxyProvider.CreateProxy<ConditionProxy>(ProxyMode.Target);
            proxy.Field = new Condition()
            {
                Function = number
            };
            return proxy;
        }
    }
}
