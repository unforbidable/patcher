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
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Patcher.Data.Plugins.Content.Functions.Skyrim
{
    public sealed class Signature
    {
        internal static readonly Signature Empty = new Signature();
        internal static readonly Signature Actor = new Signature(FormKindSet.ActorReferences);
        internal static readonly Signature Actor_AssocType = new Signature(FormKindSet.ActorReferences, FormKindSet.AstpOnly);
        internal static readonly Signature ActorValue = new Signature(typeof(ActorValue));
        internal static readonly Signature Alignment = new Signature(typeof(Alignment));
        internal static readonly Signature Any = new Signature(FormKindSet.Any);
        internal static readonly Signature AssocType = new Signature(FormKindSet.AstpOnly);
        internal static readonly Signature Axis = new Signature(typeof(Axis));
        internal static readonly Signature CastingType = new Signature(typeof(CastingType));
        internal static readonly Signature CastingType_Keyword = new Signature(typeof(CastingType), FormKindSet.KywdOnly);
        internal static readonly Signature Cell = new Signature(FormKindSet.CellOnly);
        internal static readonly Signature Class = new Signature(FormKindSet.ClasOnly);
        internal static readonly Signature CriticalStage = new Signature(typeof(CriticalStage));
        internal static readonly Signature EffectItem = new Signature(FormKindSet.Effects);
        internal static readonly Signature EffectItem_CastingType = new Signature(FormKindSet.Effects, typeof(CastingType));
        internal static readonly Signature EncounterZone = new Signature(FormKindSet.EcznOnly);
        internal static readonly Signature EquipType = new Signature(FormKindSet.EqupOnly);
        internal static readonly Signature Event = new Signature(typeof(int), FormKindSet.Any);
        internal static readonly Signature Faction = new Signature(FormKindSet.FactOnly);
        internal static readonly Signature Faction_Actor = new Signature(FormKindSet.FactOnly, FormKindSet.ActorReferences);
        internal static readonly Signature Faction_Faction = new Signature(FormKindSet.FactOnly, FormKindSet.FactOnly);
        internal static readonly Signature FormList = new Signature(FormKindSet.FlstOnly);
        internal static readonly Signature FormType = new Signature(typeof(FormType));
        internal static readonly Signature Furniture = new Signature(FormKindSet.FurnOnly);
        internal static readonly Signature FurnitureAnim = new Signature(typeof(FurnitureAnim));
        internal static readonly Signature FurnitureEntry = new Signature(typeof(FurnitureEntry));
        internal static readonly Signature GenderType = new Signature(typeof(GenderType));
        internal static readonly Signature Glob = new Signature(FormKindSet.GlobOnly);
        internal static readonly Signature Idle = new Signature(FormKindSet.IdleOnly);
        internal static readonly Signature Int = new Signature(typeof(int));
        internal static readonly Signature Int_Int = new Signature(typeof(int), typeof(int));
        internal static readonly Signature Int_Keyword = new Signature(typeof(int), FormKindSet.KywdOnly);
        internal static readonly Signature Int_Location = new Signature(typeof(int), FormKindSet.LctnOnly);
        internal static readonly Signature Int_LocRefType = new Signature(typeof(int), FormKindSet.LcrtOnly);
        internal static readonly Signature Inventory = new Signature(FormKindSet.InventoryObjects);
        internal static readonly Signature Keyword = new Signature(FormKindSet.KywdOnly);
        internal static readonly Signature Location = new Signature(FormKindSet.LctnOnly);
        internal static readonly Signature Location_LocRefType = new Signature(FormKindSet.LctnOnly, FormKindSet.LcrtOnly);
        internal static readonly Signature Location_Keyword = new Signature(FormKindSet.LctnOnly, FormKindSet.KywdOnly);
        internal static readonly Signature LocRefType = new Signature(FormKindSet.LcrtOnly);
        internal static readonly Signature MagicEffect = new Signature(FormKindSet.MgefOnly);
        internal static readonly Signature MiscStat = new Signature(typeof(MiscStat));
        internal static readonly Signature Npc = new Signature(FormKindSet.NpcOnly);
        internal static readonly Signature Owner = new Signature(FormKindSet.Owner);
        internal static readonly Signature Pack = new Signature(FormKindSet.PackOnly);
        internal static readonly Signature Perk = new Signature(FormKindSet.PerkOnly);
        internal static readonly Signature PlayerAction = new Signature(typeof(PlayerAction));
        internal static readonly Signature Quest = new Signature(FormKindSet.QustOnly);
        internal static readonly Signature Quest_Int = new Signature(FormKindSet.QustOnly, typeof(int));
        internal static readonly Signature Quest_String = new Signature(FormKindSet.QustOnly, typeof(string));
        internal static readonly Signature Race = new Signature(FormKindSet.RaceOnly);
        internal static readonly Signature Region = new Signature(FormKindSet.RegnOnly);
        internal static readonly Signature Referencable = new Signature(FormKindSet.ReferencableObjects);
        internal static readonly Signature Reference = new Signature(FormKindSet.References);
        internal static readonly Signature Reference_Axis = new Signature(FormKindSet.References, typeof(Axis));
        internal static readonly Signature Reference_Float = new Signature(FormKindSet.References, typeof(float));
        internal static readonly Signature Reference_Keyword = new Signature(FormKindSet.References, FormKindSet.KywdOnly);
        internal static readonly Signature Reference_Reference = new Signature(FormKindSet.References, FormKindSet.References);
        internal static readonly Signature Reference_String = new Signature(FormKindSet.References, typeof(string));
        internal static readonly Signature Scene = new Signature(FormKindSet.ScenOnly);
        internal static readonly Signature Shout = new Signature(FormKindSet.ShouOnly);
        internal static readonly Signature String = new Signature(typeof(string));
        internal static readonly Signature VoiceTypeOrList = new Signature(FormKindSet.VoiceTypeOrList);
        internal static readonly Signature WardState = new Signature(typeof(WardState));
        internal static readonly Signature Weather = new Signature(FormKindSet.WthrOnly);
        internal static readonly Signature Worldspace = new Signature(FormKindSet.WrldOnly);
        internal static readonly Signature VatsFunction_Value = new Signature(typeof(VatsFunction), typeof(int));

        public const int MaxParams = 2;

        ParamInfo[] formal = new ParamInfo[2];
        public ParamInfo this[int index] { get { return formal[index]; } }

        private Signature()
        {
        }

        private Signature(FormKindSet reference)
        {
            formal[0].Reference = reference;
        }

        private Signature(FormKindSet referenceA, FormKindSet referenceB)
        {
            formal[0].Reference = referenceA;
            formal[1].Reference = referenceB;
        }

        private Signature(Type type)
        {
            formal[0].PlainType = type;
        }

        private Signature(Type typeA, Type typeB)
        {
            formal[0].PlainType = typeA;
            formal[1].PlainType = typeB;
        }

        private Signature(FormKindSet reference, Type type)
        {
            formal[0].Reference = reference;
            formal[1].PlainType = type;
        }

        private Signature(Type type, FormKindSet reference)
        {
            formal[0].PlainType = type;
            formal[1].Reference = reference;
        }

        public override string ToString()
        {
            List<string> output = new List<string>(MaxParams);

            for (int i = 0; i < MaxParams; i++)
            {
                if (formal[i].IsReference)
                {
                    output.Add(formal[i].Reference.ToString());
                }
                else if (formal[i].IsPlainType)
                {
                    output.Add(formal[i].PlainType.Name);
                }
            }

            return string.Format("({0})", string.Join(",", output));
        }

        public string ToString(Condition condition)
        {
            List<object> output = new List<object>(MaxParams);

            for (int i = 0; i < MaxParams; i++)
            {
                if (formal[i].IsReference)
                {
                    output.Add(string.Format("{0:X8}", condition.GetReferenceParam(i)));
                }
                else if (formal[i].IsString)
                {
                    output.Add(string.Format("'{0}'", condition.GetStringParam(i)));
                }
                else if (formal[i].IsInt)
                {
                    output.Add(condition.GetIntParam(i));
                }
                else if (formal[i].IsFloat)
                {
                    output.Add(condition.GetFloatParam(i));
                }
            }

            return string.Format("({0})", string.Join(",", output));
        }

        public struct ParamInfo
        {
            public bool IsDefined { get { return IsReference || IsPlainType; } }
            public bool IsPlainType { get { return PlainType != null; } }
            public bool IsReference { get { return Reference != null; } }
            public bool IsString { get { return PlainType == typeof(string); } }
            public bool IsInt { get { return PlainType == typeof(int); } }
            public bool IsFloat { get { return PlainType == typeof(float); } }
            public FormKindSet Reference { get; internal set; }
            public Type PlainType { get; internal set; }
        }
    }
}
