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
using System.Linq.Expressions;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    class HelperInfo
    {
        readonly HelperProvider provider;

        public string Name { get; private set; }
        public Type InterfaceType { get; private set; }
        public bool DebugModeOnly { get; private set; }

        static Type[] ctorParameters = new Type[] { typeof(CompiledRuleContext) };
        ParameterExpression ruleContextParameterExpr = Expression.Parameter(typeof(CompiledRuleContext));

        readonly Func<CompiledRuleContext, object> creator;

        public HelperInfo(HelperProvider provider, Type type)
        {
            this.provider = provider;

            var attribute = type.GetCustomAttributes(typeof(HelperAttribute), false).Cast<HelperAttribute>().Single();
            Name = attribute.Name;
            InterfaceType = attribute.InterfaceType;
            DebugModeOnly = attribute.DebugModeOnly;

            var newExpr = Expression.New(type.GetConstructor(ctorParameters), ruleContextParameterExpr);
            creator = Expression.Lambda<Func<CompiledRuleContext, object>>(newExpr, ruleContextParameterExpr).Compile();
        }

        public object CreateInstance(CompiledRuleContext ruleContext)
        {
            return creator.Invoke(ruleContext);
        }
    }
}
