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

namespace Patcher.Data.Plugins.Content.Constants.Skyrim
{
    public enum NpcTemplateFlags : ushort
    {
        None = 0,
        UseTraits = 0x01,
        UseStats = 0x02,
        UseFactions = 0x04,
        UseSpellList = 0x08,
        UseAIData = 0x10,
        UseAIPackages = 0x20,
        UseModel = 0x40,
        UseBaseData = 0x80,
        UseInventory = 0x100,
        UseScript = 0x200,
        UseDefPackList = 0x400,
        UseAttackData = 0x800,
        UseKeywords = 0x1000,
    }

}
