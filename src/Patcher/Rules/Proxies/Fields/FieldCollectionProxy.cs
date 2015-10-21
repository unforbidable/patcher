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
        protected abstract List<TField> Fields { get; }

        internal List<TField> CopyFieldCollection()
        {
            // If the list of fields is null, return null
            if (Fields == null)
                return null;
            else
                return Fields.Select(f => f.CopyField()).Cast<TField>().ToList();
        }

        protected int GetFieldCount()
        {
            // If the list of fields is null, return 0
            if (Fields == null)
                return 0;
            else
                return Fields.Count;
        }

        protected void ClearFields()
        {
            if (Fields == null)
                Fields.Clear();
        }

        protected void AddField(TField field, bool copy)
        {
            if (field == null)
                throw new ArgumentNullException("item", "Cannot add NULL to the collection.");

            if (Fields == null)
            {
                throw new InvalidOperationException("Backing collection of fields does not exist.");
            }

            if (copy)
            {
                Fields.Add((TField)field.CopyField());
            }
            else
            {
                Fields.Add(field);
            }
        }

        protected void RemoveField(TField field)
        {
            if (field == null)
                throw new ArgumentNullException("item", "Cannot remove NULL to the collection.");

            if (Fields != null)
            {
                if (!Fields.Remove(field))
                {
                    Log.Warning("Item {0} was not found in the collection and could not be removed.", field);
                }
            }
        }

        protected IEnumerator<TInterface> GetFieldEnumerator<TInterface>()
        {
            // Empty enumerator for empty collections
            if (Fields == null || Fields.Count == 0)
                return Enumerable.Empty<TInterface>().GetEnumerator();

            // Enumerate shallow copy so that the list of fields can be modified during an iteration
            return Fields.Select(f => f).ToArray().Select(f => Provider.CreateFieldProxy(f, Mode)).Cast<TInterface>().GetEnumerator();
        }

        protected TInterface GetField<TInterface>(Func<TField, bool> predicate)
        {
            var field = Fields.Where(predicate).FirstOrDefault();
            if (field == null)
            {
                Log.Warning("Field {0} not found in collection.", predicate.ToString());
                return default(TInterface);
            }
            else
            {
                var proxy = Provider.CreateFieldProxy(field, Mode);
                return (TInterface)(object)proxy;
            }
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
