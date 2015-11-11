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

using Patcher.Data.Plugins.Content.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records
{
    [Record(Names.TES4)]
    [Game(Games.Skyrim)]
    [Game(Games.Fallout4)]
    public sealed class Tes4 : HeaderRecord
    {
        [Member(Names.HEDR)]
        [Initialize]
        private Hedr Subheader { get; set; }

        [Member(Names.OFST)]
        public ByteArray Unknown1 { get; set; }

        [Member(Names.DELE)]
        public ByteArray Unknown2 { get; set; }

        [Member(Names.CNAM)]
        public override string Author { get; set; }

        [Member(Names.TNAM)]
        [Game(Games.Fallout4)]
        private List<ByteArray> Unknown5 { get; set; }

        [Member(Names.MAST, Names.DATA)]
        private List<MasterEntry> Masters { get; set; }

        [Member(Names.SNAM)]
        public override string Description { get; set; }

        [Member(Names.SCRN)]
        public ByteArray Screenshot { get; set; }

        [Member(Names.INTV)]
        private uint? Unknown3 { get; set; }

        [Member(Names.INCC)]
        private uint? Unknown4 { get; set; }

        [Member(Names.ONAM)]
        public ByteArray OverridenForms { get; set; }

        public override float PluginVersion { get { return Subheader.Version; } set { Subheader.Version = value; } }
        public override int NumRecords { get { return Subheader.NumRecords; } set { Subheader.NumRecords = value; } }
        public override uint NextFormId { get { return Subheader.NextFormId; } set { Subheader.NextFormId = value; } }

        public override IEnumerable<string> GetMasterFiles()
        {
            return Masters != null ? Masters.Select(m => m.Filename) : Enumerable.Empty<string>();
        }

        internal override void AddMasterFile(string filename)
        {
            if (Masters == null)
            {
                Masters = new List<MasterEntry>();
            }

            Masters.Add(new MasterEntry()
            {
                Filename = filename
            });
        }

        public override string ToString()
        {
            return string.Format("Records={0} Author=\"{1}\" Description=\"{2}\"",
                NumRecords, Author, Description);
        }

        class MasterEntry : Compound
        {
            [Member(Names.MAST)]
            public string Filename { get; set; }

            [Member(Names.DATA)]
            public ulong Data { get; set; }

            public override string ToString()
            {
                return Filename;
            }
        }

        class Hedr : Field
        {
            public float Version { get; set; }
            public int NumRecords { get; set; }
            public uint NextFormId { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Version = reader.ReadSingle();
                NumRecords = reader.ReadInt32();
                NextFormId = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Version);
                writer.Write(NumRecords);
                writer.Write(NextFormId);
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("{0} {1} 0x{2:X8}", Version, NumRecords, NextFormId);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

    }
}
