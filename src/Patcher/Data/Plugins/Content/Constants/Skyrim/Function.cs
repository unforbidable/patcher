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
    public enum Function : uint
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
        MenuMode = 36,
        GetDisease = 39,
        GetClothingValue = 41,
        SameFaction = 42,
        SameRace = 43,
        SameSex = 44,
        GetDetected = 45,
        GetDead = 46,
        GetItemCount = 47,
        GetGold = 48,
        GetSleeping = 49,
        GetTalkedToPC = 50,
        GetScriptVariable = 53,
        GetQuestRunning = 56,
        GetStage = 58,
        GetStageDone = 59,
        GetFactionRankDifference = 60,
        GetAlarmed = 61,
        IsRaining = 62,
        GetAttacked = 63,
        GetIsCreature = 64,
        GetLockLevel = 65,
        GetShouldAttack = 66,
        GetInCell = 67,
        GetIsClass = 68,
        GetIsRace = 69,
        GetIsSex = 70,
        GetInFaction = 71,
        GetIsId = 72,
        GetFactionRank = 73,
        GetGlobalValue = 74,
        IsSnowing = 75,
        GetRandomPercent = 77,
        GetQuestVariable = 79,
        GetLevel = 80,
        IsRotating = 81,
        GetDeadCount = 84,
        GetIsAlerted = 91,
        GetPlayerControlsDisabled = 98,
        GetHeadingAngle = 99,
        IsWeaponMagicOut = 101,
        IsTorchOut = 102,
        IsShieldOut = 103,
        IsFacingUp = 106,
        GetKnockedState = 107,
        GetWeaponAnimType = 108,
        IsWeaponSkillType = 109,
        GetCurrentAIPackage = 110,
        IsWaiting = 111,
        IsIdlePlaying = 112,
        IsIntimidatedByPlayer = 116,
        IsPlayerInRegion = 117,
        GetActorAggroRadiusViolated = 118,
        GetCrime = 122,
        IsGreetingPlayer = 123,
        IsGuard = 125,
        HasBeenEaten = 127,
        GetStaminaPercentage = 128,
        GetPCIsClass = 129,
        GetPCIsRace = 130,
        GetPCIsSex = 131,
        GetPCInFaction = 132,
        SameFactionAsPC = 133,
        SameRaceAsPC = 134,
        SameSexAsPC = 135,
        GetIsReference = 136,
        IsTalking = 141,
        GetWalkSpeed = 142,
        GetCurrentAIProcedure = 143,
        GetTrespassWarningLevel = 144,
        IsTrespassing = 145,
        IsInMyOwnedCell = 146,
        GetWindSpeed = 147,
        GetCurrentWeatherPercent = 148,
        GetIsCurrentWeather = 149,
        IsContinuingPackagePCNear = 150,
        GetIsCrimeFaction = 152,
        CanHaveFlames = 153,
        HasFlames = 154,
        GetOpenState = 157,
        GetSitting = 159,
        GetIsCurrentPackage = 161,
        IsCurrentFurnitureRef = 162,
        IsCurrentFurnitureObj = 163,
        GetDayOfWeek = 170,
        GetTalkedToPCParam = 172,
        IsPCSleeping = 175,
        IsPCAMurderer = 176,
        HasSameEditorLocAsRef = 180,
        HasSameEditorLocAsRefAlias = 181,
        GetEquipped = 182,


        HasSpell = 264,
        GetInCurrentLoc = 359,
        HasPerk = 448,
        GetQuestCompleted = 543,
        HasKeyword = 560,
        GetVMQuestVariable = 629,
        EPTemperingItemIsEnchanted = 659,

        // TODO Complete script funtion enum definition and specify fucntion signatures if necessary

    }
}
