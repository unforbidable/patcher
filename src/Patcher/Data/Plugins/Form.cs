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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins
{
    public sealed class Form
    {
        public byte PluginNumber { get; set; }
        public long FilePosition { get; set; }
        public uint FormId { get; set; }
        public uint ParentFormId { get; set; }
        public Form OverridesForm { get; set; }
        public FormKind FormKind { get; set; }

        GenericFormRecord record;
        public GenericFormRecord Record
        {
            get { return record; }
            set
            {
                // Cannot assign another record once one is assigned
                // This restrictions makes keeping Editor ID index up to date a bit easier
                if (record != null)
                    throw new InvalidOperationException("Cannot change form record once assigned.");

                // No effect when value is null
                if (value != null)
                {
                    record = value;

                    // Subscribe to record events for forwarding EditorIdChanged
                    SubscribeToRecordEvents(value);
                }
            }
        }

        internal event Action<Form, string> EditorIdChanged;

        FormFlags flags;
        internal FormFlags Flags { get { return flags; } set { flags = value; } }

        public bool IsHardcoded { get { return flags.HasFlag(FormFlags.Hardcoded); } }
        public bool IsInjected { get { return flags.HasFlag(FormFlags.Injected); } }
        public bool IsOverriden { get { return flags.HasFlag(FormFlags.Overriden); } }
        public bool IsOverriding { get { return OverridesForm != null; } }
        public bool IsLazyLoaded { get { return flags.HasFlag(FormFlags.LazyLoaded); } }
        public bool IsDummyRecord { get { return Record != null && Record.GetType() == typeof(DummyRecord); } }
        public bool IsLoaded { get { return Record != null && Record.GetType() != typeof(DummyRecord); } }

        public string EditorId { get { return Record != null ? Record.EditorId : null; } }

        /// <summary>
        /// Creates a deep copy of the current form, optionally resetting the FormId so that it is treated as a new form.
        /// </summary>
        /// <param name="asNew">Indicates whether the copy will be a new form or an override.</param>
        /// <returns>Returns a deep copy of the current form.</returns>
        public Form CopyForm(bool asNew)
        {
            return new Form()
            {
                PluginNumber = PluginIndex.InvalidPluginNumber,
                FilePosition = -1,
                FormId = asNew ? 0 : FormId,
                ParentFormId = ParentFormId,
                OverridesForm = null,
                FormKind = FormKind,
                Record = Record.CopyRecord()
            };
        }

        public override string ToString()
        {
            return string.Format("{0} {1:X8}{2}", FormKind, FormId, EditorId != null ? string.Format(" '{0}'", EditorId) : string.Empty);
        }

        public const string NullFormString = "NULL 00000000";

        public static string GetUnresolvedFormString(uint formId)
        {
            return string.Format("UNRESOLVED {0:X8}", formId);
        }

        private void SubscribeToRecordEvents(GenericFormRecord record)
        {
            record.EditorIdChanged += ForwardEditorIdChangedEvent;
        }

        private void ForwardEditorIdChangedEvent(string previousEditorId)
        {
            var handler = EditorIdChanged;
            if (handler != null)
            {
                handler.Invoke(this, previousEditorId);
            }
        }

        public IEnumerable<uint> GetReferencedFormIds()
        {
            if (record == null)
                return Enumerable.Empty<uint>();
            else
                return record.GetReferencedFormIds();
        }
    }
}
