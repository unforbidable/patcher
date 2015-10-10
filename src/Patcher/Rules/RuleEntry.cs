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
    sealed class RuleEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RuleEntryFrom From { get; set; }
        public RuleEntryWhere Where { get; set; }
        public RuleEntrySelect Select { get; set; }
        public RuleEntryUpdate Update { get; set; }
        public IEnumerable<RuleEntryInsert> Inserts { get; set; }

        public sealed class RuleEntryFrom
        {
            public string FormKind { get; set; }
        }

        public sealed class RuleEntryWhere
        {
            public string Code { get; set; }
        }

        public sealed class RuleEntrySelect
        {
            public string Code { get; set; }
        }

        public sealed class RuleEntryUpdate
        {
            public string Code { get; set; }
        }

        public sealed class RuleEntryInsert
        {
            public string InsertedFormKind { get; set; }
            public string Code { get; set; }
        }
    }
}
