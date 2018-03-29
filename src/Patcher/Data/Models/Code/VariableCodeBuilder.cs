// Copyright(C) 2015,2016,2017,2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Code;
using Patcher.Code.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Code
{
    public class VariableCodeBuilder
    {
        const int MaxArguments = 10;

        public CodeBase BuildVariable()
        {
            var code = new CodeBase();
            code.Using.Add("System");
            var ns = new CodeNamespace("Patcher.Data.Models");
            code.Namespaces.Add(ns);

            ns.Types.Add(BuildAbstractClass());

            for (int n = 2; n <= MaxArguments; n++)
            {
                ns.Types.Add(BuildImplementingClass(n));
            }

            return code;
        }

        private CodeClass BuildAbstractClass()
        {
            // Create abstract base class
            var cls = new CodeClass("Variable");
            cls.Modifiers = CodeModifiers.Public | CodeModifiers.Abstract;

            cls.Members.Add(new CodeProperty("int", "Index")
            {
                Getter = new CodePropertyAccessor(),
                Setter = new CodePropertyAccessor() { Modifiers = CodeModifiers.Private }
            });
            cls.Members.Add(new CodeProperty("Type", "Type")
            {
                Getter = new CodePropertyAccessor(),
                Setter = new CodePropertyAccessor() { Modifiers = CodeModifiers.Private }
            });

            var ctorBody = new CodeBuilder();
            ctorBody.AppendLine("Index = index;");
            ctorBody.AppendLine("Type = type;");
            cls.Members.Add(new CodeMethod(null, cls.Name)
            {
                Modifiers = CodeModifiers.Protected,
                Parameters = "int index, Type type",
                Body = ctorBody
            });

            var getObjectBody = new CodeBuilder();
            getObjectBody.AppendLine("if (Index != index)");
            getObjectBody.EnterBlock();
            getObjectBody.AppendLine("throw new InvalidOperationException(string.Format(\"The current value index is {0} and value index {1} cannot be retrieved.\", Index, index));");
            getObjectBody.LeaveBlock();
            getObjectBody.AppendLine("else");
            getObjectBody.EnterBlock();
            getObjectBody.AppendLine("return DoGetValue(index);");
            getObjectBody.LeaveBlock();
            cls.Members.Add(new CodeMethod("object", "GetObject") {
                Modifiers = CodeModifiers.Private,
                Parameters = "int Index",
                Body = getObjectBody
            });

            var asBody = new CodeBuilder();
            asBody.AppendLine("EnsureTypeMatch(typeof(T));");
            asBody.AppendLine("return (T)GetValue(Index);");
            cls.Members.Add(new CodeMethod("T", "As<T>")
            {
                Modifiers = CodeModifiers.Public,
                Body = asBody
            });

            var covertToBody = new CodeBuilder();
            covertToBody.AppendLine("return (T)Convert.ChangeType(GetValue(Index), typeof(T));");
            cls.Members.Add(new CodeMethod("T", "ConvertTo<T>")
            {
                Modifiers = CodeModifiers.Public,
                Body = covertToBody
            });

            var doGetObjectBody = new CodeBuilder();
            doGetObjectBody.AppendLine("throw new NotImplementedException(string.Format(\"Getting value index {0} not implemented\", index));");
            cls.Members.Add(new CodeMethod("object", "GetDoObject")
            {
                Modifiers = CodeModifiers.Protected | CodeModifiers.Virtual,
                Parameters = "int Index",
                Body = doGetObjectBody
            });

            var ensureTypeMatchBody = new CodeBuilder();
            ensureTypeMatchBody.AppendLine("if (!assignedType.IsAssignableFrom(Type))");
            ensureTypeMatchBody.EnterBlock();
            ensureTypeMatchBody.AppendLine("throw new InvalidOperationException(string.Format(\"The current value type {0} cannot be assigned to type {1}.\", Type.FullName, assignedType.FullName));");
            cls.Members.Add(new CodeMethod("void", "EnsureTypeMatch")
            {
                Modifiers = CodeModifiers.Protected,
                Parameters = "Type type",
                Body = ensureTypeMatchBody
            });

            var toStringBody = new CodeBuilder();
            toStringBody.AppendLine("return GetValue(Index).ToString();");
            cls.Members.Add(new CodeMethod("string", "ToString")
            {
                Modifiers = CodeModifiers.Public | CodeModifiers.Override,
                Body = toStringBody
            });

            return cls;
        }

        private CodeType BuildImplementingClass(int genericArgumentCount)
        {
            string clsName = "Variable";
            string clsNameWithArguments = clsName + "<" + string.Join(", ", Enumerable.Range(1, genericArgumentCount).Select(i => "T" + i)) + ">";
            var cls = new CodeClass(clsNameWithArguments);
            cls.Extends.Add(clsName);

            for (int i = 1; i <= genericArgumentCount; i++)
            {
                int index = i - 1;
                string typeName = "T" + i;
                string fieldName = "value" + i;
                cls.Members.Add(new CodeField(typeName, fieldName));

                var ctorBody = new CodeBuilder();
                ctorBody.AppendLine(fieldName + " = value;");
                cls.Members.Add(new CodeMethod(null, clsName) {
                    Modifiers = CodeModifiers.Private,
                    Parameters = typeName + " value",
                    Body = ctorBody,
                    ConstructorInvocation = "base(" + index + ", typeof(" + typeName + "))"
                });

                var assignValueBody = new CodeBuilder();
                assignValueBody.AppendLine("variable.EnsureTypeMatch(typeof(" + typeName + "));");
                assignValueBody.AppendLine("return variable." + fieldName + ";");
                cls.Members.Add(new CodeMethod(typeName, null)
                {
                    Modifiers = CodeModifiers.Public | CodeModifiers.Static | CodeModifiers.Implicit | CodeModifiers.Operator,
                    Parameters = clsNameWithArguments + " variable",
                    Body = assignValueBody
                });

                var assignVariableBody = new CodeBuilder();
                assignVariableBody.AppendLine("return new " + clsNameWithArguments + "(value);");
                cls.Members.Add(new CodeMethod(clsNameWithArguments, null)
                {
                    Modifiers = CodeModifiers.Public | CodeModifiers.Static | CodeModifiers.Implicit | CodeModifiers.Operator,
                    Parameters = typeName + " variable",
                    Body = assignVariableBody
                });
            }

            var doGetValueBody = new CodeBuilder();
            doGetValueBody.AppendLine("switch (index)");
            doGetValueBody.AppendLine("{");
            doGetValueBody.EnterBlock();

            for (int i = 1; i <= genericArgumentCount; i++)
            {
                doGetValueBody.AppendLine("case " + (i - 1) + ": return value" + i + ";");
            }

            doGetValueBody.LeaveBlock();
            doGetValueBody.AppendLine("}");
            doGetValueBody.AppendLine("return base.DoGetValue(index);");
            cls.Members.Add(new CodeMethod("object", "DoGetValue")
            {
                Modifiers = CodeModifiers.Protected | CodeModifiers.Override,
                Parameters = "int index",
                Body = doGetValueBody
            });

            return cls;
        }
    }
}
