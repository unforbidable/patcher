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

using Patcher.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    public sealed class RecordReader : CustomBinaryReader
    {
        readonly DataContext context;
        public DataContext Context { get { return context; } }

        internal IReferenceMapper ReferenceMapper { get; set; }
        internal IStringLocator StringLocator { get; set; }

        int totalRecordsFound = 0;
        public int TotalRecordsFound { get { return totalRecordsFound; } }

        Stack<SegmentInfo> segmentStack = new Stack<SegmentInfo>();
        internal SegmentInfo CurrentSegment { get { return segmentStack.Count > 0 ? segmentStack.Peek() : null; } }
        int CurrentSegmentDepth { get { return segmentStack.Count; } }

        internal States CurrentState { get; private set; }

        internal PluginFlags PluginFlags { get; set; }

        // DataContext creates instances
        internal RecordReader(Stream stream, DataContext context)
            : base(stream)
        {
            this.context = context;

            CurrentState = States.Ready;
        }

        internal HeaderRecord ReadHeader()
        {
            // Change state to Reading if state is Ready
            ChangeState(States.Reading, s => s == States.Ready);

            // Go to the start of the stream
            if (BaseStream.Position > 0)
            {
                BaseStream.Position = 0;
            }

            // Read header meta data
            RecordMetadata recordMetaData = context.CreateRecordMetaData();
            NextSegment(recordMetaData);

            // Create header, set flags and read the header
            var pluginHeader = context.CreateHeader();
            pluginHeader.RawFlags = recordMetaData.Flags;
            pluginHeader.ReadRecord(this);

            // Keep plugin flags for internal use by the reader
            PluginFlags = (PluginFlags)recordMetaData.Flags;

            EndSegment();

            ChangeState(States.AfterHeader);

            return pluginHeader;
        }

        internal IEnumerable<RecordEntry> FindRecordsAsync()
        {
            foreach (var entry in new AsyncRecordFinder(this, 32))
            {
                yield return entry;
            }
        }

        internal void FindRecords(IFindRecordListener listener)
        {
            // Change state to Indexing only when current state is AfterHeader 
            ChangeState(States.Indexing, s => s == States.AfterHeader);

            FindRecordSharedData sharedData = new FindRecordSharedData()
            {
                Args = new FindRecordListenerArgs(),
                RecordMetaData = context.CreateRecordMetaData(),
                GroupMetaData = context.CreateGroupMetaData()
            };

            FindMainGroupsInternal(listener, sharedData);

            ChangeState(States.Ready);
        }

        private void FindMainGroupsInternal(IFindRecordListener listener, FindRecordSharedData sharedData)
        {
            while (NextSegment(sharedData.GroupMetaData))
            {
                if (context.IgnoredFormKinds.Contains(sharedData.GroupMetaData.FormKind))
                {
                    // Skipt ignored form groups
                    SeekEndOfSegment();
                }
                else
                {
                    totalRecordsFound++;

                    FindRecordsInternal(listener, sharedData, 0);
                }
                EndSegment();

                if (sharedData.Args.Action == FindRecordListenerAction.Cancel)
                {
                    if (CurrentSegment != null)
                        SeekEndOfSegment();
                    break;
                }
            }
        }

        private void FindRecordsInternal(IFindRecordListener listener, FindRecordSharedData sharedData, uint parentRecordFormId)
        {
            while (!IsEndOfSegment)
            {
                if (PeekNextSegmentSignature() == "GRUP")
                {
                    FindInnerGroupsInternal(listener, sharedData, parentRecordFormId);
                }

                if (sharedData.Args.Action == FindRecordListenerAction.Cancel)
                    break;

                while (NextSegment(sharedData.RecordMetaData))
                {
                    totalRecordsFound++;

                    uint currentRecordFormId = sharedData.RecordMetaData.FormId;

                    listener.OnRecordFound(sharedData.RecordMetaData.Signature, sharedData.Args);
                    if (sharedData.Args.Action == FindRecordListenerAction.Read)
                    {
                        RecordEntry entry = sharedData.Args.TargetEntry;

                        entry.FilePosition = sharedData.RecordMetaData.Position;
                        entry.Signature = sharedData.RecordMetaData.Signature;
                        entry.FormId = sharedData.RecordMetaData.FormId;
                        entry.ParentRecordFormId = parentRecordFormId;

                        listener.OnRecordEntry(entry);
                    }
                    else if (sharedData.Args.Action == FindRecordListenerAction.Skip)
                    {
                        // Default to Read after skipping one record
                        sharedData.Args.Action = FindRecordListenerAction.Read;
                    }

                    SeekEndOfSegment();
                    EndSegment();

                    if (sharedData.Args.Action == FindRecordListenerAction.Cancel)
                        break;

                    // Try reading child records
                    if (PeekNextSegmentSignature() == "GRUP")
                    {
                        FindInnerGroupsInternal(listener, sharedData, currentRecordFormId);
                    }
                }

                if (sharedData.Args.Action == FindRecordListenerAction.Cancel)
                {
                    SeekEndOfSegment();
                    break;
                }
            }
        }

        private void FindInnerGroupsInternal(IFindRecordListener listener, FindRecordSharedData sharedData, uint parentRecordFormId)
        {
            while (PeekNextSegmentSignature() == "GRUP")
            {
                NextSegment(sharedData.GroupMetaData);
                totalRecordsFound++;

                FindRecordsInternal(listener, sharedData, parentRecordFormId);

                EndSegment();

                if (sharedData.Args.Action == FindRecordListenerAction.Cancel)
                {
                    SeekEndOfSegment();
                    break;
                }
            }
        }

        //internal void ReadRecord(GenericFormRecord record, bool lazyLoading)
        //{
        //    ChangeState(States.Reading, s => s == States.Indexing);

        //    DoReadRecord(record, lazyLoading);

        //    ChangeState(States.Indexing);
        //}

        internal void ReadRecordAt(long position, GenericFormRecord record, bool lazyLoading)
        {
            ChangeState(States.Reading, s => s != States.Reading);

            if (segmentStack.Count > 0)
            {
                Log.Warning("Cleared unfinished stack left from previous reading");
                segmentStack.Clear();
            }

            BaseStream.Position = position;

            RecordMetadata recordMetaData = context.CreateRecordMetaData();
            NextSegment(recordMetaData);

            record.RawFlags = recordMetaData.Flags;
            record.Version = recordMetaData.Version;

            DoReadRecord(record, lazyLoading);

            // Prevent segment errors due no reading all record content
            // during selective property reading
            if (lazyLoading)
                SeekEndOfSegment();

            EndSegment();

            ChangeState(States.Ready);
        }

        private void DoReadRecord(GenericFormRecord record, bool lazyLoading)
        {
            RecordInfo recinf = InfoProvider.GetRecordInfo(record.GetType());
            if (record.GetType() != typeof(DummyRecord) && recinf.Attribute.Signature != CurrentSegment.Signature)
                throw new InvalidOperationException("Record signature mismatch.");

            if (record.IsRecordCompressed)
            {
                // TODO: Intermediate MomeryStream may not be needed here 
                long decompressedSize = ReadUInt32();
                byte[] compressedData = ReadBytesToEnd();
                Stream compressedStream = new MemoryStream(compressedData);
                Stream deflateStream = new CustomDeflateStream(compressedStream, decompressedSize);
                using (RecordReader deflateReader = context.CreateReader(deflateStream))
                {
                    deflateReader.CurrentState = States.Reading;

                    // Copy flags etc to the deflate reader
                    deflateReader.PluginFlags = PluginFlags;
                    deflateReader.ReferenceMapper = ReferenceMapper;
                    deflateReader.StringLocator = StringLocator;

                    // Read record form the deflate reader
                    record.ReadRecord(deflateReader, lazyLoading);
                }
            }
            else
            {
                // Read record form the current reader
                record.ReadRecord(this, lazyLoading);
            }
        }

        internal IEnumerable<string> FindFields()
        {
            FieldMetadata metaData = context.CreateFieldMetaData();
            while (NextSegment(metaData))
            {
                yield return metaData.Signature;

                EndSegment();
            }
        }

        internal void CancelFindFields()
        {
            EndSegment();
        }

        private void EndSegment()
        {
            SegmentInfo segment = segmentStack.Peek();
            if (segment == null)
                throw new InvalidOperationException("Segment has not been started");

            if (BaseStream.Position > segment.End)
            {
                Log.Error("Too much data read from last segment");
                PrintSegmentStackTrace();
                throw new InvalidProgramException("Too much data read from last segment");
            }
            else if (BaseStream.Position < segment.End)
            {
                Log.Error("Not enough data read from last segment");
                PrintSegmentStackTrace();
                throw new InvalidProgramException("Not enough data read in last segment");
            }

            segmentStack.Pop();
        }

        private void PrintSegmentStackTrace()
        {
            Log.Error("    --- SEGMENT STACK ---");
            foreach (var segment in segmentStack)
            {
                Log.Error("    {0}", segment);
            }
            Log.Error("    --- SEGMENT STACK ---");
        }

        private bool NextSegment(Metadata metaData)
        {
            // End of parent segment or the whole stream
            if (IsEndOfSegment || IsEndOfStream)
                return false;

            if (nextSignaturePeek != null)
            {
                metaData.Position = BaseStream.Position - 4;
                metaData.Signature = nextSignaturePeek;

                nextSignaturePeek = null;
                nextSignaturePeekBytes = 0;
            }
            else
            {
                metaData.Position = BaseStream.Position;
                metaData.Signature = ReadStringFixedLength(4);
            }
            metaData.ReadMetaData(this);

            SegmentInfo segment = new SegmentInfo()
            {
                Signature = metaData.Signature,
                Begin = BaseStream.Position,
                End = BaseStream.Position + metaData.Length,
                Length = metaData.Length,
                MetaData = metaData
            };
            segmentStack.Push(segment);

            return true;
        }

        string nextSignaturePeek = null;
        long nextSignaturePeekBytes = 0;

        private string PeekNextSegmentSignature()
        {
            if (IsEndOfSegment || IsEndOfStream)
                return null;

            if (nextSignaturePeek == null)
            {
                nextSignaturePeek = ReadStringFixedLength(4);
                nextSignaturePeekBytes = 4;
            }

            return nextSignaturePeek;
        }

        public bool IsEndOfSegment
        {
            get { return CurrentSegment != null && CurrentSegment.End <= BaseStream.Position; }
        }

        public bool IsEndOfStream
        {
            get { return BaseStream.Position - nextSignaturePeekBytes == BaseStream.Length; }
        }

        public byte[] ReadBytesToEnd()
        {
            if (CurrentSegment == null)
                throw new InvalidOperationException("Reading to the end of segment when not in segment");

            long bytesRemaining = CurrentSegment.End - BaseStream.Position;
            return ReadBytes((int)bytesRemaining);
        }

        public uint ReadReference(FormKind referencedFormKind)
        {
            uint formId = ReadUInt32();
            formId = ReferenceMapper.LocalToContext(formId);

            if (formId > 0 && !context.Forms.Contains(formId))
            {
                //Log.Warning("Reading unresolved form referenced 0x{0:X8}.", formId);
            }

            // TODO: Verify reference type

            return formId;
        }

        public void Seek(long count)
        {
            BaseStream.Seek(count, SeekOrigin.Current);
        }

        public void SeekEndOfSegment()
        {
            Seek(CurrentSegment.End - BaseStream.Position);
        }

        private void ChangeState(States newState)
        {
            ChangeState(newState, s => true);
        }

        private void ChangeState(States newState, Predicate<States> predicate)
        {
            lock (this)
            {
                if (predicate.Invoke(CurrentState))
                {
                    //Log.Fine("Changing reader state from {0} to {1}", CurrentState, newState);

                    CurrentState = newState;
                }
                else
                {
                    throw new InvalidOperationException("Reader is in a state that does not allow the requested operation");
                }
            }
        }

        internal enum States
        {
            Ready,
            AfterHeader,
            Indexing,
            Reading
        }

        internal class SegmentInfo
        {
            public string Signature { get; set; }
            public long Begin { get; set; }
            public long End { get; set; }
            public long Length { get; set; }
            public Metadata MetaData { get; set; }

            public override string ToString()
            {
                return string.Format("{0} From=0x{1:X8} To=0x{2:X8} Length={3}", Signature, Begin, End, Length);
            }
        }

        internal enum FindRecordListenerAction
        {
            Read,
            Skip,
            Cancel
        }

        internal class FindRecordListenerArgs
        {
            public RecordEntry TargetEntry { get; set; }
            public FindRecordListenerAction Action { get; set; }
        }

        internal interface IFindRecordListener
        {
            void OnRecordFound(string signature, FindRecordListenerArgs args);
            void OnRecordEntry(RecordEntry entry);
        }

        class FindRecordSharedData
        {
            public FindRecordListenerArgs Args { get; set; }
            public RecordMetadata RecordMetaData { get; set; }
            public GroupMetadata GroupMetaData { get; set; }
        }

        Compound[] compoundFields = new Compound[8];

        internal void BeginReadFields(int depth)
        {
            if (depth > 7)
            {
                throw new InvalidOperationException("Compound property nesting is too deep");
            }

            compoundFields[depth] = null;
        }

        internal void ReadField(object target, string fieldName, MemberInfo memberInfo, int depth)
        {
            if (typeof(Compound).IsAssignableFrom(memberInfo.FieldType))
            {
                string firstProperty = memberInfo.FieldNames.First();

                // Create new compound property when
                // - no compound property was started
                // - OR when the property that is being read is the first property of new or current compound property
                if (compoundFields[depth] == null || firstProperty == fieldName)
                {
                    // Inform previous compound property it is complete
                    if (compoundFields[depth] != null)
                    {
                        compoundFields[depth].OnComplete(this, depth + 1);
                    }

                    compoundFields[depth] = (Compound)Context.CreateField(memberInfo.FieldType);
                    compoundFields[depth].OnCreate(this, depth + 1);

                    memberInfo.SetOrAddValue(target, compoundFields[depth]);
                }
                compoundFields[depth].ReadCompoundField(this, fieldName, depth + 1);
            }
            else
            {
                // Inform current compound property it is complete
                // and finish it
                // when non-compund property is being read
                if (compoundFields[depth] != null)
                {
                    compoundFields[depth].OnComplete(this, depth + 1);
                }

                memberInfo.SetOrAddValue(target, ReadFieldValue(memberInfo));
            }
        }

        private object ReadFieldValue(MemberInfo memberInfo)
        {
            if (!memberInfo.IsPrimitiveType)
            {
                Field property = Context.CreateField(memberInfo.FieldType);
                property.ReadField(this);
                return property;
            }
            else if (memberInfo.FieldType == typeof(string))
            {
                if (memberInfo.LocalizedStringGroup != LocalizedStringGroups.None && PluginFlags.HasFlag(PluginFlags.Localized))
                {
                    uint index = ReadUInt32();
                    return GetLocalizedString(memberInfo.LocalizedStringGroup, (index));
                }
                else
                {
                    return ReadStringZeroTerminated();
                }
            }
            else if (memberInfo.FieldType == typeof(int))
            {
                return ReadInt32();
            }
            else if (memberInfo.FieldType == typeof(uint))
            {
                if (memberInfo.IsReference)
                    return ReadReference(memberInfo.ReferencedFormKind);
                else
                    return ReadUInt32();
            }
            else if (memberInfo.FieldType == typeof(ulong))
            {
                return ReadUInt64();
            }
            else if (memberInfo.FieldType == typeof(char))
            {
                return (char)ReadByte();
            }
            else if (memberInfo.FieldType == typeof(float))
            {
                if (memberInfo.IsFakeFloat)
                {
                    return (float)ReadInt32() / 100;
                }
                else
                {
                    return ReadSingle();
                }
            }
            else
            {
                throw new InvalidProgramException("Unsupported primitive field type: " + memberInfo.FieldType.FullName);
            }
        }

        public string GetLocalizedString(LocalizedStringGroups group, uint index)
        {
            if (index == 0)
                return string.Empty;

            switch (group)
            {
                case LocalizedStringGroups.Strings:
                    return StringLocator.GetString(index);

                case LocalizedStringGroups.DLStrings:
                    return StringLocator.GetDLString(index);

                case LocalizedStringGroups.ILStrings:
                    return StringLocator.GetILString(index);

                default:
                    throw new ArgumentException("Illegal localized string group: " + group);
            }
        }

        internal void EndReadFields(int depth)
        {
            // Inform last compund property it is complete
            if (compoundFields[depth] != null)
            {
                compoundFields[depth].OnComplete(this, depth + 1);
                compoundFields[depth] = null;
            }
        }
    }
}
