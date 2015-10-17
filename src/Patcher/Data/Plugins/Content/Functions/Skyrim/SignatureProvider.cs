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

namespace Patcher.Data.Plugins.Content.Functions.Skyrim
{
    sealed class SignatureProvider
    {
        public static readonly SignatureProvider Default = new SignatureProvider();

        public bool Contains(Function code)
        {
            return map.ContainsKey(code);
        }

        public Signature GetSignature(Function code)
        {
            if (!map.ContainsKey(code))
                return Signature.Empty;

            return map[code];
        }

        static IDictionary<Function, Signature> map = new SortedDictionary<Function, Signature>()
        {
            { Function.GetActorValue, Signature.ActorValue },
            { Function.GetAngle, Signature.Axis },
            { Function.GetCrime, Signature.Actor },
            { Function.GetDetected, Signature.Actor },
            { Function.GetDistance, Signature.Any },
            { Function.GetEquipped, Signature.Inventory },
            { Function.GetFactionRank, Signature.Faction },
            { Function.GetFactionRankDifference, Signature.Faction_Actor },
            { Function.GetGlobalValue, Signature.Glob },
            { Function.GetHeadingAngle, Signature.Reference },
            { Function.GetInCell, Signature.Cell },
            { Function.GetInCurrentLoc, Signature.Location },
            { Function.GetInFaction, Signature.Faction },
            { Function.GetInSameCell, Signature.Reference },
            { Function.GetIsClass, Signature.Class },
            { Function.GetIsCrimeFaction, Signature.Faction },
            { Function.GetIsCurrentPackage, Signature.Pack },
            { Function.GetIsCurrentWeather, Signature.Weather },
            { Function.GetIsId, Signature.Referencable },
            { Function.GetIsRace, Signature.Race },
            { Function.GetIsReference, Signature.Referencable },
            { Function.GetIsSex, Signature.Gender },
            { Function.GetItemCount, Signature.Inventory },
            { Function.GetLineOfSight, Signature.Reference },
            { Function.GetPCInFaction, Signature.Faction },
            { Function.GetPCIsClass, Signature.Class },
            { Function.GetPCIsRace, Signature.Race },
            { Function.GetPCIsSex, Signature.Gender },
            { Function.GetPlayerControlsDisabled, Signature.Int_Int },
            { Function.GetPos, Signature.Axis },
            { Function.GetQuestCompleted, Signature.Quest },
            { Function.GetQuestRunning, Signature.Quest },
            { Function.GetQuestVariable, Signature.Quest_String },
            { Function.GetScriptVariable, Signature.Reference_String },
            { Function.GetShouldAttack, Signature.Actor },
            { Function.GetStage, Signature.Quest },
            { Function.GetStageDone, Signature.Quest_Int },
            { Function.GetStartingAngle, Signature.Axis },
            { Function.GetStartingPos, Signature.Axis },
            { Function.GetTalkedToPCParam, Signature.Actor },
            { Function.GetVMQuestVariable, Signature.Quest_String },
            { Function.HasKeyword, Signature.Keyword },
            { Function.HasPerk, Signature.Perk },
            { Function.HasSameEditorLocAsRef, Signature.Reference_Kywd },
            { Function.HasSameEditorLocAsRefAlias, Signature.Int_Keyword },
            { Function.HasSpell, Signature.Effect },
            { Function.IsCurrentFurnitureObj, Signature.Furniture },
            { Function.IsCurrentFurnitureRef, Signature.Reference },
            { Function.IsPlayerInRegion, Signature.Region },
            { Function.IsWeaponSkillType, Signature.ActorValue },
            { Function.MenuMode, Signature.Int },
            { Function.SameFaction, Signature.Actor },
            { Function.SameRace, Signature.Actor },
            { Function.SameSex, Signature.Actor },
        };
    }
}
