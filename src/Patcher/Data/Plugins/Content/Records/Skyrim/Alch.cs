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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.ALCH)]
    [Game(Games.Skyrim)]
    public sealed class Alch : GenericFormRecord, IFeaturingObjectBounds, IFeaturingEffects
    {
        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.DESC)]
        [LocalizedString(LocalizedStringGroups.DLStrings)]
        public string Description { get; set; }

        [Member(Names.MODL, Names.MODS, Names.MODT)]
        [Initialize]
        private Model Model { get; set; }

        [Member(Names.YNAM)]
        [Reference(Names.SNDR)]
        public uint PickUpSound { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.SNDR)]
        public uint PutDownSound { get; set; }

        [Member(Names.ETYP)]
        [Reference(Names.EQUP)]
        private uint EquipmentType { get; set; }
        
        [Member(Names.DATA)]
        [Initialize]
        private MiscData Misc { get; set; }

        [Member(Names.ENIT)]
        [Initialize]
        private ConsumptionData Consumption { get; set; }

        [Member(Names.EFID, Names.EFIT, Names.CTDA, Names.CIS1, Names.CIS2)]
        [Initialize]
        public List<Effect> Effects { get; set; }

        public string WorldModel { get { return Model.Path; } set { Model.Path = value; } }
        public float Weight { get { return Misc.Weight; } set { Misc.Weight = value; } }
        public int Value { get { return Consumption.Value; } set { Consumption.Value = value; } }
        public uint UseSound { get { return Consumption.ConsumeSound; } set { Consumption.ConsumeSound = value; } }
        public PotionFlags PotionFlags { get { return Consumption.Flags; } set { Consumption.Flags = value; } }

        public class MiscData : Field
        {
            public float Weight { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Weight = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Weight);
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
                return Weight == ((MiscData)other).Weight;
            }

            public override string ToString()
            {
                return string.Format("Weight={0}", Weight);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        public class ConsumptionData : Field
        {
            public int Value { get; set; }
            public PotionFlags Flags { get; set; }
            public uint Addiction { get; set; }
            public float AddictionChance { get; set; }
            public uint ConsumeSound { get; set; } 

            internal override void ReadField(RecordReader reader)
            {
                Value = reader.ReadInt32();
                Flags = (PotionFlags)reader.ReadUInt32();
                Addiction = reader.ReadReference(FormKindSet.Any);
                AddictionChance = reader.ReadSingle();
                ConsumeSound = reader.ReadReference(FormKindSet.SndrOnly);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Value);
                writer.Write((uint)Flags);
                writer.WriteReference(Addiction, FormKindSet.Any);
                writer.Write(AddictionChance);
                writer.WriteReference(ConsumeSound, FormKindSet.SndrOnly);
            }

            public override Field CopyField()
            {
                return new ConsumptionData()
                {
                    Value = Value,
                    Flags = Flags,
                    Addiction = Addiction,
                    AddictionChance = AddictionChance,
                    ConsumeSound = ConsumeSound
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (ConsumptionData)other;
                return Value == cast.Value && Flags == cast.Flags && 
                    Addiction == cast.Addiction && AddictionChance == cast.AddictionChance && 
                    ConsumeSound == cast.ConsumeSound;
            }

            public override string ToString()
            {
                return string.Format("{0} Value={1}", Flags, Value);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Addiction;
                yield return ConsumeSound;
            }
        }
    }
}
