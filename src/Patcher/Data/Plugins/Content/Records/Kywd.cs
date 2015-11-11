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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records
{
    [Record(Names.KYWD)]
    [Game(Games.Skyrim)]
    [Game(Games.Fallout4)]
    public sealed class Kywd : GenericFormRecord, IColorFloatAdaptable
    {
        [Member(Names.CNAM)]
        private uint RawColor { get; set; }

        [Member(Names.TNAM)]
        [Game(Games.Fallout4)]
        public int? Unknown { get; set; }

        [Member(Names.NNAM)]
        [Game(Games.Fallout4)]
        public string ShortName { get; set; }

        [Member(Names.FULL)]
        [Game(Games.Fallout4)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.DNAM)]
        [Game(Games.Fallout4)]
        public string Description { get; set; }

        [Member(Names.DATA)]
        [Game(Games.Fallout4)]
        public uint Data { get; set; }

        private ColorAdapter color = null;
        public ColorAdapter Color
        {
            get
            {
                if (color == null)
                    color = new ColorAdapter(this);
                return color;
            }
        }

        public float Red { get { return (RawColor & 0xFF) / 255f; } set { RawColor = RawColor & 0xFFFFFF00 | (uint)(value * 255); } }
        public float Green { get { return (RawColor >> 8 & 0xFF) / 255f; } set { RawColor = RawColor & 0xFFFF00FF | (uint)(value * 255) << 8; } }
        public float Blue { get { return (RawColor >> 16 & 0xFF) / 255f; } set { RawColor = RawColor & 0xFF00FFFF | (uint)(value * 255) << 16; } }
    }
}
