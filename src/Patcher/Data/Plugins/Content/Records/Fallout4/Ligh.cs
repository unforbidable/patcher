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
    public sealed class Ligh : GenericFormRecord, IFeaturingObjectBounds
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
        public uint LensFlare { get; set; }

        [Member(Names.WGDR)]
        [Reference(Names.GDRY)]
        public uint GodRays { get; set; }

        public string WorldModel { get { return ModelData.Path; } set { ModelData.Path = value; } }

        public int Duration { get { return Data.Duration; } set { Data.Duration = value; } }
        public uint Radius { get { return Data.Radius; } set { Data.Radius = value; } }
        public float FallOffExponent { get { return Data.FallOffExponent; } set { Data.FallOffExponent = value; } }
        public float Angle { get { return Data.Angle; } set { Data.Angle = value; } }
        public float NearClip { get { return Data.NearClip; } set { Data.NearClip = value; } }
        public float FlickerPeriod { get { return Data.FlickerPeriod; } set { Data.FlickerPeriod = value; } }
        public float FlickerIntensity { get { return Data.FlickerIntensity; } set { Data.FlickerIntensity = value; } }
        public float FlickerMovement { get { return Data.FlickerMovement; } set { Data.FlickerMovement = value; } }
        public uint Value { get { return Data.Value; } set { Data.Value = value; } }
        public float Weight { get { return Data.Weight; } set { Data.Weight = value; } }
        public float Unknown1 { get { return Data.Unknown1; } set { Data.Unknown1 = value; } }
        public float Unknown2 { get { return Data.Unknown2; } set { Data.Unknown2 = value; } }
        public float Unknown3 { get { return Data.Unknown3; } set { Data.Unknown3 = value; } }
        public float Unknown4 { get { return Data.Unknown4; } set { Data.Unknown4 = value; } }

        public ColorAdapter Color { get { return new ColorAdapter(Data.ColorData); } }

        sealed class LightData : Field
        {
            public int Duration { get; set; }
            public uint Radius { get; set; }
            public byte[] ColorData { get; set; }
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

            internal override void ReadField(RecordReader reader)
            {
                Duration = reader.ReadInt32();
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
                writer.Write(Duration);
                writer.Write(Radius);
                writer.Write(ColorData);
                writer.Write((uint)Flags);
                writer.Write(FallOffExponent);
                writer.Write(Angle);
                writer.Write(NearClip);
                writer.Write(FlickerPeriod);
                writer.Write(FlickerIntensity);
                writer.Write(FlickerMovement);
                writer.Write(Value);
                writer.Write(Weight);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }

            public override Field CopyField()
            {
                return new LightData()
                {
                    Duration = Duration,
                    Radius = Radius,
                    ColorData = new List<byte>(ColorData).ToArray(),
                    Flags = Flags,
                    FallOffExponent = FallOffExponent,
                    Angle = Angle,
                    NearClip = NearClip,
                    FlickerPeriod = FlickerPeriod,
                    FlickerIntensity = FlickerIntensity,
                    FlickerMovement = FlickerMovement,
                    Value = Value,
                    Weight = Weight,
                    Unknown1 = Unknown1,
                    Unknown2 = Unknown2,
                    Unknown3 = Unknown3,
                    Unknown4 = Unknown4,
                };
            }

            public override string ToString()
            {
                return string.Format("Flags={0} Radius={1}", Flags, Radius);
            }

            public override bool Equals(Field other)
            {
                var cast = (LightData)other;
                return Duration == cast.Duration && Radius == cast.Radius && ColorData.SequenceEqual(cast.ColorData) && Flags == cast.Flags &&
                    FallOffExponent == cast.FallOffExponent && Angle == cast.Angle && NearClip == cast.NearClip && FlickerPeriod == cast.FlickerPeriod &&
                    FlickerIntensity == cast.FlickerIntensity && FlickerMovement == cast.FlickerMovement && Value == cast.Value && Weight == cast.Weight &&
                    Unknown1 == cast.Unknown1 && Unknown2 == cast.Unknown2 && Unknown3 == cast.Unknown3 && Unknown4 == cast.Unknown4;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
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
