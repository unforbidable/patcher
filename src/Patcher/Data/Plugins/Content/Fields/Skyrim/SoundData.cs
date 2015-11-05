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

using Patcher.Data.Plugins.Content.Constants.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class SoundData : Compound
    {
        [Member(Names.CSDT)]
        public SoundType Type { get; set; }

        [Member(Names.CSDI, Names.CSDC)]
        [Initialize]
        public List<SoundItem> Items { get; set; }

        public override string ToString()
        {
            return string.Format("Type={0}", Type);
        }

        public class SoundItem : Compound
        {
            [Member(Names.CSDI)]
            public uint Sound { get; set; }

            [Member(Names.CSDC)]
            public byte Chance { get; set; }

            public override string ToString()
            {
                return string.Format("Sound={0:X8}", Sound);
            }
        }
    }
}
