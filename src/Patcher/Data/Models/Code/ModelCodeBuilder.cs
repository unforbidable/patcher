// Copyright(C) 2015,1016,2017,2018 Unforbidable Works
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
using Patcher.Data.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Code
{
    public class ModelCodeBuilder
    {
        public CodeBase BuildModels(IEnumerable<GameModel> models)
        {
            var code = new CodeBase();
            code.Using.Add("System");
            code.Using.Add("System.Collections.Generic");

            // General namespace
            var ns = new CodeNamespace("Patcher.Data.Models");
            code.Namespaces.Add(ns);

            var context = new Context(code);

            // Prepare a namespace for each game model
            foreach (var model in models)
            {
                BuildGameNamespace(context, model);
            }

            return code;
        }

        private void BuildGameNamespace(Context context, GameModel model)
        {
            string nsName = string.Format("Patcher.Data.Models.{0}", model.Name);
            var ns = new CodeNamespace(nsName)
            {
                Comment = string.Format("Data model for {0}", model.Name)
            };
            context.Code.Namespaces.Add(ns);
            context.EnterNamespace(ns);

            foreach (var e in model.Models.OfType<EnumModel>())
            {
                BuildEnum(context, e);
            }

            foreach (var r in model.Models.OfType<RecordModel>())
            {
                BuildRecordClass(context, r);
            }

            foreach (var s in model.Models.OfType<StructModel>())
            {
                BuildStructClass(context, s);
            }
        }

        private void BuildRecordClass(Context context, RecordModel model)
        {
            var comment = new StringBuilder();
            comment.AppendLine(string.Format("[Key(\"{0}\")]", model.Key.ToUpper()));
            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                comment.AppendLine(string.Format("[DisplayName(\"{0}\")]", model.DisplayName));
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var cls = new CodeClass(model.Name)
            {
                Comment = comment.ToString()
            };

            context.Namespace.Types.Add(cls);

            context.EnterType(cls);
            foreach (var field in model.Fields)
            {
                BuildFieldProperty(context, field);
                context.CurrentMemberIndex++;
            }
            context.LeaveType();
        }

        private void BuildFieldGroupClass(Context context, FieldGroupModel model)
        {
            bool embedded = context.CurrentClass != null;

            var comment = new StringBuilder();
            if (!embedded && !string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var cls = new CodeClass(model.Name);
            cls.Comment = comment.ToString();

            if (context.CurrentClass != null)
            {
                // Create private nested class
                cls.Modifiers = CodeModifiers.Private;
                context.CurrentClass.Types.Add(cls);
            }
            else
            {
                context.Namespace.Types.Add(cls);
            }

            context.EnterType(cls);
            foreach (var field in model.Fields)
            {
                BuildFieldProperty(context, field);
                context.CurrentMemberIndex++;
            }
            context.LeaveType();
        }

        private void BuildStructClass(Context context, StructModel model)
        {
            var comment = new StringBuilder();
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var cls = new CodeClass(model.Name);
            cls.Comment = comment.ToString();
            context.Namespace.Types.Add(cls);

            context.EnterType(cls);
            if (model.IsUnion)
            {
                cls.Extends.Add(GetUnionType(model));
                foreach (var m in model.Members)
                {
                    BuildUnionMemberProperty(context, m);
                    context.CurrentMemberIndex++;
                }
            }
            else
            {
                foreach (var m in model.Members)
                {
                    BuildMemberProperty(context, m);
                    context.CurrentMemberIndex++;
                }
            }
            context.LeaveType();
        }

        private void BuildUnionMemberProperty(Context context, MemberModel model)
        {
            // Create property for a member of union
            var prop = new CodeProperty(GetMemberOuterTypeName(model), model.Name);
            prop.Getter = new CodePropertyAccessor("return value" + context.CurrentMemberIndex + ";");
            context.CurrentClass.Members.Add(prop);
        }

        private void BuildFieldProperty(Context context, FieldModel model)
        {
            string fieldName = model.Name ?? "Unused" + context.CurrentMemberIndex;

            if (!model.IsVirtual)
            {
                // Create field holding the data (non-virtual fields)
                var field = new CodeField(GetFieldInnerTypeName(model), GetFieldName(fieldName));
                context.CurrentClass.Members.Add(field);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                // Create property - either as target type or same as field type (fields with name only)
                var prop = new CodeProperty(GetFieldOuterTypeName(model), fieldName);
                prop.Getter = new CodePropertyAccessor("return " + GetFieldName(fieldName) + ";");
                if (!model.IsArray)
                {
                    // List property does not have a setter
                    prop.Setter = new CodePropertyAccessor(GetFieldName(fieldName) + " = value;");
                }
                context.CurrentClass.Members.Add(prop);
            }
        }

        private void BuildMemberProperty(Context context, MemberModel model)
        {
            string fieldName = model.Name ?? "Unused" + context.CurrentMemberIndex;

            if (!model.IsVirtual)
            {
                // Create field holding the data (non-virtual fields)
                var field = new CodeField(GetMemberInnerTypeName(model), GetFieldName(fieldName));
                context.CurrentClass.Members.Add(field);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                // Create property - either as target type or same as field type (members with name only)
                var prop = new CodeProperty(GetMemberOuterTypeName(model), fieldName);
                prop.Getter = new CodePropertyAccessor("return " + GetFieldName(fieldName) + ";");
                if (!model.IsArray)
                {
                    // List property does not have a setter
                    prop.Setter = new CodePropertyAccessor(GetFieldName(fieldName) + " = value;");
                }
                context.CurrentClass.Members.Add(prop);
            }
        }

        private string GetUnionType(StructModel model)
        {
            var builder = new StringBuilder("Variable<");
            builder.Append(string.Join(", ", model.Members.Select(m => m.TargetModel != null ? GetTargetTypeName(m.TargetModel) : GetTypeName(m.Type.Name, m.IsArray, m.ArrayLength))));
            builder.Append(">");
            return builder.ToString();
        }

        private string GetFieldInnerTypeName(FieldModel model)
        {
            return GetTypeName(model.Type.Name, model.IsArray || model.IsList, model.ArrayLength);
        }

        private string GetMemberInnerTypeName(MemberModel model)
        {
            return GetTypeName(model.Type.Name, model.IsArray, model.ArrayLength);
        }

        private string GetFieldOuterTypeName(FieldModel model)
        {
            return model.TargetModel != null ? GetTargetTypeName(model.TargetModel) : GetFieldInnerTypeName(model);
        }

        private string GetMemberOuterTypeName(MemberModel model)
        {
            return model.TargetModel != null ? GetTargetTypeName(model.TargetModel) : GetMemberInnerTypeName(model);
        }

        private string GetTargetTypeName(TargetModel target)
        {
            if (target.Type is FormReference)
            {
                return GetTypeName(GetFormReferenceTypeName(target.Type as FormReference), target.IsArray, target.ArrayLength);
            }
            else
            {
                return GetTypeName(target.Type.Name, target.IsArray, target.ArrayLength);
            }
        }

        private string GetTypeName(string type, bool isList, int listSize)
        {
            if (isList)
            {
                // List of byte and list of fixed size is always an array
                if (type == "byte" || listSize > 0)
                {
                    return type + "[]";
                }
                else
                {
                    return "List<" + type + ">";
                }
            }
            else
            {
                return type;
            }
        }

        private string GetFormReferenceTypeName(FormReference reference)
        {
            if (reference.FormTypes.Length == 1)
            {
                // One form type specified, use this one type
                string oneFormType = reference.FormTypes[0];
                return "I" + char.ToUpper(oneFormType[0]) + oneFormType.Substring(1).ToLower();
            }
            else
            {
                // Any or multiple types specified, use generic form type reference
                return "IForm";
            }
        }

        private string GetFieldName(string name)
        {
            return "field" + name;
        }

        private void BuildEnum(Context context, EnumModel model)
        {
            var comment = new StringBuilder();
            if (model.IsFlags)
            {
                comment.AppendLine("[Flags]");
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var e = new CodeEnum(model.Name);
            e.Comment = comment.ToString();
            e.Type = model.BaseType ?? typeof(int);
            context.Namespace.Types.Add(e);

            context.EnterType(e);
            foreach (var m in model.Members)
            {
                BuildEnumMember(context, m);
                context.CurrentMemberIndex++;
            }
            context.LeaveType();
        }

        private void BuildEnumMember(Context context, EnumMemberModel model)
        {
            var comment = new StringBuilder();
            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                comment.AppendLine(string.Format("[DisplayName(\"{0}\")]", model.DisplayName));
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            context.CurrentEnum.Members.Add(new CodeEnumMember(model.Name, model.Value)
            {
                Comment = comment.ToString()
            });
        }

        class Context
        {
            List<IModel> embedded = new List<IModel>();
            Stack<StackItem> stack = new Stack<StackItem>();

            public CodeBase Code { get; private set; }
            public CodeNamespace Namespace { get; private set; }

            public CodeClass CurrentClass { get { return stack.Count > 0 ? stack.Peek().Type as CodeClass : null; } }
            public CodeClass TopLevelClass { get { return stack.LastOrDefault().Type as CodeClass; } }
            public CodeEnum CurrentEnum { get { return stack.Count > 0 ? stack.Peek().Type as CodeEnum : null; } }
            public int CurrentMemberIndex { get { return stack.Count > 0 ? stack.Peek().MemberIndex : 0; } set { stack.Peek().MemberIndex = value; } }

            public List<IModel> Embedded { get { return embedded; } }

            public Context(CodeBase code)
            {
                Code = code;
            }

            public void EnterNamespace(CodeNamespace ns)
            {
                Namespace = ns;
            }

            public void LeaveNamespace()
            {
                Namespace = null;
            }

            public void EnterType(CodeType type)
            {
                stack.Push(new StackItem(type));
            }

            public void LeaveType()
            {
                stack.Pop();
            }

            class StackItem
            {
                public CodeType Type { get; private set; }
                public int MemberIndex { get; set; }

                public StackItem(CodeType type)
                {
                    Type = type;
                }
            }
        }
    }
}
