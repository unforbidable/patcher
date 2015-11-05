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

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.IMGS)]
    public sealed class Imgs : GenericFormRecord
    {
        [Member(Names.ENAM)]
        private ByteArray Unknown { get; set; }

        [Member(Names.HNAM)]
        private HdrData Hdr { get; set; }

        [Member(Names.CNAM)]
        private CinematicData Cinematic { get; set; }

        [Member(Names.TNAM)]
        private TintData Tint { get; set; }

        [Member(Names.DNAM)]
        private DofData Dof { get; set; }

        sealed class HdrData : Field
        {
            public float EyeAdaptSpeed { get; set; }
            public float BloomBlurRadius { get; set; }
            public float BloomThreshold { get; set; }
            public float BloomScale { get; set; }
            public float ReceiveBloomThreshold { get; set; }
            public float White { get; set; }
            public float SunlightScale { get; set; }
            public float SkyScale { get; set; }
            public float EyeAdaptStrength { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                EyeAdaptSpeed = reader.ReadSingle();
                BloomBlurRadius = reader.ReadSingle();
                BloomThreshold = reader.ReadSingle();
                BloomScale = reader.ReadSingle();
                ReceiveBloomThreshold = reader.ReadSingle();
                White = reader.ReadSingle();
                SunlightScale = reader.ReadSingle();
                SkyScale = reader.ReadSingle();
                EyeAdaptStrength = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(EyeAdaptSpeed);
                writer.Write(BloomBlurRadius);
                writer.Write(BloomThreshold);
                writer.Write(BloomScale);
                writer.Write(ReceiveBloomThreshold);
                writer.Write(White);
                writer.Write(SunlightScale);
                writer.Write(SkyScale);
                writer.Write(EyeAdaptStrength);
            }

            public override Field CopyField()
            {
                return new HdrData()
                {
                    EyeAdaptSpeed = EyeAdaptSpeed,
                    BloomBlurRadius = BloomBlurRadius,
                    BloomThreshold = BloomThreshold,
                    BloomScale = BloomScale,
                    ReceiveBloomThreshold = ReceiveBloomThreshold,
                    White = White,
                    SunlightScale = SunlightScale,
                    SkyScale = SkyScale,
                    EyeAdaptStrength = EyeAdaptStrength,
                };
            }

            public override string ToString()
            {
                return string.Format("HDR Data");
            }

            public override bool Equals(Field other)
            {
                var cast = (HdrData)other;
                return EyeAdaptSpeed == cast.EyeAdaptSpeed && BloomBlurRadius == cast.BloomBlurRadius && BloomThreshold == cast.BloomThreshold &&
                    BloomScale == cast.BloomScale && ReceiveBloomThreshold == cast.ReceiveBloomThreshold && White == cast.White &&
                    SunlightScale == cast.SunlightScale && SkyScale == cast.SkyScale && EyeAdaptStrength == cast.EyeAdaptStrength;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        sealed class CinematicData : Field
        {
            public float Saturation { get; set; }
            public float Brightness { get; set; }
            public float Contrast { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Saturation = reader.ReadSingle();
                Brightness = reader.ReadSingle();
                Contrast = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Saturation);
                writer.Write(Brightness);
                writer.Write(Contrast);
            }

            public override Field CopyField()
            {
                return new CinematicData()
                {
                    Saturation = Saturation,
                    Brightness = Brightness,
                    Contrast = Contrast
                };
            }

            public override string ToString()
            {
                return string.Format("Cinematic Data");
            }

            public override bool Equals(Field other)
            {
                var cast = (CinematicData)other;
                return Saturation == cast.Saturation && Brightness == cast.Saturation && Contrast == cast.Contrast;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        sealed class TintData : Field
        {
            public float Amount { get; set; }
            public float Red { get; set; }
            public float Green { get; set; }
            public float Blue { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Amount = reader.ReadSingle();
                Red = reader.ReadSingle();
                Green = reader.ReadSingle();
                Blue = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Amount);
                writer.Write(Red);
                writer.Write(Green);
                writer.Write(Blue);
            }

            public override Field CopyField()
            {
                return new TintData()
                {
                    Amount = Amount,
                    Red = Red,
                    Green = Green,
                    Blue = Blue
                };
            }

            public override string ToString()
            {
                return string.Format("Tint Data");
            }

            public override bool Equals(Field other)
            {
                var cast = (TintData)other;
                return Amount == cast.Amount && Red == cast.Red && Green == cast.Green && Blue == cast.Blue;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        sealed class DofData : Field
        {
            public float Strength { get; set; }
            public float Distance { get; set; }
            public float Range { get; set; }
            private ushort RawRadius { get; set; }

            public RadiusFlags Radius { get { return GetRadius(); } set { SetRadius(value); } }

            internal override void ReadField(RecordReader reader)
            {
                Strength = reader.ReadSingle();
                Distance = reader.ReadSingle();
                Range = reader.ReadSingle();
                if (!reader.IsEndOfSegment)
                {
                    reader.ReadUInt16();
                    RawRadius = reader.ReadUInt16();
                }
            }
            
            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Strength);
                writer.Write(Distance);
                writer.Write(Range);

                if (RawRadius != 0)
                {
                    // Write radius only if initialized
                    writer.Write((ushort)0);
                    writer.Write(RawRadius);
                }
            }

            public override Field CopyField()
            {
                return new DofData()
                {
                    Strength = Strength,
                    Distance = Distance,
                    Range = Range,
                    RawRadius = RawRadius
                };
            }

            public override string ToString()
            {
                return string.Format("Strength={0} Distance={1}", Strength, Distance);
            }

            public override bool Equals(Field other)
            {
                var cast = (DofData)other;
                return Strength == cast.Strength && Distance == cast.Distance && Range == cast.Range && RawRadius == cast.RawRadius;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }

            private void SetRadius(RadiusFlags value)
            {
                RawRadius = radiusMap.Where(m => m.Value == value).First().Key;
            }

            private RadiusFlags GetRadius()
            {
                // Return default value if not initialized
                if (RawRadius == 0)
                    return RadiusFlags.R0;
                else
                    return radiusMap.Where(m => m.Key == RawRadius).First().Value;
            }

            static Dictionary<ushort, RadiusFlags> radiusMap = new Dictionary<ushort, RadiusFlags>()
            {
                { 0x4000, RadiusFlags.R0 },
                { 0x40c0, RadiusFlags.R0 | RadiusFlags.NoSky },
                { 0x4120, RadiusFlags.R1 },
                { 0x4160, RadiusFlags.R1 | RadiusFlags.NoSky },
                { 0x4190, RadiusFlags.R2 },
                { 0x41B0, RadiusFlags.R2 | RadiusFlags.NoSky },
                { 0x41D0, RadiusFlags.R3 },
                { 0x41F0, RadiusFlags.R3 | RadiusFlags.NoSky },
                { 0x4208, RadiusFlags.R4 },
                { 0x4218, RadiusFlags.R4 | RadiusFlags.NoSky },
                { 0x4228, RadiusFlags.R5 },
                { 0x4238, RadiusFlags.R5 | RadiusFlags.NoSky },
                { 0x4248, RadiusFlags.R6 },
                { 0x4258, RadiusFlags.R6 | RadiusFlags.NoSky },
                { 0x4268, RadiusFlags.R7 },
                { 0x4278, RadiusFlags.R7 | RadiusFlags.NoSky },
            };
        }

        [Flags]
        enum RadiusFlags
        {
            R0 = 0,
            R1 = 1,
            R2 = 2,
            R3 = 3,
            R4 = 4,
            R5 = 5,
            R6 = 6,
            R7 = 7,
            NoSky = 0x8000,
        }
    }
}
