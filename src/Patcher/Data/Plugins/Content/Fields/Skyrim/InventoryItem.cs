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
using System.Runtime.InteropServices;
using System.Text;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class InventoryItem : Compound
    {
        [Member(Names.CNTO)]
        [Initialize]
        private ItemData Item { get; set; }

        [Member(Names.COED)]
        private ExtraData Extra { get; set; }

        public override string ToString()
        {
            return Item.ToString();
        }

        sealed class ItemData : Field
        {
            public uint Item { get; set; }
            public uint Count { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Item = reader.ReadReference(FormKindSet.Any);
                Count = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Item, FormKindSet.Any);
                writer.Write(Count);
            }

            public override Field CopyField()
            {
                return new ItemData()
                {
                    Item = Item,
                    Count = Count
                };
            }

            public override string ToString()
            {
                return string.Format("{0:X8}, Count={1}", Item, Count);
            }

            public override bool Equals(Field other)
            {
                var cast = (ItemData)other;
                return Item == cast.Item && Count == cast.Count;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Item;
            }
        }

        sealed class ExtraData : Field
        {
            public uint Owner { get; set; }
            public float Condition { get; set; }

            GlobalVariableOrFactionRank Union;
            bool isGlobal = true;
            
            public uint GlobalVariable { get { return Union.GlobalValue; } set { Union.GlobalValue = value; isGlobal = true; } }
            public int FactionRank { get { return Union.FactionRank; } set { Union.FactionRank = value; isGlobal = false; } }

            internal override void ReadField(RecordReader reader)
            {
                throw new NotImplementedException();
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
                if (isGlobal)
                    yield return Union.GlobalValue;
            }

            public override string ToString()
            {
                return string.Format("Owner={0:8X}", Owner);
            }

            [StructLayout(LayoutKind.Explicit)]
            struct GlobalVariableOrFactionRank
            {
                [FieldOffset(0)]
                public uint GlobalValue;

                [FieldOffset(0)]
                public int FactionRank;
            }
        }
    }
}

