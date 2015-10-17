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
using Patcher.Data.Plugins.Content.Functions.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class Condition : Compound
    {
        [Member(Names.CTDA)]
        [Initialize]
        private ConditionData Data { get; set; }

        [Member(Names.CIS1)]
        private string StringParameter1 { get; set; }

        [Member(Names.CIS2)]
        private string StringParameter2 { get; set; }

        public ConditionFlags Flags { get { return Data.Flags; } set { Data.Flags = value; } }
        public Function Function { get { return Data.Function; } set { Data.Function = value; } }

        public uint GlobalVariableOperand { get { return Data.Operand.GlobalVariable; } set { Data.Operand.GlobalVariable = value; } }
        public float FloatOperand { get { return Data.Operand.FloatValue; } set { Data.Operand.FloatValue = value; } }

        public FunctionTarget FunctionTarget { get { return Data.Target; } set { Data.Target = value; } }
        public uint FunctionTargetReference { get { return Data.TargetReference; } set { Data.TargetReference = value; } }

        public void SetStringParam(int index, string value)
        {
            if (index == 0)
                StringParameter1 = value;
            else if (index == 1)
                StringParameter2 = value;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0 or 1.");
        }

        public void SetReferenceParam(int index, uint value)
        {
            if (index == 0)
                Data.Params.UInt32_0 = value;
            else if (index == 1)
                Data.Params.UInt32_1 = value;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0 or 1.");
        }

        public void SetIntParam(int index, int value)
        {
            if (index == 0)
                Data.Params.Int32_0 = value;
            else if (index == 1)
                Data.Params.Int32_1 = value;
            else if (index == 2)
                Data.IntParam3 = value;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0, 1 or 2.");
        }

        public uint GetReferenceParam(int index)
        {
            if (index == 0)
                return Data.Params.UInt32_0;
            else if (index == 1)
                return Data.Params.UInt32_1;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0 or 1.");
        }

        public string GetStringParam(int index)
        {
            if (index == 0)
                return StringParameter1;
            else if (index == 1)
                return StringParameter2;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0 or 1.");
        }

        public int GetIntParam(int index)
        {
            if (index == 0)
                return Data.Params.Int32_0;
            else if (index == 1)
                return Data.Params.Int32_1;
            else if (index == 2)
                return Data.IntParam3;
            else
                throw new IndexOutOfRangeException("Parameter index must be 0, 1 or 2.");
        }

        public Signature FunctionSignature { get { return SignatureProvider.Default.GetSignature(Data.Function); } }

        public override string ToString()
        {
            return string.Format("Function={0}{1}", Function, SignatureProvider.Default.GetSignature(Function).ToString(this));
        }

        sealed class ConditionData : Field
        {
            public ConditionFlags Flags { get; set; }
            public Function Function { get; set; }
            public FunctionTarget Target { get; set; }
            public uint TargetReference { get; set; }

            public ConditionOperand Operand;
            public FunctionParams Params;

            public int IntParam3 { get; set; }

            public ConditionData()
            {
                // Default value for third parameter
                IntParam3 = -1;
            }

            internal override void ReadField(RecordReader reader)
            {
                // Read flags and skip 3 bytes
                Flags = (ConditionFlags)reader.ReadByte();
                reader.Seek(3);

                // Read either global variable reference or float
                if (Flags.HasFlag(ConditionFlags.UseGlobal))
                    Operand.GlobalVariable = reader.ReadReference(FormKindSet.GlobOnly);
                else
                    Operand.FloatValue = reader.ReadSingle();

                // Read function code and skip 2 bytes
                Function = (Function)reader.ReadUInt16();
                reader.Seek(2);

                // Warn if unknown function - no enum value is defined for it
                if (!Enum.IsDefined(typeof(Function), Function))
                    Log.Warning("Function '{0}' was not recorgnised and any of parameter references may have may get scrambled.", Function);

                // Find function signature
                var signature = SignatureProvider.Default.GetSignature(Function);

                // Read function params
                // References must be read with ReadReference function
                if (signature[0].IsReference)
                    Params.UInt32_0 = reader.ReadReference(signature[0].Reference);
                else
                    Params.Int32_0 = reader.ReadInt32();

                if (signature[1].IsReference)
                    Params.UInt32_1 = reader.ReadReference(signature[1].Reference);
                else
                    Params.Int32_1 = reader.ReadInt32();

                Target = (FunctionTarget)reader.ReadUInt32();
                TargetReference = reader.ReadReference(FormKindSet.Any);

                // Third parameter
                IntParam3 = reader.ReadInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((byte)Flags);
                writer.Write((byte)0);
                writer.Write((short)0);

                if (Flags.HasFlag(ConditionFlags.UseGlobal))
                    writer.WriteReference(Operand.GlobalVariable, FormKindSet.GlobOnly);
                else
                    writer.Write(Operand.FloatValue);

                writer.Write((ushort)Function);
                writer.Write((short)0);

                // Find function signature
                var signature = SignatureProvider.Default.GetSignature(Function);

                // Read function params
                // References must be read with ReadReference function
                if (signature[0].IsReference)
                    writer.WriteReference(Params.UInt32_0, signature[0].Reference);
                else
                    writer.Write(Params.Int32_0);

                if (signature[1].IsReference)
                    writer.WriteReference(Params.UInt32_1, signature[1].Reference);
                else
                    writer.Write(Params.Int32_1);

                writer.Write((uint)Target);
                if (Target == FunctionTarget.Subject || Target == FunctionTarget.Subject || Target == FunctionTarget.CombatTarget)
                    writer.Write((uint)0);
                else
                    writer.WriteReference(TargetReference, FormKindSet.Any);

                // Third parameter
                writer.Write(IntParam3);
            }

            public override Field CopyField()
            {
                return new ConditionData()
                {
                    Flags = Flags,
                    Operand = Operand,
                    Function = Function,
                    Params = new FunctionParams()
                    {
                        UInt64_0 = Params.UInt64_0
                    },
                    Target = Target,
                    TargetReference = TargetReference,
                    IntParam3 = IntParam3
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (ConditionData)other;
                return Flags == cast.Flags && Operand.FloatValue == cast.Operand.FloatValue && Function == cast.Function &&
                    Params.UInt64_0 == cast.Params.UInt64_0 && Target == cast.Target && TargetReference == cast.TargetReference && IntParam3 == cast.IntParam3;
            }

            public override string ToString()
            {
                return string.Format("Code={0}{1}, Params={2:X16}", 
                    Function,
                    SignatureProvider.Default.GetSignature(Function), 
                    Params.UInt64_0);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                if (Flags.HasFlag(ConditionFlags.UseGlobal))
                    yield return Operand.GlobalVariable;

                var signature = SignatureProvider.Default.GetSignature(Function);
                if (signature[0].IsReference)
                    yield return Params.UInt32_0;

                if (signature[1].IsReference)
                    yield return Params.UInt32_1;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct ConditionOperand
            {
                [FieldOffset(0)]
                public uint GlobalVariable;

                [FieldOffset(0)]
                public float FloatValue;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct FunctionParams
            {
                [FieldOffset(0)]
                public ulong UInt64_0;

                [FieldOffset(0)]
                public uint UInt32_0;

                [FieldOffset(4)]
                public uint UInt32_1;

                [FieldOffset(0)]
                public int Int32_0;

                [FieldOffset(4)]
                public int Int32_1;

                [FieldOffset(0)]
                public ushort UInt16_0; // Event function

                [FieldOffset(2)]
                public ushort UInt16_1; // Event member

                [FieldOffset(4)]
                public ushort UInt16_2;

                [FieldOffset(6)]
                public ushort UInt16_3;
            }

        }
    }
}
