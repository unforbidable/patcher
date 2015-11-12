using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Fields
{
    public class ReferenceArray : Field
    {
        public List<uint> List { get; set; }

        public uint this[int index] { get { return List[index]; } set { List[index] = value; } }

        public ReferenceArray()
        {
            List = new List<uint>();
        }

        public void SetLength(int length)
        {
            if (length > List.Count)
                List.AddRange(Enumerable.Range(0, length - List.Count).Select(i => (uint)0));
        }

        internal override void ReadField(RecordReader reader)
        {
            while (!reader.IsEndOfSegment)
            {
                List.Add(reader.ReadReference(FormKindSet.Any));
            }
        }

        internal override void WriteField(RecordWriter writer)
        {
            foreach (var formId in List)
            {
                writer.WriteReference(formId, FormKindSet.Any);
            }
        }

        public override Field CopyField()
        {
            return new ReferenceArray()
            {
                List = new List<uint>(List)
            };
        }

        public override bool Equals(Field other)
        {
            var cast = (ReferenceArray)other;
            return cast.List.SequenceEqual(cast.List);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            // Each reference is a referenced form
            return List.Select(i => i);
        }

        public override string ToString()
        {
            return string.Format("Count={0}{1}", List.Count,
                List.Count > 0 ? string.Format(" {0:X8}", string.Join(" ", List)) : string.Empty);
        }
    }
}
