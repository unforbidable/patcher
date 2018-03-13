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

using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Validation
{
    public class ModelValidator
    {
        List<ModelValidatorIssue> issues = new List<ModelValidatorIssue>();
        Stack<StackItem> modelStack = new Stack<StackItem>();

        public static ModelValidatorIssueCollection ValidateGameModel(GameModel model)
        {
            var validator = new ModelValidator();
            validator.ValidateFirst(model);
            return new ModelValidatorIssueCollection(validator.issues);
        }

        public void AssertWithError(bool condition, string text)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Error, text, GetStackPath()));
            }
        }

        public void AssertWithError(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Error, string.Format(format, args), GetStackPath()));
            }
        }

        public void AssertWithWarning(bool condition, string text)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Warning, text, GetStackPath()));
            }
        }

        public void AssertWithWarning(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Warning, string.Format(format, args), GetStackPath()));
            }
        }

        public void AssertWithNotice(bool condition, string text)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Notice, text, GetStackPath()));
            }
        }

        public void AssertWithNotice(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                issues.Add(new ModelValidatorIssue(ModelValidatorIssueType.Notice, string.Format(format, args), GetStackPath()));
            }
        }

        private void ValidateFirst(GameModel model)
        {
            // Game model validation
            ValidateModel(model);

            if (modelStack.Count > 0)
            {
                throw new InvalidOperationException("Validator model stack is invalid.");
            }
        }

        private void ValidateModelCollection<TModel>(IEnumerable<TModel> models) where TModel : IModel
        {
            Enter();
            List<string> modelNamesInUse = new List<string>();
            foreach (var model in models)
            {
                // Validate names are unique in the collection if models implement INamed
                if (model is INamed)
                {
                    string name = (model as INamed).Name;
                    if (!string.IsNullOrEmpty(name))
                    {
                        AssertWithError(!modelNamesInUse.Contains(name), "Name '{0}' already used by a sibling model.", name);
                        modelNamesInUse.Add(name);
                    }
                }

                // Validate model itself
                ValidateModel(model);

                CurrentIndex++;
            }
            Leave();
        }

        private void ValidateModel<TModel>(TModel model) where TModel : IModel
        {
            // Retrieve model name via INamed interface
            string name = model is INamed ? (model as INamed).Name : null;
            Enter(name ?? "<annonymous-object>");

            if (model is IValidable)
            {
                // Validate model itself if it implements IValidable
                (model as IValidable).ValidateModel(this);
            }

            // Depending on the type of model being validated, validate its child models and collections and eventually descendant models
            if (model is GameModel)
            {
                Enter("Models");
                ValidateModelCollection((model as GameModel).Models);
                Leave();
            }
            else if (model is RecordModel)
            {
                Enter("Fields");
                ValidateModelCollection((model as RecordModel).Fields);
                Leave();
            }
            else if (model is FieldModel)
            {
                Enter("Group");
                ValidateModel((model as FieldModel).FieldGroup);
                Leave();
                Enter("Struct");
                ValidateModel((model as FieldModel).Struct);
                Leave();
                Enter("Target");
                ValidateModel((model as FieldModel).TargetModel);
                Leave();
            }
            else if (model is FieldGroupModel)
            {
                Enter("Fields");
                ValidateModelCollection((model as FieldGroupModel).Fields);
                Leave();
            }
            else if (model is StructModel)
            {
                Enter("Members");
                ValidateModelCollection((model as StructModel).Members);
                Leave();
            }
            else if (model is EnumModel)
            {
                Enter("Members");
                ValidateModelCollection((model as EnumModel).Members);
                Leave();
            }
            else if (model is FunctionModel)
            {
                Enter("Params");
                ValidateModelCollection((model as FunctionModel).Params);
                Leave();
            }
            else if (model is MemberModel)
            {
                Enter("Target");
                ValidateModel((model as MemberModel).TargetModel);
                Leave();
            }

            Leave();
        }

        private void Enter()
        {
            Enter(string.Empty);
        }

        private void Enter(string text)
        {
            modelStack.Push(new StackItem(text));
        }

        private void Leave()
        {
            modelStack.Pop();
        }

        private string CurrentText { get { return modelStack.Peek().Text; } }
        private int CurrentIndex { get { return modelStack.Peek().Index; } set { modelStack.Peek().Index = value; } }

        private string GetStackPath()
        {
            return string.Join("/", modelStack.Reverse().Select(s => !string.IsNullOrEmpty(s.Text) ? s.Text : string.Format("[{0}]", s.Index)));
        }

        class StackItem
        {
            public string Text { get; private set; }
            public int Index { get; set; }

            public StackItem(string text)
            {
                Text = text;
            }
        }

    }
}
