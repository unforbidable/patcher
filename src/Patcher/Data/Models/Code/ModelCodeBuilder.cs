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
                CreateGameNamespace(context, model);
            }

            return code;
        }

        private void CreateGameNamespace(Context context, GameModel model)
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
                CreateEnum(context, e);
            }

            foreach (var s in model.Models.OfType<StructModel>())
            {
                CreateStruct(context, s);
            }
        }

        private void CreateEnum(Context context, EnumModel model)
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

            var e = new CodeEnum(model.Name)
            {
                Comment = comment.ToString(),
                Type = model.BaseType ?? typeof(int)
            };
            context.Namespace.Types.Add(e);

            context.EnterType(e);
            foreach (var m in model.Members)
            {
                CreateEnumMember(context, m);
                context.CurrentMemberIndex++;
            }
            context.LeaveType();
        }

        private void CreateEnumMember(Context context, EnumMemberModel model)
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

        private void CreateStruct(Context context, StructModel model)
        {
            var comment = new StringBuilder();
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var c = new CodeClass(model.Name)
            {
                Comment = comment.ToString(),
            };
            context.Namespace.Types.Add(c);

            context.EnterType(c);
            foreach (var m in model.Members)
            {
                CreateStructMembers(context, m);
                context.CurrentMemberIndex++;
            }
            context.LeaveType();
        }

        private void CreateStructMembers(Context context, MemberModel model)
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
            if (model.IsHidden)
            {
                comment.AppendLine("[Hidden]");
            }

            if (model.IsVirtual)
            {
            }
            else
            {
                string type = model.Type.Name;
                if (model.IsArray)
                    type = "List<" + type + ">";

                string name = !string.IsNullOrEmpty(model.Name) ? model.Name : string.Format("Unused{0}", context.CurrentMemberIndex);

                context.CurrentClass.Members.Add(new CodeField(type, name)
                {
                    Comment = comment.ToString()
                });
            }
        }

        class Context
        {
            Stack<StackItem> stack = new Stack<StackItem>();

            public CodeBase Code { get; private set; }
            public CodeNamespace Namespace { get; private set; }

            public CodeClass CurrentClass { get { return stack.Peek().Type as CodeClass; } }
            public CodeClass TopLevelClass { get { return stack.LastOrDefault().Type as CodeClass; } }
            public CodeEnum CurrentEnum { get { return stack.Peek().Type as CodeEnum; } }
            public int CurrentMemberIndex { get { return stack.Peek().MemberIndex; } set { stack.Peek().MemberIndex = value; } }

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
