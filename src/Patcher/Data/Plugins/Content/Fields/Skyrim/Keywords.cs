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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class Keywords : Compound
    {
        [Member(Names.KSIZ)]
        private int? Size { get; set; }

        [Member(Names.KWDA)]
        private KeywordData Data { get; set; }

        public List<uint> Items { get { EnsureDataCreated(); return Data.List; } set { EnsureDataCreated(); Data.List = value; } }

        private void EnsureDataCreated()
        {
            // Data are created only on demand
            // Will be destroyed if empty
            if (Data == null)
            {
                Data = new KeywordData();
            }
        }

        protected override void BeforeWrite(RecordWriter writer)
        {
            // Sync KISZ before saving
            if (Data == null || Data.List.Count == 0)
            {
                Size = null;

                // Also unset empty Data if instantiated and empty
                // to prevent the creation of an empty KWDA segment
                Data = null;
            }
            else
            {
                Size = Data.List.Count;
            }
        }

        public override string ToString()
        {
            return Data == null ? string.Empty : Data.ToString();
        }

        public class KeywordData : Field
        {
            public List<uint> List { get; set; }

            public KeywordData()
            {
                List = new List<uint>();
            }

            internal override void ReadField(RecordReader reader)
            {
                while (!reader.IsEndOfSegment)
                {
                    List.Add(reader.ReadReference(FormKindSet.KywdOnly));
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                foreach (var formId in List)
                {
                    writer.WriteReference(formId, FormKindSet.KywdOnly);
                }
            }

            public override Field CopyField()
            {
                return new KeywordData()
                {
                    List = new List<uint>(List)
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (KeywordData)other;
                return cast.List.SequenceEqual(cast.List);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                // Each keyword is a referenced form
                return List.Select(i => i);
            }

            public override string ToString()
            {
                return string.Format("Count={0}{1}", List.Count,
                    List.Count > 0 ? string.Format(" {0}", string.Join(" ", List)) : string.Empty);
            }
        }

    }
}
