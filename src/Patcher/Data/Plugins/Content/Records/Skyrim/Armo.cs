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
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string Name { get; set; }

        [Member(Names.EITM)]
        [Reference(Names.SPEL)]
        public uint Effect { get; set; } 

        [Member(Names.MOD2, Names.MO2S, Names.MO2T)]
        public ModelMale MaleModel { get; set; }

        [Member(Names.MOD4, Names.MO4S, Names.MO4T)]
        public ModelFemale FemaleModel { get; set; }

        [Member(Names.BODT)]
        public UsageData Usage { get; set; }

        [Member(Names.BOD2)]
        private UsageData NewUsage { get; set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint DropSound { get; set; }

        [Member(Names.ETYP)]
        [Reference(Names.EQUP)]
        public uint EquipType { get; set; }

        [Member(Names.BIDS)]
        [Reference(Names.IPDS)]
        public uint BashImpactDataSet { get; set; }

        [Member(Names.BAMT)]
        [Reference(Names.MATT)]
        public uint AlternateBlockMaterial { get; set; }

        [Member(Names.RNAM)]
        [Reference(Names.RACE)]
        public uint Race { get; set; }

        [Member(Names.KSIZ)]
        private uint NumberOfKeywords { get; set; }

        [Member(Names.KWDA)]
        public Keywords Keywords { get; set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        [Member(Names.MODL)]
        [Reference(Names.ARMA)]
        public List<uint> ArmorAddons { get; set; }

        [Member(Names.DATA)]
        public MiscData Misc { get; set; }

        [Member(Names.DNAM)]
        [FakeFloat]
        public float ArmorRating { get; set; }

        [Member(Names.TNAM)]
        [Reference(Names.ARMO)]
        public uint TemplateArmor { get; set; }

        protected override void AfterRead(RecordReader reader)
        {
            if (Usage != null && NewUsage == null)
            {
                // Copy BODT to BOD2 and discard BODT
                NewUsage = Usage;
                Usage = null;
            }

            base.AfterRead(reader);
        }

        public class UsageData : Field
        {
            public BodyParts BodyParts { get; set; }
            public GeneralFlags General { get; set; }
            public ArmorSkillUsage SkillUsage { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                if (reader.CurrentSegment.Signature == Names.BODT)
                {
                    BodyParts = (BodyParts)reader.ReadUInt32();
                    General = (GeneralFlags)reader.ReadUInt32();
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
                else if (reader.CurrentSegment.Signature == Names.BOD2)
                {
                    BodyParts = (BodyParts)reader.ReadUInt32();
                    SkillUsage = (ArmorSkillUsage)reader.ReadInt32();

                    // Note: Unplayable flag is part of the record flags (0x04)
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                return new UsageData()
                {
                    BodyParts = BodyParts,
                    General = General,
                    SkillUsage = SkillUsage
                };
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("FirstPersonFlags={0}, ArmorType={1}", BodyParts, SkillUsage);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        public class MiscData : Field
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Value={0} Weight={1}", Value, Weight);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }
    }
}
