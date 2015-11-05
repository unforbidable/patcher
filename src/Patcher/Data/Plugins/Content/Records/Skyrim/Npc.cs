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
using Patcher.Data.Plugins.Content.Fields;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.NPC_)]
    public sealed class Npc : GenericFormRecord, IFeaturingScripts, IFeaturingObjectBounds
    {
        [Member(Names.VMAD)]
        public VirtualMachineAdapter VirtualMachineAdapter { get; set; }

        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.ACBS)]
        [Initialize]
        private CharacterStats Stats { get; set; }

        [Member(Names.SNAM)]
        [Initialize]
        public List<FactionItem> Factions { get; set; }

        [Member(Names.INAM)]
        [Reference(Names.LVLI)]
        public uint DeathLeveledItem { get; set; }

        [Member(Names.VTCK)]
        [Reference(Names.VTYP)]
        public uint Voice { get; set; }

        [Member(Names.TPLT)]
        [Reference(Names.NPC_ + Names.LVLN)]
        public uint Template { get; set; }

        [Member(Names.RNAM)]
        [Reference(Names.RACE)]
        public uint Race { get; set; }

        [Member(Names.DEST, Names.DSTD, Names.DSTF)]
        private DestructionData Destruction { get; set; }

        [Member(Names.SPCT)]
        private uint? SpellCount { get; set; }

        [Member(Names.SPLO)]
        [Reference(Names.SPEL + Names.SHOU)]
        public List<uint> Spells { get; set; }

        [Member(Names.WNAM)]
        [Reference(Names.ARMO)]
        public uint Skin { get; set; }

        [Member(Names.ANAM)]
        [Reference(Names.ARMO)]
        public uint FarawaySkin { get; set; }

        [Member(Names.ATKR)]
        [Reference(Names.RACE)]
        public uint AttackRace { get; set; }

        [Member(Names.ATKD, Names.ATKE)]
        public List<AttackItem> Attacks { get; set; }

        [Member(Names.SPOR)]
        [Reference(Names.FLST)]
        public uint SpectatorOverride { get; set; }

        [Member(Names.OCOR)]
        [Reference(Names.FLST)]
        public uint ObserveCorpseOverride { get; set; }

        [Member(Names.GWOR)]
        [Reference(Names.FLST)]
        public uint GuardWarnOverride { get; set; }

        [Member(Names.ECOR)]
        [Reference(Names.FLST)]
        public uint CombatOverride { get; set; }

        [Member(Names.PRKZ)]
        private uint? PerkCount { get; set; }

        [Member(Names.PRKR)]
        public List<PerkItem> Perks { get; set; }

        [Member(Names.COCT)]
        private uint? ItemCount { get; set; }

        [Member(Names.CNTO, Names.COED)]
        public List<InventoryItem> Items { get; set; }

        [Member(Names.AIDT)]
        private BehaviorData Behavior { get; set; }

        [Member(Names.PKID)]
        [Reference(Names.PACK)]
        public List<uint> Packages { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.CNAM)]
        [Reference(Names.CLAS)]
        public uint Class { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.SHRT)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string ShortName { get; set; }

        [Member(Names.DATA)]
        private ByteArray EmptyData { get; set; }

        [Member(Names.DNAM)]
        private CharacterData Data { get; set; }

        [Member(Names.PNAM)]
        [Reference(Names.HDPT)]
        public uint HeadPart { get; set; }

        [Member(Names.HCLF)]
        [Reference(Names.CLFM)]
        public uint HairColor { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.CSTY)]
        public uint CombatStyle { get; set; }

        [Member(Names.GNAM)]
        [Reference(Names.FLST)]
        public uint Gifts { get; set; }

        [Member(Names.NAM5)]
        private ushort Unknown { get; set; }

        [Member(Names.NAM6)]
        public float HeightMultiplier { get; set; }

        [Member(Names.NAM7)]
        public float WeightMultiplier { get; set; }

        [Member(Names.NAM8)]
        public SoundLevel SoundLevel { get; set; }

        [Member(Names.CSDT, Names.CSDI, Names.CSDC)]
        public List<SoundData> Sounds { get; set; }

        [Member(Names.CSCR)]
        [Reference(Names.NPC_)]
        public uint AudioTemplate { get; set; }

        [Member(Names.DOFT)]
        [Reference(Names.OTFT)]
        public uint DefaultOutfit { get; set; }

        [Member(Names.SOFT)]
        [Reference(Names.OTFT)]
        public uint SleepOutfit { get; set; }

        [Member(Names.DPLT)]
        [Reference(Names.FLST)]
        public uint DefaultPackageList { get; set; }

        [Member(Names.CRIF)]
        [Reference(Names.FACT)]
        public uint CrimeFaction { get; set; }

        [Member(Names.FTST)]
        [Reference(Names.TXST)]
        public uint FaceTexture { get; set; }

        [Member(Names.QNAM)]
        private LightingData FaceLighting { get; set; }

        [Member(Names.NAM9)]
        private ByteArray FaceMorph { get; set; }

        [Member(Names.NAMA)]
        private ByteArray FaceParts { get; set; }

        [Member(Names.TINI, Names.TINC, Names.TINV, Names.TIAS)]
        public List<TintLayer> FaceTintLayer { get; set; }

        sealed class CharacterStats : Field
        {
            public CharacterFlags Flags { get; set; }
            public ushort MagickaOffset { get; set; }
            public ushort StaminaOffset { get; set; }
            public ushort Level { get; set; }
            public ushort CalcMinLevel { get; set; }
            public ushort CalcMaxLevel { get; set; }
            public ushort SpeedMultiplier { get; set; }
            public ushort BaseDisposition { get; set; }
            public NpcTemplateFlags TemplateFlags { get; set; }
            public ushort HealthOffset { get; set; }
            public ushort BleedoutOverride { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Flags = (CharacterFlags)reader.ReadUInt32();
                MagickaOffset = reader.ReadUInt16();
                StaminaOffset = reader.ReadUInt16();
                Level = reader.ReadUInt16();
                CalcMinLevel = reader.ReadUInt16();
                CalcMaxLevel = reader.ReadUInt16();
                SpeedMultiplier = reader.ReadUInt16();
                BaseDisposition = reader.ReadUInt16();
                TemplateFlags = (NpcTemplateFlags)reader.ReadUInt16();
                HealthOffset = reader.ReadUInt16();
                BleedoutOverride = reader.ReadUInt16();
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        sealed class BehaviorData : Field
        {
            public NpcAggression Aggression { get; set; }
            public NpcConfidence Confidence { get; set; }
            public byte Energe { get; set; }
            public NpcMorality Morality { get; set; }
            public NpcMood Mood { get; set; }
            public NpcAssistance Assistance { get; set; }
            public BehaviorFlags Flags { get; set; }
            public uint Warn { get; set; }
            public uint WarnAttack { get; set; }
            public uint Attack { get; set; }
            
            internal override void ReadField(RecordReader reader)
            {
                Aggression = (NpcAggression)reader.ReadByte();
                Confidence = (NpcConfidence)reader.ReadByte();
                Energe = reader.ReadByte();
                Morality = (NpcMorality)reader.ReadByte();
                Mood = (NpcMood)reader.ReadByte();
                Assistance = (NpcAssistance)reader.ReadByte();
                Flags = (BehaviorFlags)(reader.ReadUInt16() & 0xFF);
                Warn = reader.ReadUInt32();
                WarnAttack = reader.ReadUInt32();
                Attack = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }
        }

        sealed class CharacterData : Field
        {
            public byte[] BaseSkills { get; set; }
            public byte[] SkillOfsets { get; set; }
            public ushort CalculatedHealth { get; set; }
            public ushort CalculatedStamina { get; set; }
            public ushort CalculatedMagicka { get; set; }
            private ushort Padding { get; set; }
            public float FarawaySkinDistance { get; set; }
            public byte GearedUpWeapons { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                BaseSkills = reader.ReadBytes(18);
                SkillOfsets = reader.ReadBytes(18);
                CalculatedHealth = reader.ReadUInt16();
                CalculatedMagicka = reader.ReadUInt16();
                CalculatedMagicka = (ushort)(reader.ReadUInt32() & 0xFFFF);
                FarawaySkinDistance = reader.ReadSingle();
                GearedUpWeapons = (byte)(reader.ReadUInt32() & 0xFF);
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        sealed class LightingData : Field
        {
            public float Red { get; set; }
            public float Green { get; set; }
            public float Blue { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Red = reader.ReadSingle();
                Green = reader.ReadSingle();
                Blue = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        [Flags]
        enum CharacterFlags : uint
        {
            Female = 0x01,
            Essential = 0x02,
            IsCharGenFacePreset = 0x04,
            Respawn = 0x08,
            AutoCalcStats = 0x10,
            Unique = 0x20,
            DoesNotAffectStealthMeter = 0x40,
            PCLevelMult = 0x80,
            AudioTemplate = 0x100,
            Unknown9 = 0x200,
            Unknown10 = 0x400,
            Protected = 0x800,
            Unknown12 = 0x1000,
            Unknown13 = 0x2000,
            Summonable = 0x4000,
            Unknown15 = 0x8000,
            DoesNotBleed = 0x10000,
            Unknown17 = 0x20000,
            CanBeOwned = 0x40000,
            OppositeGenderAnim = 0x80000,
            SimpleActor = 0x100000,
            LoopedScript = 0x200000,
            Unknown22 = 0x400000,
            Unknown23 = 0x800000,
            Unknown24 = 0x1000000,
            Unknown25 = 0x2000000,
            Unknown26 = 0x4000000,
            Unknown27 = 0x8000000,
            LoopedAudio = 0x10000000,
            IsGhost = 0x20000000,
            Unknown30 = 0x40000000,
            Invulnerable = 0x80000000,
        }

        [Flags]
        enum BehaviorFlags
        {
            AggroRadiusBehavior = 0x01
        }
    }
}
