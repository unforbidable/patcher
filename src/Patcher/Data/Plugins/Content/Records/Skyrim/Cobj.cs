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

using Patcher.Data.Plugins.Content.Fields;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.COBJ)]
    public class Cobj : GenericFormRecord
    {
        [Member(Names.COCT)]
        private uint IngredientCount { get; set; }

        [Member(Names.CNTO)]
        public List<IngredientData> Ingredients { get; set; }

        [Member(Names.COED)]
        private ByteArray Unknown { get; set; }

        [Member(Names.CTDA, Names.CIS1, Names.CIS2)]
        public List<Condition> Conditions { get; set; }

        [Member(Names.CNAM)]
        [Reference]
        public uint Result { get; set; }

        [Member(Names.BNAM)]
        [Reference(Names.KYWD)]
        public uint CraftingStation { get; set; }

        [Member(Names.NAM1)]
        public ushort ResultQuantity { get; set; }

        public class IngredientData : Field
        {
            public uint Ingredient { get; set; }
            public uint Quantity { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Ingredient = reader.ReadReference(FormKindSet.Any);
                Quantity = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Ingredient, FormKindSet.Any);
                writer.Write(Quantity);
            }

            public override Field CopyField()
            {
                return new IngredientData()
                {
                    Ingredient = Ingredient,
                    Quantity = Quantity
                };
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                var cast = (IngredientData)other;
                return Ingredient == cast.Ingredient && Quantity == cast.Quantity;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Ingredient;
            }
        }
    }
}
