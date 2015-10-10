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
    [Record(Names.ALCH)]
    public sealed class Alch : GenericFormRecord
    {
        [Member(Names.OBND)]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string Name { get; set; }

        [Member(Names.KSIZ)]
        private uint NumberOfKeywords { get; set; }

        [Member(Names.KWDA)]
        public Keywords Keywords { get; internal set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        [Member(Names.MODL, Names.MODS, Names.MODT)]
        public Model Model { get; internal set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint DropSound { get; set; }

        [Member(Names.ETYP)]
        [Reference(Names.EQUP)]
        public uint EquipmentType { get; set; }
        
        [Member(Names.DATA)]
        public MiscData Misc { get; set; }

        [Member(Names.ENIT)]
        public ConsumptionData Consumption { get; set; }

        [Member(Names.EFID, Names.EFIT, Names.CTDA, Names.CIS1, Names.CIS2)]
        public List<Effect> ResultEffects { get; set; } 

        protected override void BeforeRead(RecordReader reader)
        {
        }

        protected override void AfterRead(RecordReader reader)
        {
        }

        public class MiscData : Field
        {
            public float Weight { get; set; }

            internal override void ReadField(RecordReader reader)
            {
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
                    Weight = Weight
                };
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Weight={0}", Weight);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        public class ConsumptionData : Field
        {
            public int Value { get; set; }
            public IngestionFlags Ingestion { get; set; }
            public uint Addiction { get; set; }
            public float AddictionChance { get; set; }

            public uint ConsumeSound { get; set; } 

            internal override void ReadField(RecordReader reader)
            {
                Value = reader.ReadInt32();
                Ingestion = (IngestionFlags)reader.ReadUInt32();
                Addiction = reader.ReadReference(FormKind.None); // TODO: Investigate usage and type of reference
                AddictionChance = reader.ReadSingle();
                ConsumeSound = reader.ReadReference(FormKind.FromString(Names.SNDR));
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                return new ConsumptionData()
                {
                    Value = Value,
                    Ingestion = Ingestion,
                    Addiction = Addiction,
                    AddictionChance = AddictionChance,
                    ConsumeSound = ConsumeSound
                };
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("{0} Value={1}", Ingestion, Value);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }
    }
}
