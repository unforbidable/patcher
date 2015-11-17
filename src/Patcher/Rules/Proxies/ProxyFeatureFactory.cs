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
using Patcher.Data.Plugins.Content.Fields;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Proxies.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    static class ProxyFeatureFactory
    {
        public static ConditionCollectionProxy CreateConditionCollectionProxy(this IFeaturingConditions target, Proxy parent)
        {
            var proxy = parent.Provider.CreateProxy<ConditionCollectionProxy>(parent.Mode);
            proxy.Target = target;
            return proxy;
        }

        public static void UpdateFromConditionCollectionProxy(this IFeaturingConditions target, IConditionCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot assign a NULL to a collection.");

            target.Conditions = ((ConditionCollectionProxy)value).CopyFieldCollection();
        }

        public static ObjectBoundsProxy CreateObjectBoundsProxy(this IFeaturingObjectBounds target, Proxy parent)
        {
            var proxy = parent.Provider.CreateProxy<ObjectBoundsProxy>(parent.Mode);
            proxy.Record = target;
            return proxy;
        }

        public static void UpdateFromObjectBoundsProxy(this IFeaturingObjectBounds target, IObjectBounds value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot assign a NULL to object bounds.");

            // Assign copy of bounds of another record
            target.ObjectBounds = (ObjectBounds)((ObjectBoundsProxy)value).Record.ObjectBounds.CopyField();
        }

        public static ScriptCollectionProxy CreateVirtualMachineAdapterProxy(this IFeaturingScripts target, Proxy parent)
        {
            ScriptCollectionProxy proxy = parent.Provider.CreateProxy<ScriptCollectionProxy>(parent.Mode);
            proxy.Target = target;
            return proxy;
        }

        public static void UpdateFromVirtualMachineAdapterProxy(this IFeaturingScripts target, IScriptCollection value)
        {
            var cast = (ScriptCollectionProxy)value;
            if (value == null || cast.Target.VirtualMachineAdapter == null)
            {
                // Assigned null or a script collection proxy or record has no scripts
                target.VirtualMachineAdapter = null;
            }
            else
            {
                // Assign scripts of another record are assigned, make a copy
                target.VirtualMachineAdapter = (VirtualMachineAdapter)cast.Target.VirtualMachineAdapter.CopyField();
            }
        }

        public static EffectCollectionProxy CreateEffectCollectionProxy(this IFeaturingEffects target, Proxy parent)
        {
            var proxy = parent.Provider.CreateProxy<EffectCollectionProxy>(parent.Mode);
            proxy.Target = target;
            return proxy;
        }

        public static void UpdateFromConditionCollectionProxy(this IFeaturingEffects target, IEffectCollection value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot assign a NULL to a collection.");

            target.Effects = ((EffectCollectionProxy)value).CopyFieldCollection();
        }      
    }
}
