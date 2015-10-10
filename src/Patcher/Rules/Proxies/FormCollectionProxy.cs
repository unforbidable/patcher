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

using Patcher.Rules.Compiled.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Patcher.Data.Plugins;

namespace Patcher.Rules.Proxies
{
    public sealed class FormCollectionProxy<TForm> : Proxy, IFormCollection<TForm> where TForm : IForm
    {
        // Assign the IList<uint> that backs this form collection 
        // or a different type such as Collection<uint> or IEnumerable<uint> if the collection is not backed in DataContext
        IEnumerable<uint> items = Enumerable.Empty<uint>();
        internal IEnumerable<uint> Items
        {
            get { return items; }
            set
            {
                //ICollection<uint> asList = value as ICollection<uint>;
                //if (asList != null)
                //    Log.Fine("Assigned list backed collection of forms of type {0}, count={1}", typeof(TForm).Name, asList.Count);
                //else
                //    Log.Fine("Assigned virtual collection of forms of type {0}, count={1}", typeof(TForm).Name, "?");

                items = value;
            }
        }

        public void Add(string editorId)
        {
            if (editorId == null)
                throw new ArgumentNullException("item", "Editor ID cannot be null.");

            if (!Provider.Engine.Context.Forms.Contains(editorId))
            {
                Log.Warning("Form with Editor ID '{0}' not found - cannot be added.", editorId);
            }
            else
            {
                var form = Provider.Engine.Context.Forms[editorId];
                if (!Items.Contains(form.FormId))
                {
                    EnsureProperKindOfFormAdded(form);
                    DoAdd(form.FormId);
                }
                else
                {
                    Log.Warning("Form {0} already added.", form);
                }
            }
        }

        public void Add(IForm formProxy)
        {
            if (formProxy == null)
                throw new ArgumentNullException("item", "Cannot add null to form collection.");

            if (!Items.Contains(formProxy.FormId))
            {
                EnsureProperKindOfFormAdded(((FormProxy)formProxy).Form);
                DoAdd(formProxy.FormId);
            }
            else
            {
                Log.Warning("Form {0} already added.", ((FormProxy)formProxy).Form);
            }
        }

        public void Add(uint formId)
        {
            if (formId == 0)
            {
                Log.Warning("Form with Form ID 0 cannot be added.");
            }
            else if (!Items.Contains(formId))
            {
                var form = Provider.Engine.Context.Forms[formId];
                EnsureProperKindOfFormAdded(form);
                DoAdd(formId);
            }
            else
            {
                Log.Warning("Form with Form ID {0} already added.", formId);
            }
        }

        private void EnsureProperKindOfFormAdded(Form form)
        {
            // Ensure form is proper kind
            // If the type of collection is TForm, any form can be added
            if (typeof(TForm) != typeof(IForm))
            {
                if (form == null)
                {
                    // Form ID has not been resolved and type is unknown, warning
                    Log.Warning("An unresolved form kind of which is unknown is being added to a form collection that may contain froms a specific kind only.");
                }
                else if (!typeof(TForm).IsAssignableFrom(Provider.GetInterface(form.FormKind)))
                {
                    throw new InvalidOperationException("Cannot add form " + form + " to form collection " + this + ".");
                }
            }
        }

        private void DoAdd(uint formId)
        {
            // Try cast to list
            IList<uint> asList = Items as IList<uint>;
            if (asList != null)
            {
                // Proxy mode matters only if the list is backed
                EnsureWritable();

                // When backed by a list, add Form ID to the list
                asList.Add(formId);
            }
            else
            {
                // If not backed, replace IEnumerable
                // This is notvery effective if many forms where to be are added to (or removed from) an enumerable like this
                // If that were the case it would be more effective to back the enumeration temporarily into a Collection<uint>
                Items = Items.Concat(new uint[] { formId });
            }
        }

        public void Remove(string editorId)
        {
            if (editorId == null)
                throw new ArgumentNullException("item", "Editor ID cannot be null.");

            if (!Provider.Engine.Context.Forms.Contains(editorId))
            {
                Log.Warning("Form with Editor ID '{0}' not found - cannot be removed.", editorId);
            }
            else
            {
                DoRemove(Provider.Engine.Context.Forms[editorId].FormId);
            }
        }

        public void Remove(IForm form)
        {
            if (form == null)
                throw new ArgumentNullException("item", "Cannot remove null from form collection.");

            if (Items.Contains(form.FormId))
            {
                DoRemove(form.FormId);
            }
            else
            {
                Log.Warning("Form {0} not in the list - cannot be removed.", form);
            }
        }

        public void Remove(uint formId)
        {
            if (formId == 0)
            {
                Log.Warning("Form with Form ID 0 cannot be removed.");
            }
            else if (Items.Contains(formId))
            {
                DoRemove(formId);
            }
            else
            {
                Log.Warning("Form with Form ID {0} not on the list - cannot be removed.", formId);
            }
        }

        private void DoRemove(uint formId)
        {
            // Try cast to list
            IList<uint> asList = Items as IList<uint>;
            if (asList != null)
            {
                // Proxy mode matters only if the list is backed
                EnsureWritable();

                // When backed by a list, add Form ID to the list
                asList.Remove(formId);
            }
            else
            {
                // If not backed, replace IEnumerable
                // This is notvery effective if many forms where to be are added to (or removed from) an enumerable like this
                // If that were the case it would be more effective to back the enumeration temporarily into a Collection<uint>
                Items = Items.Where(i => i != formId);
            }
        }

        public bool Contains(string editorId)
        {
            if (editorId == null)
                throw new ArgumentNullException("item", "Editor ID cannot be null.");

            if (!Provider.Engine.Context.Forms.Contains(editorId))
            {
                Log.Warning("Form with Editor ID '{0}' not found - method Contains() returns false.");
                return false;
            }

            return Items.Contains(Provider.Engine.Context.Forms[editorId].FormId);
        }

        public bool Contains(IForm form)
        {
            if (form == null)
                throw new ArgumentNullException("item", "Form cannot be null.");

            return Items.Contains(form.FormId);
        }

        public bool Contains(uint formId)
        {
            return Items.Contains(formId);
        }

        public int Count { get { return Items.Count(); } }

        public IEnumerator<TForm> GetEnumerator()
        {
            IEnumerable<uint> willEnumerate;

            // Try cast to list
            IList<uint> asList = Items as IList<uint>;
            if (asList != null)
            {
                // Iterate copy so that the backing list can be modified during the iteration
                willEnumerate = Items.Select(i => i);
            }
            else
            {
                // Not backed by a list, enumerate as is
                willEnumerate = Items;
            }

            // Enumerate as if a collection of Proxies of type T
            return willEnumerate.Select(f => Provider.CreateFormProxy<TForm>(Provider.Engine.Context.Forms[f], Mode)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
             return GetEnumerator();
        }

        public IFormCollection<TOther> Of<TOther>() where TOther : IForm
        {
            FormKind kind = Provider.GetFormKindOfInterface(typeof(TOther));
            var items = Items.Where(i => Provider.Engine.Context.Forms[i].FormKind == kind);
            return Provider.CreateFormCollectionProxy<TOther>(Mode, items);
        }

        /// <summary>
        /// Creates a new form collection containing only froms from the current form collection that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IFormCollection<TForm> Where(Predicate<TForm> predicate)
        {
            // Reusing a single proxy for the predicate
            var kind = Provider.GetFormKindOfInterface(typeof(TForm));
            var proxy = Provider.CreateFormProxy<FormProxy>(kind, Mode);
            var items = Items.Where(i => predicate.Invoke((TForm)(IForm)proxy.WithForm(Provider.Engine.Context.Forms[i])));
            return Provider.CreateFormCollectionProxy<TForm>(Mode, items);
        }

        public void TagAll(string text)
        {
            Provider.Engine.Tags.TagAll(text, items);
        }

        public IFormCollection<TForm> HavingTag(string text)
        {
            // Create new collection that is an intersection of the previous one and all forms tagged by the text
            var tagged = Provider.Engine.Tags.AllHavingTag(text);
            return Provider.CreateFormCollectionProxy<TForm>(Mode, items.Intersect(tagged));
        }

        public override string ToString()
        {
            return string.Format("{0} Form Collection, items={1}, backed={2}", 
                typeof(TForm) == typeof(IForm) ? "Generic" : Provider.GetFormKindOfInterface(typeof(TForm)), 
                items.Count(), items is IList<uint> ? "yes" : "no");
        }
    }
}
