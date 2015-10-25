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

namespace Patcher.Rules.Compiled.Helpers
{
    class HelperProvider
    {
        readonly RuleEngine engine;

        List<HelperInfo> helpers = new List<HelperInfo>();

        public IEnumerable<HelperInfo> Helpers { get { return helpers.Select(h => h); } }

        public HelperProvider(RuleEngine engine)
        {
            this.engine = engine;

            string thisNamespace = GetType().Namespace;
            string gameNamespace = string.Format("{0}.{1}", thisNamespace, engine.Context.GameTitle);

            var types = GetType().Assembly.GetTypes()
                .Where(t => t.Namespace == thisNamespace || t.Namespace == gameNamespace)
                .Where(t => t.GetCustomAttributes(typeof(HelperAttribute), false).Length > 0);

            foreach (var type in types)
            {
                helpers.Add(new HelperInfo(this, type));
            }
        }
    }
}
