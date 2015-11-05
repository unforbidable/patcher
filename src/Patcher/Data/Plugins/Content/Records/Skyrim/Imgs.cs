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
