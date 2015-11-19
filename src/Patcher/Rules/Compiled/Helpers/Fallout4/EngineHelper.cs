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
using Patcher.Rules.Compiled.Fields.Fallout4;
using Patcher.Rules.Compiled.Forms;
using Patcher.Data.Plugins.Content.Fields.Fallout4;
using Patcher.Rules.Proxies;
using Patcher.Data.Plugins.Content.Records.Fallout4;
using Patcher.Rules.Compiled.Forms.Skyrim;

namespace Patcher.Rules.Compiled.Helpers.Fallout4
{
    [Helper("Engine", typeof(IEngineHelper))]
    sealed class EngineHelper : IEngineHelper
    {
        readonly CompiledRuleContext context;

        public EngineHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public float GetParam(string name, float defaultValue)
        {
            return context.Rule.Engine.GetParam(GetParamName(name), defaultValue);
        }

        public int GetParam(string name, int defaultValue)
        {
            return context.Rule.Engine.GetParam(GetParamName(name), defaultValue);
        }

        public string GetParam(string name, string defaultValue)
        {
            return context.Rule.Engine.GetParam(GetParamName(name), defaultValue);
        }

        private string GetParamName(string name)
        {
            return string.Format("{0}:{1}", context.Rule.Metadata.PluginFileName, name);
        }
    }
}
