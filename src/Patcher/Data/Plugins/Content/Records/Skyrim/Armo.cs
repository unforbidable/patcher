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

using Patcher.Data.Plugins.Content.Enums.Skyrim;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.ARMO)]
    public sealed class Armo : GenericFormRecord
    {
        [Member(Names.VMAD)]
        public VirtualMachineAdapter VirtualMachineAdapter { get; set; }

        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.EITM)]
        [Reference(Names.ENCH)]
        public uint Enchantment { get; set; } 

        [Member(Names.MOD2, Names.MO2S, Names.MO2T)]
        [Initialize]
        private ModelMale MaleModel { get; set; }

        [Member(Names.MOD4, Names.MO4S, Names.MO4T)]
        [Initialize]
        private ModelFemale FemaleModel { get; set; }

        [Member(Names.BODT)]
        private UsageData Usage { get; set; }

        [Member(Names.BOD2)]
        private UsageData2 Usage2 { get; set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint PutDownSound { get; set; }

        [Member(Names.ETYP)]
        [Reference(Names.EQUP)]
        public uint EquipType { get; set; }

        [Member(Names.BIDS)]
        [Reference(Names.IPDS)]
        public uint BlockImpactDataSet { get; set; }

        [Member(Names.BAMT)]
        [Reference(Names.MATT)]
        public uint AlternateBlockMaterial { get; set; }

        [Member(Names.RNAM)]
        [Reference(Names.RACE)]
        public uint Race { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        [Member(Names.MODL)]
        [Reference(Names.ARMA)]
        [Initialize]
        public List<uint> Models { get; set; }

        [Member(Names.DATA)]
        [Initialize]
        private MiscData Misc { get; set; }

        [Member(Names.DNAM)]
        [FakeFloat]
        public float ArmorRating { get; set; }

        [Member(Names.TNAM)]
        [Reference(Names.ARMO)]
        public uint TemplateArmor { get; set; }

        public int Value { get { return Misc.Value; } set { Misc.Value = value; } }
        public float Weight { get { return Misc.Weight; } set { Misc.Weight = value; } }

        public string MaleWorldModel { get { return MaleModel.Path; } set { MaleModel.Path = value; } }
        public string FemaleWorldModel { get { return FemaleModel.Path; } set { FemaleModel.Path = value; } }

        public bool IsPlayable { get { return GetPlayable(); } set { SetPlayable(value); } }
        public bool IsShield { get { return HasFlag(ArmoRecordFlags.Shield); } set { SetFlag(ArmoRecordFlags.Shield, value); } }

        public ArmorSkillUsage SkillUsage { get { return GetSkillUsage(); } set { SetSkillUsage(value); } }
        public BodyParts BodyParts { get { return GetBodyParts(); } set { SetBodyParts(value); } }

        private bool GetPlayable()
        {
            // Negate value with ! to make playable (bool) mean non-playable (flag)
            if (Usage != null)
                // Retrieve NonPlayable flag from Usage
                return !Usage.Flags.HasFlag(ArmoUsageFlags.NonPlayable);
            else
                // Retrieve NonPlayable flag from record flags
                return !HasFlag(ArmoRecordFlags.NonPlayable);
        }

        private void SetPlayable(bool value)
        {
            // Negate value with ! to make playable (bool) mean non-playable (flag)
            if (Usage != null)
                // Set/clear flag in Usage if exists
                Usage.Flags = !value ? Usage.Flags | ArmoUsageFlags.NonPlayable : Usage.Flags & ~ArmoUsageFlags.NonPlayable;
            else
                // Otherwise set/clear record flags
                SetFlag(ArmoRecordFlags.NonPlayable, !value);
        }

        private ArmorSkillUsage GetSkillUsage()
        {
            if (Usage != null)
                return Usage.SkillUsage;
            else if (Usage2 != null)
                return Usage2.SkillUsage;
            else
                return ArmorSkillUsage.None;
        }

        private void SetSkillUsage(ArmorSkillUsage value)
        {
            EnsureEitherUsageExists();
            if (Usage != null)
                Usage.SkillUsage = value;
            else if (Usage2 != null)
                Usage2.SkillUsage = value;
        }

        private BodyParts GetBodyParts()
        {
            if (Usage != null)
                return Usage.BodyParts;
            else if (Usage2 != null)
                return Usage2.BodyParts;
            else
                return BodyParts.None;
        }

        private void SetBodyParts(BodyParts value)
        {
            EnsureEitherUsageExists();
            if (Usage != null)
                Usage.BodyParts = value;
            else if (Usage2 != null)
                Usage2.BodyParts = value;
        }

        private void EnsureEitherUsageExists()
        {
            if (Usage == null && Usage2 == null)
            {
                // Create BOD2 for new forms
                Usage2 = new UsageData2();
            }
        }

        protected override void BeforeWrite(RecordWriter writer)
        {
            // Ensure either Usage or Usage2 exists before saving record
            EnsureEitherUsageExists();
        }

        // Record flags used by ARMO
        enum ArmoRecordFlags : uint
        {
            NonPlayable = 0x04,
            Shield = 0x40
        }

        // Flags used internally in UsageData member
        enum ArmoUsageFlags : uint
        {
            NonPlayable = 0x10
        }

        class UsageData : Field
        {
            public BodyParts BodyParts { get; set; }
            public ArmoUsageFlags Flags { get; set; }
            public ArmorSkillUsage SkillUsage { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                BodyParts = (BodyParts)reader.ReadUInt32();
                Flags = (ArmoUsageFlags)reader.ReadUInt32();

                if (reader.CurrentSegment.Length == 12)
                {
                    SkillUsage = (ArmorSkillUsage)reader.ReadInt32();
                }
                else
                {
                    // Default to ArmorSkillUsage.None if the record is only 8 bytes long
                    SkillUsage = ArmorSkillUsage.None;
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((uint)BodyParts);
                writer.Write((uint)Flags);

                // Write skill usage too although it is optional
                writer.Write((uint)SkillUsage);
            }

            public override Field CopyField()
            {
                return new UsageData()
                {
                    BodyParts = BodyParts,
                    Flags = Flags,
                    SkillUsage = SkillUsage
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (UsageData)other;
                return BodyParts == cast.BodyParts && Flags == cast.Flags && SkillUsage == cast.SkillUsage;
            }

            public override string ToString()
            {
                return string.Format("BodyParts={0}, Skill={1}", BodyParts, SkillUsage);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        class UsageData2 : Field
        {
            public BodyParts BodyParts { get; set; }
            public ArmorSkillUsage SkillUsage { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                BodyParts = (BodyParts)reader.ReadUInt32();
                SkillUsage = (ArmorSkillUsage)reader.ReadInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((uint)BodyParts);
                writer.Write((uint)SkillUsage);
            }

            public override Field CopyField()
            {
                return new UsageData2()
                {
                    BodyParts = BodyParts,
                    SkillUsage = SkillUsage
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (UsageData2)other;
                return BodyParts == cast.BodyParts && SkillUsage == cast.SkillUsage;
            }

            public override string ToString()
            {
                return string.Format("BodyParts={0}, Skill={1}", BodyParts, SkillUsage);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        class MiscData : Field
        {
            public int Value { get; set; }
            public float Weight { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Value = reader.ReadInt32();
                Weight = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Value);
                writer.Write(Weight);
            }

            public override Field CopyField()
            {
                return new MiscData()
                {
                    Value = Value,
                    Weight = Weight
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (MiscData)other;
                return Value == cast.Value && Weight == cast.Weight;
            }

            public override string ToString()
            {
                return string.Format("Value={0} Weight={1}", Value, Weight);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }
    }
}
