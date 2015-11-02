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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Patcher.Data
{
    public sealed class FormRepository : IEnumerable<Form>
    {
        IList<Form> forms = new List<Form>();
        IDictionary<byte, IList<Form>> formsByPlugin = new SortedDictionary<byte, IList<Form>>();
        IDictionary<FormKind, IList<Form>> formsByKind = new SortedDictionary<FormKind, IList<Form>>();
        IDictionary<uint, Form> formsById = new SortedDictionary<uint, Form>();
        IDictionary<string, Form> formsByEditorId = new SortedDictionary<string, Form>(StringComparer.InvariantCultureIgnoreCase);

        IList<Form> formsWithEditorIdOverriden = new List<Form>();

        public Form this[uint formId] { get { return formsById[formId]; } }
        public Form this[string editorId] { get { return formsByEditorId[editorId]; } }

        public IEnumerable<Form> OfKind(FormKind kind)
        {
            return formsByKind.ContainsKey(kind) ? formsByKind[kind].Select(i => i) : Enumerable.Empty<Form>();
        }

        public IEnumerable<Form> OfPlugin(byte pluginNumber)
        {
            return formsByPlugin.ContainsKey(pluginNumber) ? formsByPlugin[pluginNumber].Select(i => i) : Enumerable.Empty<Form>();
        }

        public IEnumerable<Form> OfPlugin(Plugin plugin)
        {
            var pluginNumber = plugin.Context.Plugins.GetPluginNumber(plugin);
            return formsByPlugin.ContainsKey(pluginNumber) ? formsByPlugin[pluginNumber].Select(i => i) : Enumerable.Empty<Form>();
        }

        public IEnumerable<FormKind> GetFormKinds()
        {
            return formsByKind.Keys.Select(i => i);
        }

        internal void Add(Form form)
        {
            lock (forms)
            {
                if (form.PluginNumber > 0 && !formsById.ContainsKey(form.FormId))
                {
                    // Handle injecting when Form ID has not been used yet
                    // but the actual plugin number does not correspond to the plugin number 
                    // where this form belong to.
                    byte actualPluginNumber = (byte)(form.FormId >> 24);
                    if (actualPluginNumber > form.PluginNumber)
                    {
                        Log.Warning("Cannot inject forms into a plugin the number of which is greater than the number of the target plugin.");
                    }
                    else if (actualPluginNumber < form.PluginNumber)
                    {
                        // Injected forms are added to the target plugin
                        // as if they have always been there
                        // but the form record will never be loaded
                        // The actual form will override the injected form normally
                        var injected = new Form()
                        {
                            PluginNumber = actualPluginNumber,
                            FilePosition = -1,
                            Flags = FormFlags.Injected,
                            FormKind = form.FormKind,
                            FormId = form.FormId
                        };
                        Add(injected);
                        Log.Fine("Injected form {0}", injected);
                    }
                }

                forms.Add(form);

                if (!formsByPlugin.ContainsKey(form.PluginNumber))
                    formsByPlugin.Add(form.PluginNumber, new List<Form>());
                formsByPlugin[form.PluginNumber].Add(form);

                if (!formsByKind.ContainsKey(form.FormKind))
                    formsByKind.Add(form.FormKind, new List<Form>());
                formsByKind[form.FormKind].Add(form);

                if (formsById.ContainsKey(form.FormId))
                {
                    // Override previous form
                    var previous = formsById[form.FormId];
                    previous.Flags |= FormFlags.Overriden;
                    formsById.Remove(previous.FormId);
                    form.OverridesForm = previous;

                    // Do not unlink Editor ID of previous form
                    // It will be unlinked automatically if new form uses the same Editor ID (which it should)
                    // If overriding form has a different Editor ID both Editor IDs can be used to look up the overrdiding form
                }

                formsById.Add(form.FormId, form);

                // Link new form Editor ID
                if (form.EditorId != null)
                    LinkEditorId(form, form.EditorId);

                form.EditorIdChanged += OnEditorIdUpdated;
            }
        }

        internal void Remove(Form form)
        {
            lock (forms)
            {
                form.EditorIdChanged -= OnEditorIdUpdated;

                forms.Remove(form);

                formsByPlugin[form.PluginNumber].Remove(form);
                if (formsByPlugin[form.PluginNumber].Count == 0)
                    formsByPlugin.Remove(form.PluginNumber);

                formsByKind[form.FormKind].Remove(form);
                if (formsByKind[form.FormKind].Count == 0)
                    formsByKind.Remove(form.FormKind);

                formsById.Remove(form.FormId);

                // Unlink Editor ID (and link previous form using this Editor ID if available)
                if (form.EditorId != null)
                    UnlinkEditorId(form, form.EditorId);

                if (form.IsOverriding)
                {
                    // Restore overriden form
                    form.OverridesForm.Flags &= ~FormFlags.Overriden;
                    formsById.Add(form.OverridesForm.FormId, form.OverridesForm);

                    // Link Editor ID to the restored form
                    if (form.OverridesForm.EditorId != null)
                        LinkEditorId(form.OverridesForm, form.OverridesForm.EditorId);

                    form.OverridesForm = null;
                }
            }
        }

        private void OnEditorIdUpdated(Form form, string previousEditorId)
        {
            if (form.EditorId != previousEditorId)
            {
                lock (formsByEditorId)
                {
                    // Link new Editor ID
                    if (form.EditorId != null)
                    {
                        LinkEditorId(form, form.EditorId);
                    }

                    // Unlink previous Editor ID
                    if (previousEditorId != null)
                    {
                        UnlinkEditorId(form, previousEditorId);
                    }
                }
            }
        }

        private void LinkEditorId(Form form, string editorId)
        {
            if (form.IsOverriden)
            {
                // Do not link overriden forms
                // but place into the overriden EditorID list
                formsWithEditorIdOverriden.Add(form);
            }
            else
            {
                if (formsByEditorId.ContainsKey(editorId))
                {
                    // Editor ID already in use
                    var existingForm = formsByEditorId[editorId];

                    // Override the earlier reference
                    formsWithEditorIdOverriden.Add(existingForm);
                    formsByEditorId.Remove(editorId);

                    // Warn only when not overriding
                    if (existingForm.FormId != form.FormId)
                        Log.Warning("Form " + form + " uses EditorID that has already been used by form " + existingForm + ". The Editor ID will reference the most recently added form.");
                }

                formsByEditorId.Add(editorId, form);
            }
        }

        private void UnlinkEditorId(Form form, string editorId)
        {
            // If the form Editor ID has been overriden just remove from the list
            if (formsWithEditorIdOverriden.Contains(form))
            {
                // No need to release overriden reference
                formsWithEditorIdOverriden.Remove(form);
            }
            else
            {
                formsByEditorId.Remove(editorId);

                // Look for a form with overriden EditorID equal to the released Editor ID to reference
                foreach (var f in formsWithEditorIdOverriden.Reverse())
                {
                    if (f.EditorId == editorId)
                    {
                        // Register released EditorID to the form
                        formsWithEditorIdOverriden.Remove(f);
                        formsByEditorId.Add(editorId, f);
                        break;
                    }
                }
            }
        }

        public IEnumerable<uint> GetAllFormIds()
        {
            // Project to itself to prevent modification outside.
            return formsById.Keys.Select(i => i);
        }

        public bool Contains(uint formId)
        {
            return formsById.ContainsKey(formId);
        }

        public bool Contains(string editorId)
        {
            return formsByEditorId.ContainsKey(editorId);
        }

        public IQueryable<Form> AsQueryable()
        {
            return new QueryableFormRepository(this);
        }

        public IEnumerator<Form> GetEnumerator()
        {
            return forms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
