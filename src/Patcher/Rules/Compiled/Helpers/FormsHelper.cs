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

using Patcher.Data;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    [Helper("Forms", typeof(IFormsHelper))]
    sealed class FormsHelper : IFormsHelper
    {
        readonly CompiledRuleContext context;

        public FormsHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public IForm Find(uint formId)
        {
            // The main plugin only
            uint actualFormId = formId & 0xffffff;
            if (!context.Rule.Engine.Context.Forms.Contains(actualFormId))
            {
                Log.Warning("Form with Form ID {0:X8} not found in the main plugin.", formId);

                // Warning is issued but an unresovled reference form proxy will be created
                //return null;
            }

            return context.Rule.Engine.ProxyProvider.CreateFormProxy(actualFormId, ProxyMode.Discovered);
        }

        public IForm Find(string plugin, uint formId)
        {
            byte number = context.Rule.Engine.Context.Plugins.GetPluginNumber(plugin);
            if (number == PluginIndex.InvalidPluginNumber)
            {
                Log.Warning("Plugin file name {0} not found.", plugin);
                return null;
            }

            uint actualFormId = (((uint)number) << 24) + (formId & 0xffffff);
            if (!context.Rule.Engine.Context.Forms.Contains(actualFormId))
            {
                Log.Warning("Form with Form ID {0} not found in plugin {1}.", formId, plugin);
                return null;
            }

            return context.Rule.Engine.ProxyProvider.CreateFormProxy(actualFormId, ProxyMode.Discovered);
        }

        public IForm Find(string editorId)
        {
            if (!context.Rule.Engine.Context.Forms.Contains(editorId))
            {
                Log.Warning("Form with Editor ID {0} not found.", editorId);
                return null;
            }
            return context.Rule.Engine.ProxyProvider.CreateFormProxy(context.Rule.Engine.Context.Forms[editorId], ProxyMode.Discovered);
        }

        public IFormCollection<IForm> FindAll()
        {
            var items = context.Rule.Engine.Context.Forms.GetAllFormIds();
            return context.Rule.Engine.ProxyProvider.CreateFormCollectionProxy<IForm>(ProxyMode.Discovered, items);
        }

        public IFormCollection<IForm> FindAllHavingTag(string text)
        {
            var items = context.Rule.Engine.Tags.AllHavingTag(text);
            return context.Rule.Engine.ProxyProvider.CreateFormCollectionProxy<IForm>(ProxyMode.Discovered, items);
        }
    }
}
