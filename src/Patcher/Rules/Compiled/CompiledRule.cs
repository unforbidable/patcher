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

using Patcher.Rules.Methods;
using Patcher.Data.Plugins;

namespace Patcher.Rules.Compiled
{
    sealed class CompiledRule : IRule
    {
        public FormKind From { get; set; }
        public WhereMethod Where { get; set; }
        public SelectMethod Select { get; set; }
        public UpdateMethod Update { get; set; }
        public InsertMethod[] Inserts { get; set; }

        public string WhereEditorId { get; set; }

        readonly RuleEngine engine;
        public RuleEngine Engine { get { return engine; } }

        readonly RuleMetadata metadata;
        public RuleMetadata Metadata { get { return metadata; } }

        public CompiledRule(RuleEngine engine, RuleMetadata metadata)
        {
            this.engine = engine;
            this.metadata = metadata;
        }

        public override string ToString()
        {
            return string.Format("{0}\\{1}@{2}", metadata.PluginFileName, metadata.RuleFileName, metadata.Name);
        }
    }
}
