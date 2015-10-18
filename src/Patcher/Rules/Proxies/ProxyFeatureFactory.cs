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

using Patcher.Data.Plugins.Content;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Proxies.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    public static class ProxyFeatureFactory
    {
        public static ConditionCollectionProxy CreateConditionCollectionProxy(this IHasConditions target, ProxyProvider provider, ProxyMode mode)
        {
            var proxy = provider.CreateProxy<ConditionCollectionProxy>(mode);
            proxy.Fields = target.Conditions;
            return proxy;
        }

        public static void UpdateFromConditionCollectionProxy(this IHasConditions target, IConditionCollection proxy)
        {
            if (proxy == null)
                throw new ArgumentNullException("value", "Cannot assign a NULL to a collection.");

            target.Conditions = ((ConditionCollectionProxy)proxy).CopyFields();
        }
    }
}
