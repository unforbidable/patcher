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

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class AttackItem : Compound
    {
        [Member(Names.ATKD)]
        [Initialize]
        public AttackData Data { get; set; }

        [Member(Names.ATKE)]
        public string Event { get; set; }

        public override string ToString()
        {
            return Data.ToString();
        }

        public sealed class AttackData : Field
        {
            public float DamageMultiplier { get; set; }
            public float AttackChance { get; set; }
            public uint AttackSpell { get; set; }
            public AttackFlags Flags { get; set; }
            public float AttackAngle { get; set; }
            public float StrikeAngle { get; set; }
            public float Stagger { get; set; }
            public uint AttackType { get; set; }
            public float Knockdown { get; set; }
            public float RecoveryTime { get; set; }
            public float FatigueMultiplier { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                DamageMultiplier = reader.ReadSingle();
                AttackChance = reader.ReadSingle();
                AttackSpell = reader.ReadReference(FormKindSet.FromNames(Names.SPEL + Names.SHOU));
                Flags = (AttackFlags)reader.ReadUInt32();
                AttackAngle = reader.ReadSingle();
                StrikeAngle = reader.ReadSingle();
                Stagger = reader.ReadSingle();
                AttackType = reader.ReadReference(FormKindSet.KywdOnly);
                Knockdown = reader.ReadSingle();
                RecoveryTime = reader.ReadSingle();
                FatigueMultiplier = reader.ReadSingle();
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
    }
}
