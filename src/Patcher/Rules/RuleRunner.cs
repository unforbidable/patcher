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
using Patcher.Rules.Proxies;
using Patcher.Rules.Proxies.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules
{
    sealed class RuleRunner
    {
        readonly RuleEngine engine;
        readonly IRule rule;
        
        IList<Form> result = new List<Form>();
        public IList<Form> Result { get { return result; } }

        public int Created { get; private set; }
        public int Updated { get; private set; }

        public RuleRunner(RuleEngine engine, IRule rule)
        {
            this.engine = engine;
            this.rule = rule;
        }

        public void Run()
        {
            // No source form
            if (rule.From == FormKind.None)
            {
                Log.Fine("Non-query rule without source form");

                // Only select and inserts are valid in a non-query rule
                DoSelect(null);
                DoInserts(null);
            }
            // Single source form
            else if (rule.WhereEditorId != null)
            {
                Log.Fine("Looking up form with Editor ID = '" + rule.WhereEditorId + "'");

                if (engine.Context.Forms.Contains(rule.WhereEditorId))
                {
                    // Single form that is linked to the specified Editor ID
                    // Make sure the FormType matches and the form has been loaded
                    var form = engine.Context.Forms[rule.WhereEditorId];
                    if (form.FormKind == rule.From && form.IsLoaded)
                    {
                        var sourceProxy = engine.ProxyProvider.CreateFormProxy(form, ProxyMode.Source);
                        var updateProxy = engine.ProxyProvider.CreateFormProxy(rule.From, ProxyMode.Target);
                        DoActions(sourceProxy, updateProxy);
                    }
                    else
                    {
                        Log.Fine("Form with Editor ID = '" + rule.WhereEditorId + "' found (" + form + ") but it is a different type or not loaded");
                    }
                }
                else
                {
                    Log.Fine("Form with Editor ID = '" + rule.WhereEditorId + "' not found");
                }
            }
            // List of source forms
            else
            {
                Log.Fine("Querying all {0} forms", rule.From);
                var forms = engine.Context.Forms.OfKind(rule.From).Where(f => f.IsLoaded && !f.IsOverriden).ToArray();
                Log.Fine("Found {0} forms of type {1}", forms.Length, rule.From);

                // Prepare readonly source proxy
                var sourceProxy = engine.ProxyProvider.CreateFormProxy(rule.From, ProxyMode.Source);
                // Prepare update proxy (same type as the source proxy)
                var updateProxy = engine.ProxyProvider.CreateFormProxy(rule.From, ProxyMode.Target);

                // All loaded forms of specified type
                foreach (var form in forms)
                {
                    // Assign each form to the reusable readonly source proxy
                    sourceProxy.WithForm(form);

                    // Apply condition to all forms if specified
                    if (rule.Where == null || rule.Where.Method(sourceProxy))
                    {
                        // Perform all available actions
                        DoActions(sourceProxy, updateProxy);
                    }
                }
            }
        }

        private void DoActions(FormProxy sourceProxy, FormProxy updateProxy)
        {
            DoSelect(sourceProxy);
            DoUpdate(sourceProxy, updateProxy);
            DoInserts(sourceProxy);
        }

        private void DoUpdate(FormProxy sourceProxy, FormProxy updateProxy)
        {
            if (rule.Update != null)
            {
                // Copy of the source form will be chnaged
                var targetForm = sourceProxy.Form.CopyForm(false);
                updateProxy.WithForm(targetForm);
                if (rule.Update.Method(sourceProxy, updateProxy))
                {
                    result.Add(targetForm);
                    Updated++;
                }
            }
        }

        private void DoSelect(FormProxy sourceProxy)
        {
            if (rule.Select != null)
            {
                // Select makes no changes to the form
                rule.Select.Method(sourceProxy);
            }
        }

        private void DoInserts(FormProxy sourceProxy)
        {
            if (rule.Inserts != null)
            {
                foreach (var insert in rule.Inserts)
                {
                    Form targetForm;
                    if (insert.Copy && sourceProxy != null && sourceProxy.Form.FormKind == insert.InsertedFormKind)
                    {
                        // Copy form data if  source is the same as target (and source is not null)
                        targetForm = sourceProxy.Form.CopyForm(true);
                    }
                    else
                    {
                        // Create new otherwise
                        targetForm = engine.Context.CreateForm(insert.InsertedFormKind);
                    }

                    var targetProxy = engine.ProxyProvider.CreateFormProxy(targetForm, ProxyMode.Target);
                    if (insert.Method(sourceProxy, targetProxy))
                    {
                        result.Add(targetForm);
                        Created++;
                    }
                }
            }
        }

    }
}
