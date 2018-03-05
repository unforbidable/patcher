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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.WEAP)]
    [Game(Games.Skyrim)]
    public sealed class Weap : GenericFormRecord, IFeaturingObjectBounds, IFeaturingScripts
    {
        [Member(Names.VMAD)]
        public VirtualMachineAdapter VirtualMachineAdapter { get; set; }

        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.MODL, Names.MODT, Names.MODS)]
        [Initialize]
        private Model ModelData { get; set; }

        [Member(Names.ICON)]
        public string InventoryImage { get; set; }

        // Field not shown in TES5Edit
        [Member(Names.MICO)]
        public string MessageIcon { get; set; }

        [Member(Names.EITM)]
        [Reference(Names.ENCH)]
        public uint Enchantment { get; set; }

        [Member(Names.EAMT)]
        public ushort? EnchantmentAmount { get; set; }

        [Member(Names.DEST, Names.DSTD, Names.DSTF)]
        private DestructionData Destruction { get; set; }

        [Member(Names.ETYP)]
        [Reference(Names.EQUP)]
        public uint EquipType { get; set; }

        [Member(Names.BIDS)]
        [Reference(Names.IPDS)]
        public uint BlockImpactDataSet { get; set; }

        [Member(Names.BAMT)]
        [Reference(Names.MATT)]
        public uint AlternateBlockMaterial { get; set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint PutDownSound { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        // Field not shown in TES5Edit
        [Member(Names.NNAM)]
        public string EmbeddedWeaponNode { get; set; }

        [Member(Names.INAM)]
        [Reference(Names.IPDS)]
        public uint ImpactDataSet { get; set; }

        [Member(Names.WNAM)]
        [Reference(Names.STAT)]
        public uint FirstPersonModel { get; set; }

        [Member(Names.SNAM)]
        [Reference(Names.SNDR)]
        public uint AttackSound { get; set; }

        [Member(Names.XNAM)]
        [Reference(Names.SNDR)]
        public uint AttackSound2D { get; set; }

        [Member(Names.NAM7)]
        [Reference(Names.SNDR)]
        public uint AttackLoopSound { get; set; }

        [Member(Names.TNAM)]
        [Reference(Names.SNDR)]
        public uint AttackFailSound { get; set; }

        [Member(Names.UNAM)]
        [Reference(Names.SNDR)]
        public uint IdleSound { get; set; }

        [Member(Names.NAM9)]
        [Reference(Names.SNDR)]
        public uint EquipSound { get; set; }

        [Member(Names.NAM8)]
        [Reference(Names.SNDR)]
        public uint UnequipSound { get; set; }

        [Member(Names.DATA)]
        [Initialize]
        private MiscData Misc { get; set; }

        [Member(Names.DNAM)]
        [Initialize]
        private WeaponData Data { get; set; }

        [Member(Names.CRDT)]
        [Initialize]
        private CriticalData Critical { get; set; }

        [Member(Names.VNAM)]
        public SoundLevel SoundLevel { get; set; }

        [Member(Names.CNAM)]
        [Reference(Names.WEAP)]
        public uint TemplateWeapon { get; set; }

        protected override void BeforeWrite(RecordWriter writer)
        {
            // Sent ench ammount to null if no enchantment assigned
            if (Enchantment == 0)
                EnchantmentAmount = null;

            // Ensure enchantment amount is set (at least zero) if enchantment is assigned
            else if (EnchantmentAmount == null)
                EnchantmentAmount = 0;
        }

        public string WorldMode { get { return ModelData.Path; } set { ModelData.Path = value; } }

        public int Value { get { return Misc.Value; } set { Misc.Value = value; } }
        public float Weight { get { return Misc.Weight; } set { Misc.Weight = value; } }
        public short Damage { get { return Misc.Damage; } set { Misc.Damage = value; } }

        public ushort CriticalDamage { get { return Critical.Damage; } set { Critical.Damage = value; } }
        public float CriticalChanceMultiplier { get { return Critical.Multiplier; } set { Critical.Multiplier = value; } }
        public uint CriticalEffect { get { return Critical.SpellEffect; } set { Critical.SpellEffect = value; } }

        public WeaponType WeaponType { get { return Data.WeaponType; } set { Data.WeaponType = value; } }
        public float Speed { get { return Data.Speed; } set { Data.Speed = value; } }
        public float Reach { get { return Data.Reach; } set { Data.Reach = value; } }
        public AttackAnimation AttackAnimation { get { return Data.AttackAnimation; } set { Data.AttackAnimation = value; } }
        public byte NumberOfProjectiles { get { return Data.Projectiles; } set { Data.Projectiles = value; } }
        public float MinRange { get { return Data.MinRange; } set { Data.MinRange = value; } }
        public float MaxRange { get { return Data.MaxRange; } set { Data.MaxRange = value; } }
        public float AttackAnimationMultiplier { get { return Data.AttackAnimationMultiplier; } set { Data.AttackAnimationMultiplier = value; } }
        public ActorValue Skill { get { return Data.Skill; } set { Data.Skill = value; } }
        public ActorValue Resist { get { return Data.Resist; } set { Data.Resist = value; } }
        public float Stagger { get { return Data.Stagger; } set { Data.Stagger = value; } }

        public bool IsPlayable { get { return !Data.Flags.HasFlag(WeaponFlags.NonPlayable); } set { SetWeaponFlag(WeaponFlags.NonPlayable, !value); } }
        public bool CanDrop { get { return !Data.Flags.HasFlag(WeaponFlags.CannotDrop); } set { SetWeaponFlag(WeaponFlags.CannotDrop, !value); } }
        public bool IsBackpackHidden { get { return Data.Flags.HasFlag(WeaponFlags.HideBackpack); } set { SetWeaponFlag(WeaponFlags.HideBackpack, value); } }
        public bool IsRangeFixed { get { return Data.Flags2.HasFlag(WeaponFlags2.FixedRange); } set { SetWeaponFlag2(WeaponFlags2.FixedRange, value); } }
        public bool IsMinorCrime { get { return Data.Flags2.HasFlag(WeaponFlags2.MinorCrime); } set { SetWeaponFlag2(WeaponFlags2.MinorCrime, value); } }
        public bool IsBoundWeapon { get { return Data.Flags2.HasFlag(WeaponFlags2.BoundWeapon); } set { SetWeaponFlag2(WeaponFlags2.BoundWeapon, value); } }
        public bool IsNonHostile { get { return Data.Flags2.HasFlag(WeaponFlags2.NonHostile); } set { SetWeaponFlag2(WeaponFlags2.NonHostile, value); } }
        public bool CanUseInNormalCombat { get { return !Data.Flags2.HasFlag(WeaponFlags2.NotUsesInNormalCombat); } set { SetWeaponFlag2(WeaponFlags2.NotUsesInNormalCombat, !value); } }
        public bool IsAmmoUsed { get { return Data.Flags2.HasFlag(WeaponFlags2.NpcsUseAmmo); } set { SetWeaponFlag2(WeaponFlags2.NpcsUseAmmo, value); } }
        public bool IsPlayerOnly { get { return Data.Flags2.HasFlag(WeaponFlags2.PlayerOnly); } set { SetWeaponFlag2(WeaponFlags2.PlayerOnly, value); } }

        public bool IsCriticalEffectTriggeredOnDeath { get { return Critical.Flags.HasFlag(CriticalFlags.OnDeath); } set { SetCriticalFlag(CriticalFlags.OnDeath, value); } }

        public bool CanCauseDismemberment { get { return !Data.OnHitBehavior.HasFlag(OnHitBehavior.NoLimbDismember); } set { SetOnHitHehavior(OnHitBehavior.NoLimbDismember, !value); } }
        public bool CanCauseLimbExplosion { get { return !Data.OnHitBehavior.HasFlag(OnHitBehavior.NoLimbExplosion); } set { SetOnHitHehavior(OnHitBehavior.NoLimbExplosion, !value); } }

        public string WorldModel { get; internal set; }

        private void SetOnHitHehavior(OnHitBehavior value, bool set)
        {
            if (set)
                Data.OnHitBehavior |= value;
            else
                Data.OnHitBehavior &= ~value;
        }

        private void SetCriticalFlag(CriticalFlags value, bool set)
        {
            if (set)
                Critical.Flags |= value;
            else
                Critical.Flags &= ~value;
        }

        private void SetWeaponFlag(WeaponFlags value, bool set)
        {
            if (set)
                Data.Flags |= value;
            else
                Data.Flags &= ~value;
        }

        private void SetWeaponFlag2(WeaponFlags2 value, bool set)
        {
            if (set)
                Data.Flags2 |= value;
            else
                Data.Flags2 &= ~value;
        }

        class MiscData : Field
        {
            public int Value { get; set; }
            public float Weight { get; set; }
            public short Damage { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Value = reader.ReadInt32();
                Weight = reader.ReadSingle();
                Damage = reader.ReadInt16();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Value);
                writer.Write(Weight);
                writer.Write(Damage);
            }

            public override Field CopyField()
            {
                return new MiscData()
                {
                    Value = Value,
                    Weight = Weight,
                    Damage = Damage
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (MiscData)other;
                return Value == cast.Value && Weight == cast.Weight && Damage == cast.Damage;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }

            public override string ToString()
            {
                return string.Format("Value={0} Weight={1} Damage={2}", Value, Weight, Damage);
            }
        }

        [Flags]
        public enum OnHitBehavior
        {
            Normal = 0x00,
            NoLimbExplosion = 0x01,
            NoLimbDismember = 0x02,
            NoLimbDismemberOrExplode = 0x03
        }

        [Flags]
        enum WeaponFlags : ushort
        {
            None = 0,
            IgnoresNormalWeaponResistence = 0x0001,
            Automtic = 0x0002,
            HasScope = 0x0004,
            CannotDrop = 0x0008,
            HideBackpack = 0x0010,
            EmbeddedWeapon = 0x0020,
            NoFirstPersonIronSights = 0x0040,
            NonPlayable = 0x0080
        }

        [Flags]
        enum WeaponFlags2 : uint
        {
            None = 0,
            PlayerOnly = 0x0001,
            NpcsUseAmmo = 0x0002,
            NoJamAfterReload = 0x0004,
            MinorCrime = 0x0008,
            FixedRange = 0x0020,
            NotUsesInNormalCombat = 0x0040,
            NoThridPersonIronSights = 0x0100,
            BurstShot = 0x0200,
            AlternateRumble = 0x0400,
            LongBurst = 0x0800,
            NonHostile = 0x1000,
            BoundWeapon = 0x2000
        }

        class WeaponData : Field
        {
            public WeaponType WeaponType { get; set; }
            public float Speed { get; set; }
            public float Reach { get; set; }
            public WeaponFlags Flags { get; set; }
            private ushort Unknown1 { get; set; }
            public float SightFov { get; set; }
            private float Unknown2 { get; set; }
            public byte VatsToHitChance { get; set; }
            public AttackAnimation AttackAnimation { get; set; }
            public byte Projectiles { get; set; }
            public byte EmbeddedWeapon { get; set; }
            public float MinRange { get; set; }
            public float MaxRange { get; set; }
            public OnHitBehavior OnHitBehavior { get; set; }
            public WeaponFlags2 Flags2 { get; set; }
            public float AttackAnimationMultiplier { get; set; }
            private float Unknown3 { get; set; }
            public float RumbleLeftMotorStrength { get; set; }
            public float RumbleRightMotosStrength { get; set; }
            public float RumbleDuration { get; set; }
            private uint Unknown4 { get; set; }
            private uint Unknown5 { get; set; }
            private uint Unknown6 { get; set; }
            public ActorValue Skill { get; set; }
            private uint Unknown7 { get; set; }
            private uint Unknown8 { get; set; }
            public ActorValue Resist { get; set; }
            private uint Unknown9 { get; set; }
            public float Stagger { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                WeaponType = (WeaponType)(reader.ReadInt32() & 0xFF);
                Speed = reader.ReadSingle();
                Reach = reader.ReadSingle();
                Flags = (WeaponFlags)reader.ReadUInt16();
                Unknown1 = reader.ReadUInt16();
                SightFov = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                VatsToHitChance = reader.ReadByte();
                AttackAnimation = (AttackAnimation)reader.ReadByte();
                Projectiles = reader.ReadByte();
                EmbeddedWeapon = reader.ReadByte();
                MinRange = reader.ReadSingle();
                MaxRange = reader.ReadSingle();
                OnHitBehavior = (OnHitBehavior)reader.ReadUInt32();
                Flags2 = (WeaponFlags2)reader.ReadUInt32();
                AttackAnimationMultiplier = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();
                RumbleLeftMotorStrength = reader.ReadSingle();
                RumbleRightMotosStrength = reader.ReadSingle();
                RumbleDuration = reader.ReadSingle();
                Unknown4 = reader.ReadUInt32();
                Unknown5 = reader.ReadUInt32();
                Unknown6 = reader.ReadUInt32();
                Skill = (ActorValue)reader.ReadInt32();
                Unknown7 = reader.ReadUInt32();
                Unknown8 = reader.ReadUInt32();
                Resist = (ActorValue)reader.ReadInt32();
                Unknown9 = reader.ReadUInt32();
                Stagger = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((uint)WeaponType & 0xFF);
                writer.Write(Speed);
                writer.Write(Reach);
                writer.Write((ushort)Flags);
                writer.Write(Unknown1);
                writer.Write(SightFov);
                writer.Write(Unknown2);
                writer.Write(VatsToHitChance);
                writer.Write((byte)AttackAnimation);
                writer.Write(Projectiles);
                writer.Write(EmbeddedWeapon);
                writer.Write(MinRange);
                writer.Write(MaxRange);
                writer.Write((uint)OnHitBehavior);
                writer.Write((uint)Flags2);
                writer.Write(AttackAnimationMultiplier);
                writer.Write(Unknown3);
                writer.Write(RumbleLeftMotorStrength);
                writer.Write(RumbleRightMotosStrength);
                writer.Write(RumbleDuration);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write((int)Skill);
                writer.Write(Unknown7);
                writer.Write(Unknown8);
                writer.Write((int)Resist);
                writer.Write(Unknown9);
                writer.Write(Stagger);
            }

            public override Field CopyField()
            {
                return new WeaponData()
                {
                    WeaponType = WeaponType,
                    Speed = Speed,
                    Reach = Reach,
                    Flags = Flags,
                    Unknown1 = Unknown1,
                    SightFov = SightFov,
                    Unknown2 = Unknown2,
                    VatsToHitChance = VatsToHitChance,
                    AttackAnimation = AttackAnimation,
                    Projectiles = Projectiles,
                    EmbeddedWeapon = EmbeddedWeapon,
                    MinRange = MinRange,
                    MaxRange = MaxRange,
                    OnHitBehavior = OnHitBehavior,
                    Flags2 = Flags2,
                    AttackAnimationMultiplier = AttackAnimationMultiplier,
                    Unknown3 = Unknown3,
                    RumbleLeftMotorStrength = RumbleLeftMotorStrength,
                    RumbleRightMotosStrength = RumbleRightMotosStrength,
                    RumbleDuration = RumbleDuration,
                    Unknown4 = Unknown4,
                    Unknown5 = Unknown5,
                    Unknown6 = Unknown6,
                    Skill = Skill,
                    Unknown7 = Unknown7,
                    Unknown8 = Unknown8,
                    Resist = Resist,
                    Unknown9 = Unknown9,
                    Stagger = Stagger
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (WeaponData)other;
                return WeaponType == cast.WeaponType && Speed == cast.Speed && Reach == cast.Reach && Flags == cast.Flags && 
                    Unknown1 == cast.Unknown1 && SightFov == cast.SightFov && Unknown2 == cast.Unknown2 && VatsToHitChance == cast.VatsToHitChance && 
                    AttackAnimation == cast.AttackAnimation && Projectiles == cast.Projectiles && EmbeddedWeapon == cast.EmbeddedWeapon && 
                    MinRange == cast.MinRange && MaxRange == cast.MaxRange && OnHitBehavior == cast.OnHitBehavior && Flags2 == cast.Flags2 && 
                    AttackAnimationMultiplier == cast.AttackAnimationMultiplier && Unknown3 == cast.Unknown3 && 
                    RumbleLeftMotorStrength == cast.RumbleLeftMotorStrength && RumbleRightMotosStrength == cast.RumbleRightMotosStrength &&
                    RumbleDuration == cast.RumbleDuration && Unknown4 == cast.Unknown4 && Unknown5 == cast.Unknown5 && Unknown6 == cast.Unknown6 &&
                    Skill == cast.Skill && Unknown7 == cast.Unknown7 && Unknown8 == cast.Unknown8 && Resist == cast.Resist && 
                    Unknown9 == cast.Unknown9 && Stagger == cast.Stagger;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }

            public override string ToString()
            {
                return string.Format("Animation={0} Speed={1} Reach={2}", WeaponType, Speed, Reach);
            }
        }

        [Flags]
        enum CriticalFlags : uint
        {
            OnDeath = 0x01
        }

        class CriticalData : Field
        {
            public ushort Damage { get; set; }
            private ushort Unknown { get; set; }
            public float Multiplier { get; set; }
            public CriticalFlags Flags { get; set; }
            public uint SpellEffect { get; set; }
            public uint Unknown1 { get; set; } // Added in Skyrim SE
            public uint Unknown2 { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Damage = reader.ReadUInt16();
                Unknown = reader.ReadUInt16();
                Multiplier = reader.ReadSingle();
                Flags = (CriticalFlags)reader.ReadUInt32();
                SpellEffect = reader.ReadReference(FormKindSet.SpelOnly);

                Unknown1 = reader.ReadUInt32();
                Unknown2 = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Damage);
                writer.Write(Unknown);
                writer.Write(Multiplier);
                writer.Write((uint)Flags);
                writer.WriteReference(SpellEffect, FormKindSet.SpelOnly);
            }

            public override Field CopyField()
            {
                return new CriticalData()
                {
                    Damage = Damage,
                    Unknown = Unknown,
                    Multiplier = Multiplier,
                    Flags = Flags,
                    SpellEffect = SpellEffect
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (CriticalData)other;
                return Damage == cast.Damage && Unknown == cast.Unknown && Multiplier == cast.Multiplier && Flags == cast.Flags && SpellEffect == cast.SpellEffect;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return SpellEffect;
            }

            public override string ToString()
            {
                return string.Format("Damage={0} Multiplier={1}", Damage, Multiplier);
            }
        }
    }
}
