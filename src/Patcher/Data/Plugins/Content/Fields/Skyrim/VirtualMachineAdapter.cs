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

using Patcher.Data.Plugins.Content.Constants.Skyrim;
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
        public List<Script> Scripts { get; set; }

        public VirtualMachineAdapter()
        {
            Scripts = new List<Script>();
        }

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
                Scripts.Add(script);
            }
        }

        internal override void WriteField(RecordWriter writer)
        {
            writer.Write(Version);
            writer.Write(Format);
            writer.Write((short)Scripts.Count);

            foreach (var script in Scripts)
            {
                script.WriteScript(writer);
            }
        }

        public override Field CopyField()
        {
            return new VirtualMachineAdapter()
            {
                Version = Version,
                Format = Format,
                Scripts = new List<Script>(Scripts.Select(s => s.CopyScript())) // Create list containing copies of each script in original list
            };
        }

        public override bool Equals(Field other)
        {
            var cast = (VirtualMachineAdapter)other;
            return Version == cast.Version && Format == cast.Format && Scripts.SequenceEqual(cast.Scripts);
        }

        public override string ToString()
        {
            return string.Format("Scripts={0}", Scripts.Count);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            // Get references from each script
            return Scripts.SelectMany(s => s.GetReferencedFormIds());
        }

        public sealed class Script : IEquatable<Script>
        {
            public string Name { get; set; }
            public ScriptType Type { get; set; }
            public List<ScriptProperty> Properties { get; set; }

            public Script()
            {
                Properties = new List<ScriptProperty>();
            }

            internal void ReadScript(RecordReader reader)
            {
                // UESP - Contrary to UESP script name IS null terminated in addition to size recorded 
                ushort scriptNameLength = reader.ReadUInt16();
                Name = reader.ReadStringZeroTerminated();
                byte numberOfProperties = reader.ReadByte();
                Type = (ScriptType)reader.ReadByte();

                while (numberOfProperties-- > 0)
                {
                    var property = new ScriptProperty();
                    property.ReadProperty(reader);
                    Properties.Add(property);
                }
            }

            internal void WriteScript(RecordWriter writer)
            {
                // UESP - contrary to UESP write length (excluding null termination) and also null terminated string
                if (string.IsNullOrEmpty(Name))
                {
                    Log.Warning("Writting null or blank script name.");
                    writer.Write((ushort)0);
                    writer.WriteStringZeroTerminated(string.Empty);
                }
                else
                {
                    writer.Write((ushort)Name.Length);
                    writer.WriteStringZeroTerminated(Name);
                }
                writer.Write((byte)Properties.Count);
                writer.Write((byte)Type);

                foreach (var property in Properties)
                {
                    property.WriteProperty(writer);
                }
            }

            public override string ToString()
            {
                return string.Format("Name=\"{0}\"", Name);
            }

            public Script CopyScript()
            {
                return new Script()
                {
                    Name = Name,
                    Type = Type,
                    Properties = new List<ScriptProperty>(Properties.Select(p => p.CopyProperty()))
                };
            }

            internal IEnumerable<uint> GetReferencedFormIds()
            {
                return Properties.SelectMany(p => p.GetReferencedFormIds());
            }

            public bool Equals(Script other)
            {
                return Name == other.Name && Type == other.Type && Properties.SequenceEqual(other.Properties);
            }
        }

        public sealed class ScriptProperty : IEquatable<ScriptProperty>
        {
            static Dictionary<ScriptPropertyType, Type> propertyTypeMap = new Dictionary<ScriptPropertyType, Type>()
            {
                { ScriptPropertyType.Object, typeof(ObjectProperty) },
                { ScriptPropertyType.String, typeof(string) },
                { ScriptPropertyType.Int, typeof(int) },
                { ScriptPropertyType.Float, typeof(float) },
                { ScriptPropertyType.Bool, typeof(byte) },
                { ScriptPropertyType.ArrayOfObject, typeof(ObjectProperty[]) },
                { ScriptPropertyType.ArrayOfString, typeof(string[]) },
                { ScriptPropertyType.ArrayOfInt, typeof(int[]) },
                { ScriptPropertyType.ArrayOfFloat, typeof(float[]) },
                { ScriptPropertyType.ArrayOfBool, typeof(byte[]) },
            };

            public string Name { get; set; }
            public ScriptPropertyType Type { get; set; }
            public ScriptPropertyFlags Flags { get; set; }
            public object Value { get; set; }

            internal void ReadProperty(RecordReader reader)
            {
                ushort propertyNameLength = reader.ReadUInt16();
                Name = reader.ReadStringFixedLength(propertyNameLength);
                Type = (ScriptPropertyType)reader.ReadByte();
                Flags = (ScriptPropertyFlags)reader.ReadByte();

                if (!propertyTypeMap.ContainsKey(Type))
                    throw new InvalidDataException("Unexpected script property type: " + Type);

                var instanceType = propertyTypeMap[Type];
                Value = ReadValue(reader, instanceType);
            }

            private object ReadValue(RecordReader reader, Type type)
            {
                if (type == typeof(ObjectProperty))
                {
                    return new ObjectProperty()
                    {
                        Unknown = reader.ReadUInt16(),
                        AliasId = reader.ReadUInt16(),
                        FormId = reader.ReadReference(FormKind.None)
                    };
                }
                else if (type == typeof(string))
                {
                    ushort len = reader.ReadUInt16();
                    return reader.ReadStringFixedLength(len);
                }
                else if (type == typeof(int))
                {
                    return reader.ReadInt32();
                }
                else if (type == typeof(float))
                {
                    return reader.ReadSingle();
                }
                else if (type == typeof(byte))
                {
                    return reader.ReadByte();
                }
                else if (type.IsArray)
                {
                    var elementType = type.GetElementType();
                    int count = reader.ReadInt32();
                    var array = Array.CreateInstance(elementType, count);
                    for (int i = 0; i < count; i++)
                    {
                        var value = ReadValue(reader, elementType);
                        array.SetValue(value, i);
                    }
                    return array;
                }
                else
                {
                    throw new InvalidDataException("Unexpected script property value instance type: " + type.FullName);
                }
            }

            internal void WriteProperty(RecordWriter writer)
            {
                // Verify Type is compatible with Value.GetType()
                if (!propertyTypeMap.ContainsKey(Type))
                    throw new InvalidDataException("Unexpected script property type: " + Type);

                if (propertyTypeMap[Type] != Value.GetType())
                    throw new InvalidDataException("Unexpected script property value instance type: " + Value.GetType().FullName);

                writer.Write((ushort)Name.Length);
                writer.WriteStringFixedLength(Name);
                writer.Write((byte)Type);
                writer.Write((byte)Flags);
                WriteValue(writer, Value.GetType(), Value);
            }

            private void WriteValue(RecordWriter writer, Type type, object value)
            {
                if (type == typeof(ObjectProperty))
                {
                    var obj = (ObjectProperty)value;
                    writer.Write(obj.Unknown);
                    writer.Write(obj.AliasId);
                    writer.WriteReference(obj.FormId, FormKind.None);
                }
                else if (type == typeof(string))
                {
                    var str = (string)value;
                    writer.Write((ushort)str.Length);
                    writer.WriteStringFixedLength(str);
                }
                else if (type == typeof(int))
                {
                    writer.Write((int)value);
                }
                else if (type == typeof(float))
                {
                    writer.Write((float)value);
                }
                else if (type == typeof(byte))
                {
                    writer.Write((byte)value);
                }
                else if (type.IsArray)
                {
                    Type elementType = type.GetElementType();
                    var array = (Array)Value;
                    writer.Write((ushort)array.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        WriteValue(writer, elementType, array.GetValue(i));
                    }
                }
                else
                {
                    throw new InvalidDataException("Unexpected script property value instance type: " + type.FullName);
                }
            }

            public ScriptProperty CopyProperty()
            {
                return new ScriptProperty()
                {
                    Name = Name,
                    Type = Type,
                    Flags = Flags,
                    Value = CopyValue()
                };
            }

            private object CopyValue()
            {
                var type = Value.GetType();
                if (type.IsArray)
                {
                    // Clone array
                    return ((Array)Value).Clone();
                }
                else
                {
                    // Simply copy value if not array
                    return Value;
                }
            }

            public bool Equals(ScriptProperty other)
            {
                return Name == other.Name && Type == other.Type && Flags == other.Flags && ValueEquals(other);
            }

            private bool ValueEquals(ScriptProperty other)
            {
                var type = Value.GetType();
                if (type.IsArray)
                {
                    // Compare array length first
                    // and then each element
                    var thisArray = (Array)Value;
                    var otherArray = (Array)other.Value;
                    if (thisArray.Length != otherArray.Length)
                        return false;

                    for (int i = 0; i < thisArray.Length; i++)
                    {
                        if (thisArray.GetValue(i) != otherArray.GetValue(i))
                            return false;
                    }

                    return true;
                }
                else
                {
                    // Simply compare value if not array
                    return Value == other.Value;
                }
            }

            internal IEnumerable<uint> GetReferencedFormIds()
            {
                // Only Object and ArrayOfObjects have Form IDs
                if (Type == ScriptPropertyType.Object)
                {
                    yield return ((ObjectProperty)Value).FormId;
                }
                else if (Type == ScriptPropertyType.ArrayOfObject)
                {
                    var array = (Array)Value;
                    for (int i = 0; i < array.Length; i++)
                    {
                        yield return ((ObjectProperty)array.GetValue(i)).FormId;
                    }
                }
            }
        }

        public struct ObjectProperty
        {
            public ushort Unknown { get; set; }
            public ushort AliasId { get; set; }
            public uint FormId { get; set; }
        }
    }
}
