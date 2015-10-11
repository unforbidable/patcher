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

                    // Raise loaded event and also EditorIdChanged event
                    RaiseLoadedEvent();
                    RaiseEditorIdChangedEvent(value.EditorId);

                    // Subscribe to record events for forwarding EditorIdChanging and EditorIdChanged
                    SubscribeToRecordEvents(value);
                }
            }
        }

        internal event EventHandler<EventArgs> Loaded;
        internal event EventHandler<EditorIdChangingEventArgs> EditorIdChanging;
        internal event EventHandler<EditorIdChangedEventArgs> EditorIdChanged;

        FormFlags flags;
        internal FormFlags Flags { get { return flags; } set { flags = value; } }

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
            return string.Format("{0} [0x{1:X8}]{2}", FormKind, FormId, EditorId != null ? string.Format(" '{0}'", EditorId) : string.Empty);
        }

        private void RaiseLoadedEvent()
        {
            var handler = Loaded;
            if (handler != null)
            {
                var args = EventArgs.Empty;
                handler.Invoke(this, args);
            }
        }

        private void SubscribeToRecordEvents(GenericFormRecord record)
        {
            record.EditorIdChanging += ForwardEditorIdChangingEvent;
            record.EditorIdChanged += ForwardEditorIdChangedEvent;
        }

        private void ForwardEditorIdChangingEvent(object sender, EditorIdChangingEventArgs args)
        {
            var handler = EditorIdChanging;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        private void ForwardEditorIdChangedEvent(object sender, EditorIdChangedEventArgs args)
        {
            var handler = EditorIdChanged;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        private void RaiseEditorIdChangedEvent(string newEditorId)
        {
            var handler = EditorIdChanged;
            if (handler != null)
            {
                var args = new EditorIdChangedEventArgs(null, newEditorId);
                handler.Invoke(this, args);
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
