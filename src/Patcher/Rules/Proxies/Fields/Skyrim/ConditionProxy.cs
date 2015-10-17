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

using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Rules.Proxies.Forms.Skyrim;
using Patcher.Rules.Proxies.Forms;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(ICondition))]
    public sealed class ConditionProxy : FieldProxy<Condition>, ICondition, IDumpabled
    {
        public ICondition AndNext()
        {
            ClearFlag(ConditionFlags.Or);
            return this;
        }

        public ICondition IsEqualTo(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsFalse()
        {
            return IsEqualTo(0);
        }

        public ICondition IsGreaterThan(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsGreaterThan(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsGreaterThanOrEqualTo(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsGreaterThanOrEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsLessThen(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsLessThen(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsLessThenOrEqualTo(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsLessThenOrEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsNotEqualTo(IGlob glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsNotEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.Equal);
            return this;
        }

        public ICondition IsTrue()
        {
            return IsEqualTo(1);
        }

        public ICondition OrNext()
        {
            SetFlag(ConditionFlags.Or);
            return this;
        }

        public ICondition RunOn(IForm reference)
        {
            Field.FunctionTarget = FunctionTarget.Reference;
            Field.FunctionTargetReference = reference.ToFormId();
            return this;
        }

        public ICondition RunOn(RunOn runOn)
        {
            Field.FunctionTarget = runOn.ToTarget();
            Field.FunctionTargetReference = 0;
            return this;
        }

        public ICondition SwapSubjectAndTarget()
        {
            SetFlag(ConditionFlags.SwapSubjectAndTarget);
            return this;
        }

        private void UseGlobal(IGlob glob)
        {
            Field.GlobalVariableOperand = glob.ToFormId();
            SetFlag(ConditionFlags.UseGlobal);
        }

        private void UseFloat(float value)
        {
            Field.FloatOperand = value;
            ClearFlag(ConditionFlags.UseGlobal);
        }

        private void UseOperator(ConditionFlags oper)
        {
            Field.Flags &= ~ConditionFlags.OperatorMask;
            Field.Flags |= oper;
        }

        private void SetFlag(ConditionFlags flag)
        {
            Field.Flags |= flag;
        }

        private void ClearFlag(ConditionFlags flag)
        {
            Field.Flags &= (~flag) | ConditionFlags.OperatorMask;
        }

        public override string ToString()
        {
            return Field.ToString();
        }

        void IDumpabled.Dump(ObjectDumper dumper)
        {
            dumper.DumpText("Function", FunctionToString());
            if (Field.Flags.HasFlag(ConditionFlags.UseGlobal))
                dumper.DumpObject("Operand", Provider.CreateReferenceProxy<GlobProxy>(Field.GlobalVariableOperand));
            else
                dumper.DumpObject("Operand", Field.FloatOperand);
            dumper.DumpObject("RunOn", Field.FunctionTarget);
            if (Field.FunctionTarget == FunctionTarget.Reference || Field.FunctionTarget == FunctionTarget.LinkedReference)
                dumper.DumpObject("RunOnReference", Provider.CreateReferenceProxy<FormProxy>(Field.FunctionTargetReference));
            dumper.DumpObject("Flags", Field.Flags);
        }

        private string FunctionToString()
        {
            var args = new List<object>(2);

            var sig = Field.FunctionSignature;

            if (sig.IsReferenceA)
                args.Add(Provider.CreateReferenceProxy<FormProxy>(Field.ReferenceParam1));
            else if (sig.TypeA == typeof(string))
                args.Add(string.Format("'{0}'", Field.StringParameter1));
            else if (sig.TypeA != null)
                args.Add(Field.IntParam1);

            if (sig.IsReferenceB)
                args.Add(Provider.CreateReferenceProxy<FormProxy>(Field.ReferenceParam2));
            else if (sig.TypeB == typeof(string))
                args.Add(string.Format("'{0}'", Field.StringParameter2));
            else if (sig.TypeB != null)
                args.Add(Field.IntParam2);

            return string.Format("{0}({1})", Field.Function, string.Join(",", args));
        }
    }
}
