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
using Patcher.Data.Plugins.Content.Functions.Skyrim;
using Patcher.Data.Plugins;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(ICondition))]
    public sealed class ConditionProxy : FieldProxy<Condition>, ICondition, IDumpabled
    {
        const uint PlayerRefFormId = 0x00000014;

        public ICondition AndNext()
        {
            ClearFlag(ConditionFlags.Or);
            return this;
        }

        public ICondition IsEqualTo(IForm glob)
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
            return IsNotEqualTo(1);
        }

        public ICondition IsGreaterThan(IForm glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.GreaterThan);
            return this;
        }

        public ICondition IsGreaterThan(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.GreaterThan);
            return this;
        }

        public ICondition IsGreaterThanOrEqualTo(IForm glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.GreaterThanOrEqual);
            return this;
        }

        public ICondition IsGreaterThanOrEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.GreaterThanOrEqual);
            return this;
        }

        public ICondition IsLessThen(IForm glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.LessThen);
            return this;
        }

        public ICondition IsLessThen(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.LessThen);
            return this;
        }

        public ICondition IsLessThenOrEqualTo(IForm glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.LessThenOrEqual);
            return this;
        }

        public ICondition IsLessThenOrEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.LessThenOrEqual);
            return this;
        }

        public ICondition IsNotEqualTo(IForm glob)
        {
            UseGlobal(glob);
            UseOperator(ConditionFlags.NotEqual);
            return this;
        }

        public ICondition IsNotEqualTo(float value)
        {
            UseFloat(value);
            UseOperator(ConditionFlags.NotEqual);
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

        public ICondition RunOnCombatTarget()
        {
            Field.FunctionTarget = FunctionTarget.CombatTarget;
            Field.FunctionTargetReference = 0;
            return this;
        }

        public ICondition RunOnSubject()
        {
            Field.FunctionTarget = FunctionTarget.Subject;
            Field.FunctionTargetReference = 0;
            return this;
        }

        public ICondition RunOnTarget()
        {
            Field.FunctionTarget = FunctionTarget.Target;
            Field.FunctionTargetReference = 0;
            return this;
        }

        public ICondition RunOnPlayer()
        {
            Field.FunctionTarget = FunctionTarget.Reference;
            Field.FunctionTargetReference = PlayerRefFormId;
            return this;
        }

        public ICondition SwapSubjectAndTarget()
        {
            SetFlag(ConditionFlags.SwapSubjectAndTarget);
            return this;
        }

        private void UseGlobal(IForm glob)
        {
            if (glob is IGlob)
            {
                Field.GlobalVariableOperand = glob.ToFormId();
                SetFlag(ConditionFlags.UseGlobal);
            }
            else
            {
                Log.Warning("Expected global variable form kind, form {0} cannot be used.", glob);
            }
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

        bool[] parameterSet = new bool[] { false, false };

        /// <summary>
        /// Issues a warning when not all formal parameters required by the current function have been set.
        /// </summary>
        /// <returns></returns>
        internal ConditionProxy CheckParams()
        {
            var signature = Field.FunctionSignature;
            for (int i = 0; i < 2; i++)
            {
                if (signature[i].IsDefined && !parameterSet[i])
                    Log.Warning("The {0} parameter of function {1}() has never been set.", (i == 0 ? "first" : "second"), Field.Function);
            }
            return this;
        }

        /// <summary>
        /// Assigns a string to one of the function parameters.
        /// </summary>
        /// <param name="index">Index of the parameter to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal ConditionProxy SetParam(int index, string value)
        {
            parameterSet[index] = true;
            Field.SetStringParam(index, value);
            return this;
        }

        /// <summary>
        /// Assigns an integer value to one of the function parameters.
        /// </summary>
        /// <param name="index">Index of the parameter to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal ConditionProxy SetParam(int index, int value)
        {
            parameterSet[index] = true;
            Field.SetIntParam(index, value);
            return this;
        }

        /// <summary>
        /// Assigns a float value to one of the function parameters.
        /// </summary>
        /// <param name="index">Index of the parameter to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal ConditionProxy SetParam(int index, float value)
        {
            parameterSet[index] = true;
            Field.SetFloatParam(index, value);
            return this;
        }

        /// <summary>
        /// Assigns a form reference to one of the function parameters.
        /// </summary>
        /// <param name="index">Index of the parameter to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal ConditionProxy SetParam(int index, IForm value)
        {
            if (value == null)
            {
                Log.Warning("A null form is being used as argument {0} in function {1}().", index, Field.Function);
            }
            else
            {
                // Verify form kind
                var sig = Field.FunctionSignature;
                if (sig[index].IsReference)
                {
                    var formKindSet = sig[index].Reference;
                    if (!formKindSet.IsAny)
                    {
                        // Parameter requires a specific kind (or kinds) of form
                        // Determine which kind of form the argument is
                        // Cast to proxy to access the FormKind property
                        var proxy = (FormProxy)value;
                        if (!formKindSet.Contains(proxy.Form.FormKind))
                            Log.Warning("Form {0} is being used as argument where only the following kinds are valid: {1}", value, formKindSet);
                    }
                }
            }

            parameterSet[index] = true;
            Field.SetReferenceParam(index, value.ToFormId());
            return this;
        }

        /// <summary>
        /// Attempts to assign a value of unknown type to one of the function parameters.
        /// </summary>
        /// <param name="index">Index of the parameter to set.</param>
        /// <param name="value">Value to set.</param>
        /// <returns></returns>
        internal ConditionProxy SetParam(int index, object value)
        {
            var signature = Field.FunctionSignature;

            var valueAsformProxy = value as FormProxy;
            var valueAstext = value as string;

            bool success = false;

            if (signature[index].IsDefined)
            {
                if (signature[index].IsReference)
                {
                    // If value is null, take it as a null reference
                    if (value == null || valueAsformProxy != null)
                    {
                        SetParam(index, valueAsformProxy);
                        success = true;
                    }
                    else
                    {
                        Log.Warning("Expected a form references as the argument {0} of function {1}().", index, Field.Function);
                    }
                }
                else if (signature[index].PlainType == typeof(string))
                {
                    if (valueAstext != null)
                    {
                        SetParam(index, valueAstext);
                        success = true;
                    }
                    else
                    {
                        Log.Warning("Expected a string as the argument {0} of function {1}().", index, Field.Function);
                    }
                }
                else if (signature[index].PlainType == typeof(float))
                {
                    try
                    {
                        // Try to convert int and string even to int
                        float number = Convert.ToSingle(value);
                        SetParam(index, number);
                        success = true;
                    }
                    catch (Exception)
                    {
                        Log.Warning("Expected a float value as the argument {0} of function {1}().", index, Field.Function);
                    }
                }
                else if (signature[index].PlainType == typeof(int) || signature[index].PlainType.IsEnum && signature[index].PlainType.GetEnumUnderlyingType() == typeof(int))
                {
                    try
                    {
                        // Try to convert enums (based on int), float and string even to int
                        int integer = Convert.ToInt32(value);
                        SetParam(index, integer);
                        success = true;
                    }
                    catch (Exception)
                    {
                        Log.Warning("Expected an integer value as the argument {0} of function {1}().", index, Field.Function);
                    }
                }
                else
                {
                    throw new InvalidProgramException("Unsupported " + index + " formal parameter type in function definition " + Field.Function + "(): " + signature[index].PlainType.Name);
                }
            }
            else
            {
                Log.Warning("Unexpected arguemnt {0} of function {1}().", index, Field.Function);
            }

            if (!success)
            {
                // Value did not patch the formal parameter type
                // this may be the case when the function signature has not been defined
                // In this case a warning has been issued and the value will be forced to the appropriate slot
                // as long as it is that of an expected type.

                if (valueAsformProxy != null)
                {
                    // Assume form reference
                    SetParam(index, valueAsformProxy);
                    Log.Warning("The rgument {0} of function {1}() assumed to be a form reference.", index, Field.Function);
                }
                else if (valueAstext != null)
                {
                    // Assume string
                    SetParam(index, valueAstext);
                    Log.Warning("The argument {0} of function {1}() assumed to be a string.", index, Field.Function);
                }
                else
                {
                    if (value != null && value.GetType() == typeof(float))
                    {
                        // Assume float
                        SetParam(index, (float)value);
                        Log.Warning("The argument {0} of function {1}() assumed to be a float.", index, Field.Function);
                    }
                    else
                    {
                        // Try convert anything to int
                        try
                        {
                            int integer = Convert.ToInt32(value);
                            SetParam(index, integer);
                            Log.Warning("The argument {0} of function {1}() assumed to be an integer.", index, Field.Function);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException("The type of the argument " + index + " of function " + Field.Function + " could not be inferred from the value " + value + ".");
                        }
                    }
                }
            }

            return this;
        }

        void IDumpabled.Dump(ObjectDumper dumper)
        {
            dumper.DumpText("Function", FunctionToString());

            if (Field.Flags.HasFlag(ConditionFlags.UseGlobal))
            {
                dumper.DumpObject("Operand", Provider.CreateReferenceProxy<GlobProxy>(Field.GlobalVariableOperand));
            }
            else
            {
                dumper.DumpObject("Operand", Field.FloatOperand);
            }

            if (Field.FunctionTarget == FunctionTarget.Reference && Field.FunctionTargetReference == PlayerRefFormId)
            {
                dumper.DumpText("RunOn", "Player");
            }
            else
            {
                dumper.DumpObject("RunOn", Field.FunctionTarget);
                if (Field.FunctionTarget == FunctionTarget.Reference || Field.FunctionTarget == FunctionTarget.LinkedReference)
                {
                    dumper.DumpObject("RunOnReference", Provider.CreateReferenceProxy<FormProxy>(Field.FunctionTargetReference));
                }
            }

            dumper.DumpObject("Flags", Field.Flags);
        }

        private string FunctionToString()
        {
            var output = new List<object>(2);

            var sig = Field.FunctionSignature;

            for (int i = 0; i < Signature.MaxParams; i++)
            {
                if (sig[i].IsReference)
                {
                    output.Add(Provider.CreateFormProxy<FormProxy>(Field.GetReferenceParam(i), ProxyMode.Referenced));
                }
                else if (sig[i].IsString)
                {
                    output.Add(string.Format("'{0}'", Field.GetStringParam(i)));
                }
                else if (sig[i].IsInt)
                {
                    output.Add(Field.GetIntParam(i));
                }
                else if (sig[i].IsFloat)
                {
                    output.Add(Field.GetFloatParam(i));
                }
            }

            return string.Format("{0}({1})", Field.Function, string.Join(",", output));
        }
    }
}
