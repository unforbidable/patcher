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

using Patcher.Data.Plugins;
using Patcher.Data.Plugins.Content;
using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Rules.Proxies.Forms
{
    public abstract class FormProxy : Proxy, IForm
    {
        uint formId;
        Form form;

        internal Form Form { get { return form; } }

        internal FormProxy WithForm(uint formId)
        {
            if (GetType() != typeof(DummyFormProxy))
            {
                throw new InvalidOperationException("Only a dummy form proxy can be assigned with a Form ID directly. Proxy type is " + GetType().Name + ".");
            }

            this.formId = formId;
            form = null;
            WithRecord(null);
            return this;
        }

        internal FormProxy WithForm(Form form)
        {
            this.form = form;
            if (form != null)
            {
                formId = form.FormId;
                WithRecord(form.Record);
            }
            else
            {
                formId = 0;
                WithRecord(null);
            }
            return this;
        }

        public abstract string EditorId { get; set; }

        protected abstract void WithRecord(GenericFormRecord record);

        public uint FormId { get { EnsureFormCreated(); return formId; } }

        private void EnsureFormCreated()
        {
            if (GetType() != typeof(DummyFormProxy) && formId == 0)
            {
                throw new InvalidOperationException("Cannot retrieve Form ID of " + this + " because no Form ID has been assigned to the form yet.");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", form != null ? form.ToString() : string.Format("{0} [0x{1:X8}]", formId != 0 ? "UNRESOLVED" : "NULL", formId));
        }

        public bool HasTag(string text)
        {
            return Provider.Engine.Tags.HasTag(text, formId);
        }

        public void Tag(string text)
        {
            Provider.Engine.Tags.Tag(text, formId);
        }
    }

    public class FormProxy<T> : FormProxy where T : GenericFormRecord
    {
        protected T record;

        public sealed override string EditorId { get { EnsureEditorIdReadable(); return record.EditorId; } set { EnsureEditorIdWritable(); record.EditorId = value; } }

        protected sealed override void WithRecord(GenericFormRecord record)
        {
            if (record != null && !(record is T))
                throw new ArgumentException("Record " + record.GetType().FullName + " is not compatible with proxy " + GetType().FullName);

            this.record = (T)record;
        }

        private void EnsureEditorIdWritable()
        {
            // Validate Proxy mode writable first to get an appropriate message
            EnsureWritable();

            // Validate readibility (access) first
            EnsureEditorIdReadable();

            // Unsupported form, not fully loaded
            if (record.GetType() == typeof(DummyRecord))
                throw new InvalidOperationException("Cannot edit properties of form " + this + " of type that is not supported. Depending on the kind of form, only Editor ID may be accessed, but not modified.");
        }

        private void EnsureEditorIdReadable()
        {
            // Validate Proxy mode readibility first to get an appropriate message
            EnsureReadable();

            // Check only if dummy form proxy
            if (GetType() == typeof(DummyFormProxy))
            {
                // Note Target form during an insert operation (new form creation) will have form ID 0
                // but it will not be a dummy form proxy
                if (FormId == 0)
                {
                    // Null form proxy
                    throw new InvalidOperationException("Cannot access properties of a null form " + this + ".");
                }
                else if (Form == null)
                {
                    // Not respolved Form ID
                    throw new InvalidOperationException("Cannot access properties of an uresolved form " + this + ".");
                }
                else if (record == null)
                {
                    // Not loaded form proxy
                    throw new InvalidOperationException("Cannot access properties of form " + this + " which has not loaded.");
                }
            }
        }
    }
}
