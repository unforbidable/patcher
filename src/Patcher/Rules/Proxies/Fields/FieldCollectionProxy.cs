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
using System.Collections;
using Patcher.Data.Plugins.Content;

namespace Patcher.Rules.Proxies.Fields
{
    public abstract class FieldCollectionProxy<TField> : Proxy where TField : Field
    {
        internal List<TField> Fields { get; set; }

        internal List<TField> CopyFieldCollection()
        {
            // If the list of fields is null, return null
            if (Fields == null)
                return null;
            else
                return Fields.Select(f => f.CopyField()).Cast<TField>().ToList();
        }

        protected IEnumerator<TInterface> GetFieldEnumerator<TInterface>()
        {
            // Empty enumerator for empty collections
            if (Fields.Count == 0)
                return Enumerable.Empty<TInterface>().GetEnumerator();

            // Enumerate shallow copy so that the list of fields can be modified during an iteration
            return Fields.Select(f => f).ToArray().Select(f => Provider.CreateFieldProxy(f, Mode)).Cast<TInterface>().GetEnumerator();
        }

        protected TField ProxyToField(object item)
        {
            var proxy = item as FieldProxy<TField>;
            if (proxy == null)
                throw new InvalidOperationException("Field interface implementation " + item.GetType().FullName + " does not match the expected field proxy implementation " + typeof(TField).FullName + ".");

            return proxy.Field;
        }
    }
}
