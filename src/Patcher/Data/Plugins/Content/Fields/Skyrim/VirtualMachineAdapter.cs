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

using Patcher.Data.Plugins.Content.Enums.Skyrim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class VirtualMachineAdapter : Field
    {
        internal short Version { get; private set; }
        internal short Format { get; private set; }

        List<Script> scripts = new List<Script>();
        public List<Script> Scripts { get { return scripts; } }

        internal override void ReadField(RecordReader reader)
        {
            Version = reader.ReadInt16(); // 5
            Format = reader.ReadInt16(); // 2

            if (Format != 2 || Version != 5)
            {
                Log.Warning("Unexpected version {0} or format {1}", Version, Format);
            }
            
            short numberOfAttchedScripts = reader.ReadInt16();
            while (numberOfAttchedScripts-- > 0)
            {
                Script script = new Script();
                script.ReadScript(reader);
                scripts.Add(script);
            }
        }

        internal override void WriteField(RecordWriter writer)
        {
            throw new NotImplementedException();
        }

        public override Field CopyField()
        {
            return new VirtualMachineAdapter()
            {
                Version = Version,
                Format = Format,
                scripts = new List<Script>(scripts.Select(s => s.CopyScript())) // Create list containing copies of each script in original list
            };
        }

        public override bool Equals(Field other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("Scripts={0}", scripts.Count);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            throw new NotImplementedException();
        }

        public sealed class Script
        {
            public string Name { get; set; }
            public ScriptType Type { get; set; }

            public List<ScriptProperty> Properties { get { return properties; } }
            List<ScriptProperty> properties = new List<ScriptProperty>();

            public void ReadScript(RecordReader reader)
            {
                ushort scriptNameLength = reader.ReadUInt16();
                Name = reader.ReadStringZeroTerminated();
                byte numberOfProperties = reader.ReadByte();
                Type = (ScriptType)reader.ReadByte();

                while (numberOfProperties-- > 0)
                {
                    ScriptProperty property = new ScriptProperty();
                    property.ReadProperty(reader);
                    properties.Add(property);
                }
            }

            public override string ToString()
            {
                return string.Format("Script=\"{0}\"", Name);
            }

            public Script CopyScript()
            {
                return new Script()
                {
                    Name = Name,
                    Type = Type,
                    properties = new List<ScriptProperty>(properties.Select(p => p.CopyScriptProperty()))
                };
            }
        }

        public sealed class ScriptProperty
        {
            public string Name { get; set; }
            public ScriptPropertyType Type { get; set; }
            public ScriptPropertyFlags Flags { get; set; }

            public ScriptPropertyValue Value { get; private set; }

            public void ReadProperty(RecordReader reader)
            {
                ushort propertyNameLength = reader.ReadUInt16();
                Name = reader.ReadStringFixedLength(propertyNameLength);
                Type = (ScriptPropertyType)reader.ReadByte();
                Flags = (ScriptPropertyFlags)reader.ReadByte();

                Value = CreateValueInstance(Type);
                Value.ReadValue(reader);
            }

            private ScriptPropertyValue CreateValueInstance(ScriptPropertyType type)
            {
                switch (type)
                {
                    case ScriptPropertyType.Object:
                        return new ScriptPropertyValueObject();

                    case ScriptPropertyType.String:
                        return new ScriptPropertyValueString();

                    case ScriptPropertyType.Int:
                        return new ScriptPropertyValueInt();

                    case ScriptPropertyType.Float:
                        return new ScriptPropertyValueFloat();

                    case ScriptPropertyType.Bool:
                        return new ScriptPropertyValueBool();

                    case ScriptPropertyType.ArrayOfObject:
                        return new ScriptPropertyValueArrayOf<ScriptPropertyValueObject>();

                    case ScriptPropertyType.ArrayOfString:
                        return new ScriptPropertyValueArrayOf<ScriptPropertyValueString>();

                    case ScriptPropertyType.ArrayOfInt:
                        return new ScriptPropertyValueArrayOf<ScriptPropertyValueInt>();

                    case ScriptPropertyType.ArrayOfFloat:
                        return new ScriptPropertyValueArrayOf<ScriptPropertyValueFloat>();

                    case ScriptPropertyType.ArrayOfBool:
                        return new ScriptPropertyValueArrayOf<ScriptPropertyValueBool>();

                    default:
                        throw new InvalidDataException("Encountered unknown script property type: " + type);
                }
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", Name, Value);
            }

            public ScriptProperty CopyScriptProperty()
            {
                return new ScriptProperty()
                {
                    Name = Name,
                    Type = Type,
                    Flags = Flags,
                    Value = Value.CopyValue()
                };
            }
        }

        public abstract class ScriptPropertyValue
        {
            public abstract void ReadValue(RecordReader reader);
            public abstract ScriptPropertyValue CopyValue();
        }

        public sealed class ScriptPropertyValueArrayOf<T> : ScriptPropertyValue where T: ScriptPropertyValue, new()
        {
            List<T> list = new List<T>();
            public List<T> List { get { return list; } }

            public override void ReadValue(RecordReader reader)
            {
                int count = reader.ReadInt32();
                while (count-- > 0)
                {
                    T value = new T();
                    value.ReadValue(reader);
                    list.Add(value);
                }
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueArrayOf<T>()
                {
                    list = new List<T>(list.Select(i => (T)i.CopyValue()))
                };
            }

            public override string ToString()
            {
                return string.Format("Count={0}{1}", list.Count,
                    list.Count > 0 ? string.Format(" {0}", string.Join(" ", list)) : "");
            }
        }

        public sealed class ScriptPropertyValueInt : ScriptPropertyValue
        {
            public int Value { get; set; }

            public override void ReadValue(RecordReader reader)
            {
                Value = reader.ReadInt32();
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueInt()
                {
                    Value = Value
                };
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public sealed class ScriptPropertyValueFloat : ScriptPropertyValue
        {
            public float Value { get; set; }

            public override void ReadValue(RecordReader reader)
            {
                Value = reader.ReadSingle();
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueFloat()
                {
                    Value = Value
                };
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public sealed class ScriptPropertyValueBool : ScriptPropertyValue
        {
            public bool Value { get; set; }

            public override void ReadValue(RecordReader reader)
            {
                Value = reader.ReadByte() == 1;
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueBool()
                {
                    Value = Value
                };
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public sealed class ScriptPropertyValueString : ScriptPropertyValue
        {
            public string Value { get; set; }

            public override void ReadValue(RecordReader reader)
            {
                ushort length = reader.ReadUInt16();
                Value = reader.ReadStringFixedLength(length);
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueString()
                {
                    Value = Value
                };
            }

            public override string ToString()
            {
                return Value;
            }
        }

        public sealed class ScriptPropertyValueObject : ScriptPropertyValue
        {
            public short Unknown { get; set; }
            public short Alias { get; set; }
            public uint Object { get; set; }

            public override void ReadValue(RecordReader reader)
            {
                Unknown = reader.ReadInt16();
                Alias = reader.ReadInt16();
                Object = reader.ReadReference(FormKind.None);
            }

            public override ScriptPropertyValue CopyValue()
            {
                return new ScriptPropertyValueObject()
                {
                    Unknown = Unknown,
                    Alias = Alias,
                    Object = Object
                };
            }

            public override string ToString()
            {
                return string.Format("Alias={0} {1}", Alias, Object);
            }
        }
    }
}
