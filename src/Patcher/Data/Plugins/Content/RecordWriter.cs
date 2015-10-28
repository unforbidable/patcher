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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Patcher.Data.Plugins.Content
{
    public sealed class RecordWriter : IDisposable
    {
        const int initialSegmentMemoryStreamSize = 65536;

        readonly Stream mainStream;
        Stream segmentStream;

        bool closed = false;

        readonly DataContext context;
        public DataContext Context { get { return context; } }

        Stack<SegmentInfo> segmentStack = new Stack<SegmentInfo>();

        Stack<MemoryStream> memoryStreamPool = new Stack<MemoryStream>();

        internal IReferenceMapper ReferenceMapper { get; set; }

        HeaderRecord header = null;

        int numRecords = 0;
        FormKind currentGroupFormKind = FormKind.Any;

        internal RecordWriter(Stream stream, DataContext context)
        {
            this.context = context;
            mainStream = stream;
            segmentStream = stream;
        }

        internal void WriteHeader(HeaderRecord header)
        {
            if (this.header != null)
                throw new InvalidOperationException("Header already written");

            this.header = header;

            DoWriteRecord(header, 0);
        }

        internal void WriteRecord(GenericFormRecord record, uint formId)
        {
            if (header == null)
                throw new InvalidOperationException("Header not yet written");

            FormKind formKind = (FormKind)InfoProvider.GetRecordInfo(record.GetType()).Signature;
            if (formKind != currentGroupFormKind)
            {
                // End existing group if one has begun
                if (currentGroupFormKind != FormKind.Any)
                {
                    EndSegment();
                }

                currentGroupFormKind = formKind;
                BeginGroupSegment(currentGroupFormKind);
                numRecords++;
            }

            // Convert global context Form ID to local relative Form ID
            uint localFormId = ReferenceMapper.ContexToLocal(formId);

            DoWriteRecord(record, localFormId);
            numRecords++;
        }

        private void DoWriteRecord(Record record, uint formId)
        {
            var recinfo = InfoProvider.GetRecordInfo(record.GetType());
            BeginRecordSegment(recinfo.Signature, record.RawFlags, record.Version, formId);

            record.WriteRecord(this);

            EndSegment();
        }

        internal void WriteField(object target, MemberInfo meminfo)
        {
            var value = meminfo.GetValue(target);

            // Skip null fields right away (covers nullable primitive types)
            if (value == null)
                return;

            // Skip null form references (that are not a list of references) unless it is a required field
            if (meminfo.IsReference && !meminfo.IsListType && (uint)value == 0 && !meminfo.IsRequired)
                return;

            string fieldName = meminfo.FieldNames.First();
            if (meminfo.IsPrimitiveType)
            {
                // Primitive types have only one propery name
                if (meminfo.IsListType)
                {
                    foreach (var item in (IEnumerable)value)
                    {
                        BeginPropertySegment(fieldName);
                        WritePrimitiveField(meminfo, item);
                        EndSegment();
                    }
                }
                else
                {
                    BeginPropertySegment(fieldName);
                    WritePrimitiveField(meminfo, value);
                    EndSegment();
                }
            }
            else
            {
                if (meminfo.FieldType.IsSubclassOf(typeof(Compound)))
                {
                    if (meminfo.IsListType)
                    {
                        foreach (var item in (IEnumerable)value)
                        {
                            WriteCompoundField((Compound)item);
                        }
                    }
                    else
                    {
                        WriteCompoundField((Compound)value);
                    }
                }
                else
                {
                    if (meminfo.IsListType)
                    {
                        foreach (var item in (IEnumerable)value)
                        {
                            WriteStructuredField((Field)item, fieldName);
                        }
                    }
                    else
                    {
                        WriteStructuredField((Field)value, fieldName);
                    }
                }
            }
        }

        private void WriteStructuredField(Field field, string fieldName)
        {
            if (field.CanWriteField())
            {
                // Each structured field in it's own segment
                BeginPropertySegment(fieldName);
                field.WriteField(this);
                EndSegment();
            }
        }

        private void WriteCompoundField(Compound compound)
        {
            if (compound.CanWriteField())
            {
                // Compund property will begin segment for each of its fields
                compound.WriteField(this);
            }
        }

        private void WritePrimitiveField(MemberInfo meminf, object value)
        {
            if (meminf.FieldType == typeof(string))
            {
                WriteStringZeroTerminated((string)value);
            }
            else if (meminf.FieldType == typeof(int))
            {
                Write((int)value);
            }
            else if (meminf.FieldType == typeof(uint))
            {
                if (meminf.IsReference)
                    WriteReference((uint)value, meminf.ReferencedFormKinds);
                else
                    Write((uint)value);
            }
            else if (meminf.FieldType == typeof(long))
            {
                Write((long)value);
            }
            else if (meminf.FieldType == typeof(ulong))
            {
                Write((ulong)value);
            }
            else if (meminf.FieldType == typeof(short))
            {
                Write((short)value);
            }
            else if (meminf.FieldType == typeof(ushort))
            {
                Write((ushort)value);
            }
            else if (meminf.FieldType == typeof(char))
            {
                segmentStream.WriteByte(Convert.ToByte((char)value));
            }
            else if (meminf.FieldType == typeof(byte))
            {
                segmentStream.WriteByte((byte)value);
            }
            else if (meminf.FieldType == typeof(float))
            {
                if (meminf.IsFakeFloat)
                {
                    Write((int)((float)value * 100));
                }
                else
                {
                    Write((float)value);
                }
            }
            else
            {
                throw new InvalidProgramException("Unsupported primitive field type: " + meminf.FieldType.FullName);
            }
        }

        public void WriteReference(uint formId, FormKindSet referencedFormKinds)
        {
            // All fields should write references via this method so problems can be detected
            if (formId == 0)
            {
                // Null references are normal, do not warn
                //Log.Warning("Writting null reference 0x{0:X8}.", formId);
            }
            else if (!context.Forms.Contains(formId))
            {
                Log.Warning("Writting unresolved form reference 0x{0:X8}.", formId);
            }
            else if (!referencedFormKinds.IsAny)
            {
                // Verify correct reference type
                // if specified reference type is not FormType.None
                var form = context.Forms[formId];
                if (!referencedFormKinds.Contains(form.FormKind))
                {
                    Log.Warning("Writting reference to {0} used where olny references to forms of following types should be: {1}", form, referencedFormKinds);
                }
            }

            var localFormId = ReferenceMapper.ContexToLocal(formId);
            Write(localFormId);
        }

        private void BeginPropertySegment(string signature)
        {
            BeginSegment(new FieldMetadata()
            {
                Signature = signature
            });
        }

        private void BeginRecordSegment(string signature, uint flags, ushort version, uint formId)
        {
            BeginSegment(new RecordMetadata()
            {
                Signature = signature,
                Flags = flags,
                Version = version,
                FormId = formId
            });
        }

        private void BeginGroupSegment(FormKind formKind)
        {
            BeginSegment(new GroupMetadata()
            {
                Signature = "GRUP",
                GroupType = GroupType.Top,
                FormKind = formKind
            });
        }

        private void BeginSegment(Metadata metaData)
        {
            MemoryStream memoryStream;
            if (memoryStreamPool.Count > 0)
            {
                // Reuse old memory stream, reset the position
                memoryStream = memoryStreamPool.Pop();
                memoryStream.SetLength(0);
            }
            else
            {
                memoryStream = new MemoryStream(initialSegmentMemoryStreamSize);
            }

            segmentStack.Push(new SegmentInfo()
            {
                MetaData = metaData,
                Stream = memoryStream
            });

            // New memory stream will be used as the current stream
            segmentStream = memoryStream;
        }

        private void EndSegment()
        {
            if (segmentStack.Count == 0)
                throw new InvalidOperationException("There is no more segments to end");

            var segment = segmentStack.Pop();

            // Reset current stream to the top of the stack (or main stream if there are no more segments)
            segmentStream = segmentStack.Count > 0 ? segmentStack.Peek().Stream : mainStream;

            // Update segment length and write metadata
            segment.MetaData.Length = segment.Stream.Length;
            WriteStringFixedLength(segment.MetaData.Signature);
            segment.MetaData.WriteMetaData(this);

            // Write last segment stream to the current segment stream (or the main stream if there are no more segments)
            segment.Stream.Position = 0;
            segment.Stream.CopyTo(segmentStream);

            // Put segment memory stream to the pool
            memoryStreamPool.Push(segment.Stream);
        }

        public void WriteStringFixedLength(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            segmentStream.Write(bytes, 0, bytes.Length);
        }

        public void WriteStringZeroTerminated(string value)
        {
            WriteStringFixedLength(value);
            segmentStream.WriteByte(0);
        }

        public void Write(byte value)
        {
            segmentStream.WriteByte(value);
        }

        public void Write(short value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public void Write(ushort value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 2);
        }

        public void Write(int value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 4);
         }

        public void Write(uint value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public void Write(long value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 8);
        }

        public void Write(ulong value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 8);
        }

        public void Write(float value)
        {
            segmentStream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public void Write(byte[] bytes)
        {
            segmentStream.Write(bytes, 0, bytes.Length);
        }

        public void Close()
        {
            if (closed)
                throw new InvalidOperationException("Writer has been closed already.");

            // End last group if one has begun
            if (currentGroupFormKind != FormKind.Any)
            {
                EndSegment();
            }

            // Verify no more segments
            if (segmentStack.Count > 0)
            {
                //throw new InvalidProgramException("Not all segments have been closed");
                Log.Warning("Record writer has been disposed without every segment having been closed.");
            }

            // Set number of records and write header again
            header.NumRecords = numRecords;
            mainStream.Position = 0;
            DoWriteRecord(header, 0);

            closed = true;
        }

        public void Dispose()
        {
            if (!closed)
            {
                // Close if not explicitly closed
                Close();
            }

            mainStream.Dispose();
            while (segmentStack.Count > 0)
                segmentStack.Pop().Stream.Dispose();
        }

        class SegmentInfo
        {
            public string Signature { get; set; }
            public Metadata MetaData { get; set; }
            public MemoryStream Stream { get; set; }
        }
    }
}
