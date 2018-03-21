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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization.Json
{
    public class JsonSerializationReader
    {
        readonly JsonTokenReader reader;

        Dictionary<string, IModel> references = new Dictionary<string, IModel>();
        StackManager stack = new StackManager();

        public JsonSerializationReader(Stream stream)
        {
            reader = new JsonTokenReader(stream);
        }

        public IEnumerable<GameModel> ReadModels(Func<GameModel> method)
        {
            // Build dom tree from tokens
            var tree = JsonTreeBuilder.BuildJsonTree(reader.ReadTokens());

            // Read array using all tokens in the source stream
            return ReadArray(tree, method).ToArray();
        }

        /// <summary>
        /// Gets the current object type name.
        /// </summary>
        /// <returns></returns>
        public string CurrentObjectTypeName
        {
            get { return stack.ObjectType; }
        }

        /// <summary>
        /// Reads string value from a property.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns></returns>
        public string ReadPropertyString(string name)
        {
            var prop = stack.Object.GetPropertyString(name);
            return prop != null ? prop.Text : null;
        }

        /// <summary>
        /// Reads int value from a property.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns></returns>
        public int ReadPropertyInt32(string name)
        {
            string value = ReadPropertyString(name);
            int parsed;
            if (value != null && int.TryParse(value, out parsed))
            {
                return parsed;
            }
            else
            {
                return default(int);
            }
        }

        /// <summary>
        /// Reads short value from a property.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns></returns>
        public short ReadPropertyInt16(string name)
        {
            string value = ReadPropertyString(name);
            short parsed;
            if (value != null && short.TryParse(value, out parsed))
            {
                return parsed;
            }
            else
            {
                return default(short);
            }
        }

        /// <summary>
        /// Reads bool value from a property.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns></returns>
        public bool ReadPropertyBoolean(string name)
        {
            string value = ReadPropertyString(name);
            if (value != null && value.Equals("true"))
            {
                return true;
            }
            else
            {
                return default(bool);
            }
        }

        /// <summary>
        /// Reads object from a property using the provided method.
        /// </summary>
        /// <typeparam name="TModel">Type of the model object to read.</typeparam>
        /// <param name="name">Name of the property.</param>
        /// <param name="method">Method to read the model object from the property.</param>
        /// <returns></returns>
        public TModel ReadPropertyObject<TModel>(string name, Func<TModel> method) where TModel : IModel
        {
            var obj = stack.Object.GetPropertyObject(name);
            if (obj != null)
            {
                return ReadObject(obj, method);
            }
            else
            {
                return default(TModel);
            }
        }

        /// <summary>
        /// Reads objects from the array from a property using the provided method to read each model object.
        /// </summary>
        /// <typeparam name="TModel">Type of the model objects to read.</typeparam>
        /// <param name="name">Name of the property.</param>
        /// <param name="method">Method to read the model objects from the array from the property.</param>
        /// <returns></returns>
        public IEnumerable<TModel> ReadPropertyArray<TModel>(string name, Func<TModel> method) where TModel : IModel
        {
            var array = stack.Object.GetPropertyArray(name);
            if (array != null)
            {
                return ReadArray(array, method);
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<TModel> ReadArray<TModel>(JsonArray array, Func<TModel> method) where TModel : IModel
        {
            return array.Select(m => ReadObject(m, method));
        }

        private TModel ReadObject<TModel>(JsonObject obj, Func<TModel> method) where TModel : IModel
        {
            if (obj.HasProperty("ref"))
            {
                // Retrieve model from reference map
                var path = obj.GetPropertyString("ref").Text;

                if (!references.ContainsKey(path))
                {
                    throw new InvalidDataException(string.Format("Referenced object '{0}' not found.", path));
                }

                if (references[path] is TModel)
                {
                    //Log.Fine("Referenced object '{0}' found.", path);
                    return (TModel)references[path];
                }
                else
                {
                    throw new InvalidDataException(string.Format("Referenced object '{0}' of type '{1}' found but it is not the expected type {2}.", path, references[path].GetType().Name, typeof(TModel).Name));
                }
            }
            else if (obj.HasProperty("type") && obj.HasProperty("value"))
            {
                var type = obj.GetPropertyString("type").Text;
                var actualObject = obj.GetPropertyObject("value");

                //Log.Fine("Reading object of type '{0}'", type);

                stack.Enter(type, actualObject);
                TModel model = method();
                stack.Leave();

                var path = obj.GetPropertyString("path");
                if (path != null)
                {
                    // Add model with path to reference map
                    references.Add(path.Text, model);
                }

                return model;
            }
            else
            {
                throw new InvalidDataException("Expected 'type' and 'value' or 'ref' property to specify the object.");
            }
        }

        class StackManager
        {
            Stack<StackItem> stack = new Stack<StackItem>();

            public string ObjectType { get { return stack.Peek().ObjectType; }}
            public JsonObject Object { get { return stack.Peek().Object; } }
            public string Path { get { return GetStackPath(); } }
            public int Depth { get { return stack.Count; } }

            public void Enter(string objectType, JsonObject obj)
            {
                stack.Push(new StackItem(objectType, obj));
            }

            public void Leave()
            {
                stack.Pop();
            }

            private string GetStackPath()
            {
                return string.Join("/", stack.Reverse().Select(i => i.ToString()));
            }

            public override string ToString()
            {
                return GetStackPath();
            }
        }

        class StackItem
        {
            public string ObjectType { get; private set; }
            public JsonObject Object { get; private set; }

            public StackItem(string objectType, JsonObject obj)
            {
                ObjectType = objectType;
                Object = obj;
            }

            public override string ToString()
            {
                return ObjectType;
            }
        }
    }
}
