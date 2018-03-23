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

using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization.Json
{
    public class JsonSerializationWriter
    {
        readonly TextWriter writer;
        readonly bool pretty;

        StackManager stack = new StackManager();
        Dictionary<IModel, string> references = new Dictionary<IModel, string>();

        public JsonSerializationWriter(Stream stream, bool pretty)
        {
            writer = new StreamWriter(stream);
            this.pretty = pretty;
        }

        /// <summary>
        /// Initiate writing game models.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="method">Method that will be called to write the properties of each GameModel object.</param>
        public void WriteModels(IEnumerable<GameModel> models, Action<GameModel> method)
        {
            WriteArray(models, method);
            writer.Flush();
        }

        /// <summary>
        /// Write model object property as reference. This method should be used for models StructModel, FieldGroupModel and EnumModel that are not root models.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="name"></param>
        /// <param name="model"></param>
        public void WriteProperty<TModel>(string name, TModel model) where TModel : IModel
        {
            if (model != null)
            {
                WritePropertyName(name);
                WriteObjectReference(model);
            }
        }

        /// <summary>
        /// Write model object property as object. This method should be used when writing MemberModel and FieldModel and also when writting root models.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="method">Method that will be called to write the properties of the model object.</param>
        public void WriteProperty<TModel>(string name, TModel model, Action<TModel> method) where TModel : IModel
        {
            if (model != null)
            {
                WritePropertyName(name);
                WriteObject(model, method);
            }
        }

        /// <summary>
        /// Write property as string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteProperty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WritePropertyName(name);
                // Escape \ and "
                value = value.Replace("\\", "\\\\").Replace("\"", "\\\"");
                Write(string.Format("\"{0}\"", value));
            }
        }

        /// <summary>
        /// Write property as Int32.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteProperty(string name, int value)
        {
            if (value != 0)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value));
            }
        }

        /// <summary>
        /// Write property as Int16.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteProperty(string name, short value)
        {
            if (value != 0)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value));
            }
        }

        /// <summary>
        /// Write property as Boolean.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteProperty(string name, bool value)
        {
            if (value)
            {
                WritePropertyName(name);
                Write(string.Format("{0}", value.ToString().ToLower()));
            }
        }

        /// <summary>
        /// Write property as Type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void WriteProperty(string name, Type value)
        {
            if (value != null)
            {
                WritePropertyName(name);
                Write(string.Format("\"{0}\"", value.FullName));
            }
        }

        public void WriteProperty<TModel>(string name, IEnumerable<TModel> models, Action<TModel> method) where TModel : IModel
        {
            if (models != null)
            {
                WritePropertyName(name);
                WriteArray(models, method);
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

        /// <summary>
        /// Write property that is an array of model objects.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="models"></param>
        /// <param name="method">The method that will be called to write the properties of each model object in the array.</param>
        private void WriteArray<TModel>(IEnumerable<TModel> models, Action<TModel> method) where TModel : IModel
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
                WriteObject(model, method);
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
                throw new InvalidOperationException(string.Format("ERROR - reference to model object {0} not found", model is INamed ? (model as INamed).Name : model.GetType().Name));
            }

            Write("{");
            stack.Enter(true);
            WriteProperty("ref", path);
            stack.Leave();
            Write("}");
        }

        private void WriteObject<TModel>(TModel model, Action<TModel> method) where TModel : IModel
        {
            // Add only root models path to reference map
            bool isRootModel = stack.Depth == 4;

            Write("{");
            stack.Enter(true);
            if (isRootModel)
            {
                references.Add(model, stack.Path);
                WriteProperty("path", stack.Path);
            }
            WriteProperty("type", model.GetType().Name);
            WritePropertyName("value");
            Write("{");
            stack.Enter();
            method(model);
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
            if (newLine && pretty)
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

            if (pretty)
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
