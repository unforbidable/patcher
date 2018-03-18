// Copyright(C) 2018 Unforbidable Works
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
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization
{
    public class ModelSerializationWriter
    {
        readonly TextWriter writer;
        readonly StackManager stack = new StackManager();

        Dictionary<IModel, string> references = new Dictionary<IModel, string>();

        public bool Pretty { get; set; }

        public ModelSerializationWriter(Stream stream) : this(new StreamWriter(stream))
        {
        }

        public ModelSerializationWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void WriteModels(IEnumerable<GameModel> models)
        {
            WriteArray(models, WriteModel);
            writer.Flush();
        }

        private void WriteModel(GameModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("BasePlugin", model.BasePlugin);
            WriteProperty("LatestFormVersion", model.LatestFormVersion);
            WriteProperty("PluginsFileLocation". model.PluginsFileLocation);
            WriteProperty("PluginsMatchLine", model.PluginsMatchLine);
            WriteProperty("ArchivesExtension", model.ArchivesExtension);
            WriteProperty("StringsDefaultLanguage", model.StringsDefaultLanguage);
            WriteProperty("Models", model.Models, WriteRootModel);
        }

        private void WriteRootModel(IModel model)
        {
            if (model is EnumModel)
            {
                WriteModel(model as EnumModel);
            }
            else if (model is StructModel)
            {
                WriteModel(model as StructModel);
            }
            else if (model is RecordModel)
            {
                WriteModel(model as RecordModel);
            }
            else if (model is FieldGroupModel)
            {
                WriteModel(model as FieldGroupModel);
            }
            else if (model is FunctionModel)
            {
                WriteModel(model as FunctionModel);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unexpected root model {0}", model.GetType().Name));
            }
        }

        private void WriteModel(EnumModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Description", model.Description);
            WriteProperty("BaseType", model.BaseType.Name); // Write only .Name for types
            WriteProperty("IsFlags", model.IsFlags);
            WriteProperty("Members", model.Members, WriteModel); // Array property - pass in the method used to write the model in the array
        }

        private void WriteModel(EnumMemberModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Value", model.Value);
        }

        private void WriteModel(MemberModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("DisplayName", model.DisplayName);
            WriteProperty("Description", model.Description);
            WriteProperty("Type", model.Type.Name);
            WritePropertyAsReference("TargetModel", model.TargetModel);
            WriteProperty("IsHidden", model.IsHidden);
            WriteProperty("IsVirtual", model.IsVirtual);
            WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                WriteProperty("ArrayLength", model.ArrayLength);
                WriteProperty("ArrayPrefixSize", model.ArrayPrefixSize);
            }

            WriteProperty("Type", model.Type.Name);
            if (model.IsStruct)
            {
                WriteProperty("Type", model.Struct, WriteModel);
            }
            else if (model.IsMemberType)
            {
                WriteProperty("Type", model.MemberType, WriteModel);
            }
        }

        private void WriteModel(FunctionModel model)
        {
            WriteProperty("Index", model.Index);
            WriteProperty("Name", model.Name);
            WriteProperty("Description", model.Description);
            WriteProperty("Params", model.Params, WriteModel);
        }

        private void WriteModel(FunctionParamModel model)
        {
            WriteProperty("DisplayName", model.DisplayName);
            WriteProperty("Type", model.Type.Name);
            if (model.IsFunctionParamType)
            {
                WriteProperty("Type", model.FunctionParamType, WriteModel);
            }
            else if (model.IsEnumeration)
            {
                WriteProperty("Type", model.Emumeration, WriteModel);
            }
            else if (model.IsFormReference)
            {
                WriteProperty("Type", model.FormReference, WriteModel);
            }
        }

        private void WriteModel(StructModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Description", model.Description);
            WriteProperty("Members", model.Members, WriteModel);
        }

        private void WriteModel(FieldModel model)
        {
            WriteProperty("Key", model.Key);
            WriteProperty("Name", model.Name);
            WriteProperty("DisplayName", model.DisplayName);
            WriteProperty("Description", model.Description);
            WritePropertyAsReference("TargetModel", model.TargetModel);
            WriteProperty("IsHidden", model.IsHidden);
            WriteProperty("IsVirtual", model.IsVirtual);
            WriteProperty("IsList", model.IsList);
            WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                WriteProperty("ArrayLength", model.ArrayLength);
            }

            WriteProperty("Type", model.Type.Name);
            if (model.IsStruct)
            {
                WriteProperty("Type", model.Struct, WriteModel);
            }
            else if (model.IsFieldGroup)
            {
                WriteProperty("Type", model.FieldGroup, WriteModel);
            }
            else if (model.IsMember)
            {
                WriteProperty("Type", model.MemberType, WriteModel);
            }
        }

        private void WriteModel(FieldGroupModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Description", model.Description);
            WriteProperty("Fields", model.Fields, WriteModel);
        }

        private void WriteModel(RecordModel model)
        {
            WriteProperty("Key", model.Key);
            WriteProperty("Name", model.Name);
            WriteProperty("DisplayName", model.DisplayName);
            WriteProperty("Description", model.Description);
            WriteProperty("Fields", model.Fields, WriteModel); // Array property - pass in the method used to write the model in the array
        }

        private void WriteModel(TargetModel model)
        {
            WriteProperty("Type", model.Type.Name);
            WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                WriteProperty("ArrayLength", model.ArrayLength);
            }
        }

        private void WriteModel(ISerializableAsId model)
        {
            WriteProperty("Id", model.Id);
        }

        // Write IModel property as reference (structs, enums, field groups, targets)
        private void WritePropertyAsReference<TModel>(string name, TModel model) where TModel : IModel
        {
            if (model != null)
            {
                WritePropertyName(name);
                WriteObjectReference(model);
            }
        }

        // Write IModel property as object (field, member)
        private void WriteProperty<TModel>(string name, TModel model, Action<TModel> writeObjectAction) where TModel : IModel
        {
            if (model != null)
            {
                WritePropertyName(name);
                WriteObject(model, writeObjectAction);
            }
        }

        // Write string property
        private void WriteProperty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WritePropertyName(name);
                Write(string.Format("\"{0}\"", value));
            }
        }

        // Write int property
        private void WriteProperty(string name, int value)
        {
            if (value != null)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value));
            }
        }

        // Write short property
        private void WriteProperty(string name, short value)
        {
            if (value != null)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value));
            }
        }

        // Write bool propery
        private void WriteProperty(string name, bool value)
        {
            if (value)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value.ToString().ToLower()));
            }
        }

        // Write property that is an array of model objects, specifying the method used to write each model object
        private void WriteProperty<TModel>(string name, IEnumerable<TModel> models, Action<TModel> writeObjectAction) where TModel : IModel
        {
            if (models != null)
            {
                WritePropertyName(name);
                WriteArray(models, writeObjectAction);
            }
        }

        private void WritePropertyName(string name)
        {
            stack.Name = name;
            if (stack.Index > 0)
            {
                WriteLine(", ");
            }
            else
            {
                WriteLine(" ");
            }
            Write("\"{0}\": ", name);
            stack.Index++;
        }

        private void WriteArray<TModel>(IEnumerable<TModel> models, Action<TModel> writeObjectAction) where TModel : IModel
        {
            Write("[");
            stack.Enter();
            foreach (var model in models)
            {
                if (stack.Index > 0)
                {
                    Write(", ");
                }
                else
                {
                    WriteLine(" ");
                }
                WriteObject(model, writeObjectAction);
                stack.Index++;
            }
            stack.Leave();
            WriteLine(" ");
            Write("]");
        }

        private void WriteObjectReference<TModel>(TModel model) where TModel : IModel
        {
            // Find object reference path
            string path;
            if (!references.TryGetValue(model, out path))
            {
                path = string.Format("ERROR - reference to model object not {0} found", model is INamed ? (model as INamed).Name : model.GetType().Name);
            }

            Write("{");
            stack.Enter(true);
            WriteProperty("ref", path);
            stack.Leave();
            Write("}");
        }

        private void WriteObject<TModel>(TModel model, Action<TModel> writeObjectAction) where TModel : IModel
        {
            // Add only root models path to reference map
            if (stack.Depth == 4)
            {
                references.Add(model, stack.Path);
            }

            Write("{");
            stack.Enter(true);
            if (stack.Depth == 5)
            {
                WriteProperty("path", stack.Path);
            }
            WriteProperty("type", model.GetType().Name);
            WritePropertyName("value");
            Write("{");
            stack.Enter();
            writeObjectAction(model);
            stack.Leave();
            WriteLine(" ");
            WriteLine("} ");
            stack.Leave();
            Write("}");
        }

        bool newLine = true;

        private void Write(string format, params string[] args)
        {
            Write(string.Format(format, args));
        }

        private void Write(string text)
        {
            if (newLine && Pretty)
                writer.Write(new string(' ', stack.Depth * 2));

            writer.Write(text);
            newLine = false;
        }

        private void WriteLine()
        {
            WriteLine(string.Empty);
        }

        private void WriteLine(string format, params string[] args)
        {
            WriteLine(string.Format(format, args));
        }

        private void WriteLine(string text)
        {
            Write(text);
            newLine = true;

            if (Pretty)
                writer.WriteLine();
        }

        class StackManager
        {
            Stack<StackItem> stack = new Stack<StackItem>();

            public string Name { get { return stack.Peek().Name; } set { stack.Peek().Name = value; } }
            public int Index { get { return stack.Peek().Index; } set { stack.Peek().Index = value; } }
            public string Path { get { return GetStackPath(); } }
            public int Depth { get { return stack.Count; } }

            public void Enter()
            {
                Enter(false);
            }

            public void Enter(bool skipFromPath)
            {
                stack.Push(new StackItem(skipFromPath));
            }

            public void Leave()
            {
                stack.Pop();
            }

            private string GetStackPath()
            {
                return string.Join("/", stack.Reverse().Where(i => !i.SkipFromPath).Select(i => i.ToString()));
            }

            public override string ToString()
            {
                return GetStackPath();
            }
        }

        class StackItem
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public bool SkipFromPath { get; set; }

            public StackItem(bool skipFromPath)
            {
                SkipFromPath = skipFromPath;
            }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name;
                }
                else
                {
                    return string.Format("[{0}]", Index);
                }
            }
        }
    }
}
