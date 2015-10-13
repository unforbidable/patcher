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

using Patcher.Rules.Compiled.Forms;
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
        /// Gets the list of Form IDs that are contained in this form collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<uint> ToFormIdList<T>(this IFormCollection<T> value) where T : IForm
        {
            // Copy the internal list of IDs in the form collection into a new list
            return ((FormCollectionProxy<T>)value).Items.Select(id => id).ToList();
        }
    }
}
