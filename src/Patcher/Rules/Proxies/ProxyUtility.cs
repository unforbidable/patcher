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
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies.Fields;
using Patcher.Rules.Proxies.Fields.Skyrim;
using Patcher.Rules.Proxies.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    static class ProxyUtility
    {
        /// <summary>
        /// Gets the Form ID of the form this proxy represents, or 0 if the proxy is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form"></param>
        /// <returns></returns>
        public static uint ToFormId<TForm>(this TForm form) where TForm : IForm
        {
            if (form == null)
                return 0;
            else
                return form.FormId;
        }

        /// <summary>
        /// Gets a copy of the list of Form IDs that are contained in this form collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<uint> ToFormIdList<T>(this IFormCollection<T> value) where T : IForm
        {
            // Copy the internal list of IDs in the form collection into a new list
            return ((FormCollectionProxy<T>)value).Items.Select(id => id).ToList();
        }

        private static T ProxyToField<T>(FieldProxy<T> proxy) where T : Field
        {
            return proxy == null ? null : proxy.Field;
        }

        /// <summary>
        /// Retreives the Condition field exposed by this proxy, or null if the proxy is null.
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static Condition ToField(this ICondition proxy)
        {
            return ProxyToField((ConditionProxy)proxy);
        }

        /// <summary>
        /// Retreives the Effect field exposed by this proxy, or null if the proxy is null.
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static Effect ToField(this IEffect proxy)
        {
            return ProxyToField((EffectProxy)proxy);
        }

        /// <summary>
        /// Retreives the MaterialData field exposed by this proxy, or null if the proxy is null.
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static Cobj.MaterialData ToField(this IMaterial proxy)
        {
            return ProxyToField((MaterialProxy)proxy);
        }

        /// <summary>
        /// Retreives the Script field exposed by this proxy, or null if the proxy is null.
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static VirtualMachineAdapter.Script ToField(this IScript proxy)
        {
            return ProxyToField((ScriptProxy)proxy);
        }
    }
}
