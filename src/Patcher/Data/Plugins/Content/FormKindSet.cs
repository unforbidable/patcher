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

namespace Patcher.Data.Plugins.Content
{
    /// <summary>
    /// Represents a set of form kinds and is used to specify the set of form kids that are allowed in a reference.
    /// </summary>
    public sealed class FormKindSet
    {
        static IDictionary<string, FormKindSet> cache = new SortedDictionary<string, FormKindSet>();

        IEnumerable<FormKind> kinds;

        public static FormKindSet FromNames(string names)
        {
            if (string.IsNullOrEmpty(names))
                return Any;

            lock (cache)
            {
                // Find predefined ro cached set
                if (cache.ContainsKey(names))
                    return cache[names];

                // Create new one and add to cache
                var set = new FormKindSet(names);
                cache.Add(names, set);
                return set;
            }
        }

        public bool IsAny { get { return !kinds.Any(); } }

        private FormKindSet(string names)
        {
            if (string.IsNullOrEmpty(names))
            {
                // Empty set if null or empty
                kinds = Enumerable.Empty<FormKind>();
            }
            else
            {
                if (names.Length % 4 != 0)
                    throw new ArgumentException("The input length must be a multiple of four.");

                if (names.Length == 4)
                {
                    // Single element array if single kind
                    kinds = new FormKind[] { FormKind.FromName(names) };
                }
                else
                {
                    // Sorted set if multiple kinds, also sort names to prevent duplicates
                    var split = Enumerable.Range(0, names.Length / 4).Select(i => names.Substring(i * 4, 4)).OrderBy(n => n);
                    kinds = new SortedSet<FormKind>(split.Select(s => FormKind.FromName(s)));
                }
            }
        }

        public bool Contains(FormKind kind)
        {
            return kinds.Contains(kind);
        }

        public override string ToString()
        {
            if (IsAny)
                return "(Any)";
            else
                return string.Join("|", kinds);
        }

        // Each set is ummutable so we can offer reusable predefiner sets
        // And cache any newly created
        public static readonly FormKindSet Any = new FormKindSet(null);
        public static readonly FormKindSet CellOnly = FromNames(Names.CELL);
        public static readonly FormKindSet ClasOnly = FromNames(Names.CLAS);
        public static readonly FormKindSet FactOnly = FromNames(Names.FACT);
        public static readonly FormKindSet FurnOnly = FromNames(Names.FURN);
        public static readonly FormKindSet GlobOnly = FromNames(Names.GLOB);
        public static readonly FormKindSet KywdOnly = FromNames(Names.KYWD);
        public static readonly FormKindSet LctnOnly = FromNames(Names.LCTN);
        public static readonly FormKindSet NpcOnly = FromNames(Names.NPC_);
        public static readonly FormKindSet PackOnly = FromNames(Names.PACK);
        public static readonly FormKindSet PerkOnly = FromNames(Names.PERK);
        public static readonly FormKindSet QustOnly = FromNames(Names.QUST);
        public static readonly FormKindSet RaceOnly = FromNames(Names.RACE);
        public static readonly FormKindSet RegnOnly = FromNames(Names.REGN);
        public static readonly FormKindSet SndrOnly = FromNames(Names.SNDR);
        public static readonly FormKindSet WthrOnly = FromNames(Names.WTHR);

        // More multiple kind sets will be cached too
        public static readonly FormKindSet ActorReferences = FromNames(Names.ACHR + Names.PLYR + Names.REFR);
        public static readonly FormKindSet Effects = FromNames(Names.ALCH + Names.ENCH + Names.INGR + Names.SCRL + Names.SPEL);
        public static readonly FormKindSet References = FromNames(Names.ACHR + Names.PARW + Names.PBEA + Names.PCON + Names.PFLA + Names.PGRE + 
            Names.PHZD + Names.PLYR + Names.PMIS + Names.REFR);
        public static readonly FormKindSet ReferencableObjects = FromNames(Names.ACTI + Names.ALCH + Names.AMMO + Names.ARMA + Names.ARMO + Names.ASPC +
            Names.BOOK + Names.CONT + Names.DOOR + Names.ENCH + Names.FLST + Names.FURN + Names.GRAS + Names.IDLM + Names.KEYM + Names.LIGH + Names.LVLI +
            Names.LVSP + Names.MISC + Names.MSTT + Names.NPC_ + Names.PROJ + Names.SCRL + Names.SHOU + Names.SLGM + Names.SOUN + Names.SPEL + Names.STAT +
            Names.TACT + Names.TREE + Names.WEAP);
        public static readonly FormKindSet InventoryObjects = FromNames(Names.ALCH + Names.AMMO + Names.ARMO + Names.BOOK + Names.COBJ + Names.FLST +
            Names.INGR + Names.KEYM + Names.LIGH + Names.LVLI + Names.MISC + Names.SCRL + Names.SLGM + Names.WEAP);
    }
}
