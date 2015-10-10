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
    public enum ConditionFunction
    {
        GetWantBlocking = 0,
        GetDistance = 1,
        GetLocked = 5,
        GetPos = 6,
        GetAngle = 8,
        GetStartingPos = 10,
        GetStartingAngle = 11,
        GetSecondsPassed = 12,
        GetActorValue = 14,
        GetCurrentTime = 18,
        GetScale = 24,
        IsMoving = 25,
        IsTurning = 26,
        GetLineOfSight = 27,
        GetInSameCell = 32,
        GetDisabled = 35,

        // TODO Complete script funtion enum definition
    }
}
