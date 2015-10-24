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

namespace Patcher.Rules
{
    public sealed class TagManager
    {
        readonly RuleEngine engine;

        IDictionary<string, ISet<uint>> tags = new SortedDictionary<string, ISet<uint>>(StringComparer.CurrentCultureIgnoreCase);

        // Used by engine
        internal TagManager(RuleEngine engine)
        {
            this.engine = engine;
        }

        public void Tag(string text, uint formId)
        {
            if (!tags.ContainsKey(text))
            {
                tags.Add(text, new HashSet<uint>() { formId });
            }
            else
            {
                tags[text].Add(formId);
            }
        }

        public void TagAll(string text, IEnumerable<uint> formIds)
        {
            if (!tags.ContainsKey(text))
            {
                tags.Add(text, new HashSet<uint>(formIds));
            }
            else
            {
                tags[text] = new HashSet<uint>(tags[text].Concat(formIds));
            }
        }

        public bool HasTag(string text, uint formId)
        {
            return tags.ContainsKey(text) && tags[text].Contains(formId);
        }

        public IEnumerable<uint> AllHavingTag(string text)
        {
            // Return list as IEnumerable
            return tags.ContainsKey(text) ? tags[text].Select(i => i) : Enumerable.Empty<uint>();
        }

        internal void Clear()
        {
            tags.Clear();
        }
    }
}
