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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class Condition : Compound
    {
        [Member(Names.CTDA)]
        public ConditionData Data { get; set; }

        [Member(Names.CIS1)]
        public string Parameter1 { get; set; }

        [Member(Names.CIS2)]
        public string Parameter2 { get; set; }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Data.ToString();
        }

        public sealed class ConditionData : Field
        {
            public ConditionOperator Operators { get; set; }
            public FloatOrGlobal ComparisonValue { get; set; }
            public ConditionFunction Function { get; set; }
            public ulong Params { get; set; }
            public ConditionTarget RunOn { get; set; }
            public uint Reference { get; set; }
            public int Parameter1 { get; set; }
            
            internal override void ReadField(RecordReader reader)
            {
                Operators = (ConditionOperator)reader.ReadUInt32();
                ComparisonValue = reader.ReadUInt32();
                Function = (ConditionFunction)reader.ReadUInt32();
                Params = reader.ReadUInt64();
                RunOn = (ConditionTarget)reader.ReadUInt32();
                Reference = reader.ReadUInt32();
                Parameter1 = reader.ReadInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                return new ConditionData()
                {
                    Operators = Operators,
                    ComparisonValue = ComparisonValue,
                    Function = Function,
                    Params = Params,
                    RunOn = RunOn,
                    Reference = Reference,
                    Parameter1 = Parameter1
                };
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("Function='{0}'", Function);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public struct FloatOrGlobal
            {
                uint value;

                public uint Value { get { return value; } set { this.value = value; } }
                public float ValueAsFloat { get { return BitConverter.ToSingle(BitConverter.GetBytes(value), 0); } set { this.value = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0); } }

                public static implicit operator FloatOrGlobal(uint value)
                {
                    return new FloatOrGlobal()
                    {
                        Value = value
                    };
                }

                public static implicit operator FloatOrGlobal(float value)
                {
                    return new FloatOrGlobal()
                    {
                        ValueAsFloat = value
                    };
                }

                public override string ToString()
                {
                    char c = (char)value;
                    return string.Format("0x{0:X8} / {1:F6} / '{2}'", value, ValueAsFloat, char.IsLetterOrDigit(c) ? c : '?');
                }
            }
        }
    }
}
