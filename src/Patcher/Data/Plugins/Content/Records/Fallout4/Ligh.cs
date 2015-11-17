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
using Patcher.Data.Plugins.Content.Fields.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Fallout4
{
    [Record(Names.LIGH)]
    [Game(Games.Fallout4)]
    public sealed class Ligh : GenericFormRecord
    {
        [Member(Names.VMAD)]
        public ByteArray VirtualMachineAdapter { get; set; }

        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.PTRN)]
        [Reference(Names.TRNS)]
        public uint Trns { get; set; }

        [Member(Names.MODL, Names.MODT, Names.MODS)]
        [Initialize]
        private Model ModelData { get; set; }

        [Member(Names.KSIZ, Names.KWDA)]
        [Initialize]
        public Keywords Keywords { get; set; }

        [Member(Names.PRPS)]
        public ByteArray Unknown { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.DATA)]
        [Initialize]
        private LightData Data { get; set; }

        [Member(Names.FNAM)]
        public float Fade { get; set; }

        [Member(Names.NAM0)]
        public string StencilTexture { get; set; }

        [Member(Names.LNAM)]
        [Reference(Names.LENS)]
        public uint LensEffect { get; set; }

        [Member(Names.WGDR)]
        [Reference(Names.WTHR)]
        public uint Weather { get; set; }

        protected override void AfterRead(RecordReader reader)
        {
            if (Unknown != null)
            {
                uint a = BitConverter.ToUInt32(Unknown.Bytes, 0);
                float b = BitConverter.ToSingle(Unknown.Bytes, 4);
            }

            base.AfterRead(reader);
        }

        sealed class LightData : Field
        {
            public int Time { get; set; }
            public uint Radius { get; set; }
            private byte[] ColorData { get; set; }
            public Flags Flags { get; set; }
            public float FallOffExponent { get; set; }
            public float Angle { get; set; }
            public float NearClip { get; set; }
            public float FlickerPeriod { get; set; }
            public float FlickerIntensity { get; set; }
            public float FlickerMovement { get; set; }
            public uint Value { get; set; }
            public float Weight { get; set; }
            public float Unknown1 { get; set; }
            public float Unknown2 { get; set; }
            public float Unknown3 { get; set; }
            public float Unknown4 { get; set; }
            public float Unknown5 { get; set; }
            public float Unknown6 { get; set; }
            public float Unknown7 { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Time = reader.ReadInt32();
                Radius = reader.ReadUInt32();
                ColorData = reader.ReadBytes(4);
                Flags = (Flags)reader.ReadUInt32();
                FallOffExponent = reader.ReadSingle();
                Angle = reader.ReadSingle();
                NearClip = reader.ReadSingle();
                FlickerPeriod = reader.ReadSingle();
                FlickerIntensity = reader.ReadSingle();
                FlickerMovement = reader.ReadSingle();
                Value = reader.ReadUInt32();
                Weight = reader.ReadSingle();
                if (!reader.IsEndOfSegment)
                {
                    Unknown1 = reader.ReadSingle();
                    Unknown2 = reader.ReadSingle();
                    Unknown3 = reader.ReadSingle();
                    if (!reader.IsEndOfSegment)
                        Unknown4 = reader.ReadSingle();
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }
        }

        [Flags]
        enum Flags : uint
        {
            Dynamic = 0x0001,
            CanBeCarrier = 0x0002,
            Negative = 0x0004,
            Flicker = 0x0008,
            Unnamed4 = 0x0010,
            OffByDefault = 0x0020,
            FlickerSlow = 0x0040,
            Pulse = 0x0080,
            PulseSlow = 0x0100,
            SpotLight = 0x0200,
            ShadowSpotLight = 0x0400,
            ShadowHemishere = 0x0800,
            ShadowOmnidirectional = 0x1000,
            PortalStrict = 0x2000,
            Unnamed14 = 0x4000,
            Unnamed15 = 0x8000,
            Unnamed16 = 0x10000,
            Unnamed17 = 0x20000,
            Unnamed18 = 0x40000,
            Unnamed19 = 0x80000,
            Unnamed20 = 0x100000,
            Unnamed21 = 0x200000,
            Unnamed22 = 0x400000,
            Unnamed23 = 0x800000,
            Unnamed24 = 0x1000000,
            Unnamed25 = 0x2000000,
            Unnamed26 = 0x4000000,
            Unnamed27 = 0x8000000,
            Unnamed28 = 0x10000000,
            Unnamed29 = 0x20000000,
            Unnamed30 = 0x40000000,
            Unnamed31 = 0x80000000

        }
    }
}
