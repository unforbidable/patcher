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

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(ICondition))]
    public sealed class ConditionProxy : FieldProxy<Condition>, ICondition
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
    }
}
