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

namespace Patcher.Data.Plugins.Content.Enums.Skyrim
{
    [Flags]
    public enum ConditionOperator : uint
    {
        NotEquals = 0x00,
        Equals = 0x01,
        GreaterThan = 0x02,
        LesserThen = 0x04,
        Or = 0x08,
        UseAliases = 0x10,
        UseGlobal = 0x20,
        UsePackData = 0x40,
        SwapSubjectAndTarget = 0x80
    }
}
