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
            // TODO: More properties to write
            WriteProperty("Models", model.Models.Where(t => t is EnumModel), WriteModel);
        }

        private void WriteModel(EnumModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Description", model.Description);
            WriteProperty("BaseType", model.BaseType.Name);
            WriteProperty("IsFlags", model.IsFlags);
            WriteProperty("Members", model.Members, WriteModel);
        }

        private void WriteModel(EnumMemberModel model)
        {
            WriteProperty("Name", model.Name);
            WriteProperty("Value", model.Value);
        }

        // TODO: More models to write

        private void WriteModel(IModel model)
        {
            if (model is EnumModel)
            {
                WriteModel(model as EnumModel);
            }
            else
            {
                throw new NotImplementedException(string.Format("Serialization of model object {0} not implemented.", model.GetType().Name));
            }
        }

        // Write string property
        private void WriteProperty(string name, string value)
        {
            WritePropertyName(name);
            Write(string.Format("\"{0}\"", value));
        }

        // Write int property
        private void WriteProperty(string name, int value)
        {
            WritePropertyName(name);
            Write(string.Format("{0}", value));
        }

        // Write bool propery
        private void WriteProperty(string name, bool value)
        {
            WritePropertyName(name);
            Write(string.Format("{0}", value.ToString().ToLower()));
        }

        // Write property that is an array of model objects, specifying the method used to write each model object
        private void WriteProperty<TModel>(string name, IEnumerable<TModel> models, Action<TModel> writeObjectAction) where TModel : IModel
        {
            WritePropertyName(name);
            WriteArray(models, writeObjectAction);
        }

        private void WritePropertyName(string name)
        {
            stack.Name = name;
            if (stack.Index++ > 0)
            {
                WriteLine(", ");
            }
            else
            {
                WriteLine(" ");
            }
            Write("{0}: ", name);
        }

        private void WriteArray<TModel>(IEnumerable<TModel> models, Action<TModel> writeObjectAction) where TModel : IModel
        {
            Write("[");
            stack.Enter();
            foreach (var model in models)
            {
                if (stack.Index++ > 0)
                {
                    Write(", ");
                }
                else
                {
                    WriteLine(" ");
                }
                WriteObject(model, writeObjectAction);
            }
            stack.Leave();
            WriteLine(" ");
            Write("]");
        }

        private void WriteObject<TModel>(TModel model, Action<TModel> writeObjectAction) where TModel : IModel
        {
            Write("{");
            stack.Enter(true);
            WriteProperty("path", stack.Path);
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
