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

namespace Patcher.Data.Plugins.Content.Records
{
    [Record(Names.IMAD)]
    [Game(Games.Skyrim)]
    [Game(Games.Fallout4)]
    public sealed class Imad : GenericFormRecord
    {
        [Member(Names.DNAM)]
        [Initialize]
        private MainData Data { get; set; }

        [Member(Names.BNAM)]
        [Initialize]
        private ByteArray Blur { get; set; }

        [Member(Names.VNAM)]
        [Initialize]
        private ByteArray DoubleVision { get; set; }

        [Member(Names.TNAM)]
        [Initialize]
        private ByteArray Tint { get; set; }

        [Member(Names.NAM3)]
        [Initialize]
        private ByteArray Fade { get; set; }

        [Member(Names.RNAM)]
        [Initialize]
        private ByteArray BlurStrength { get; set; }

        [Member(Names.SNAM)]
        [Initialize]
        private ByteArray BlurRampup { get; set; }

        [Member(Names.UNAM)]
        [Initialize]
        private ByteArray BlurStart { get; set; }

        [Member(Names.NAM1)]
        [Initialize]
        private ByteArray BlurRampdown { get; set; }

        [Member(Names.NAM2)]
        [Initialize]
        private ByteArray BlurDownStart { get; set; }

        [Member(Names.WNAM)]
        [Initialize]
        private ByteArray DepthOfFieldStrength { get; set; }

        [Member(Names.XNAM)]
        [Initialize]
        private ByteArray DepthOfFieldDistance { get; set; }

        [Member(Names.YNAM)]
        [Initialize]
        private ByteArray DepthOfFieldRange { get; set; }

        [Member(Names.NAM5)]
        [Game(Games.Fallout4)]
        private ByteArray Unknown1 { get; set; }

        [Member(Names.NAM6)]
        [Game(Games.Fallout4)]
        private ByteArray Unknown2 { get; set; }

        [Member(Names.NAM4)]
        [Initialize]
        private ByteArray MotionBlur { get; set; }

        [Member(".IAD")]
        [Initialize]
        private TimeFloatCoupleArray Couples { get; set; }

        private IEnumerable<TimeFloat> GetTimeFloats(byte[] buffer)
        {
            return Enumerable.Range(0, buffer.Length / 8).Select(i => new TimeFloat(buffer, i * 8));
        }

        private IEnumerable<TimeColor> GetTimeColors(byte[] buffer)
        {
            return Enumerable.Range(0, buffer.Length / 8).Select(i => new TimeColor(buffer, i * 8));
        }

        public IEnumerable<TimeFloat> GetSaturationMult()
        {
            return GetTimeFloats(Couples.GetElement(17));
        }

        public IEnumerable<TimeFloat> GetSaturationAdd()
        {
            return GetTimeFloats(Couples.GetElement(17 + 64));
        }

        public IEnumerable<TimeFloat> GetBrightnessMult()
        {
            return GetTimeFloats(Couples.GetElement(18));
        }

        public IEnumerable<TimeFloat> GetBrightnessAdd()
        {
            return GetTimeFloats(Couples.GetElement(18 + 64));
        }

        public IEnumerable<TimeFloat> GetContrastMult()
        {
            return GetTimeFloats(Couples.GetElement(19));
        }

        public IEnumerable<TimeFloat> GetContrastAdd()
        {
            return GetTimeFloats(Couples.GetElement(19 + 64));
        }

        public sealed class TimeFloat
        {
            public float Time { get { return BitConverter.ToSingle(buffer, offset); } set { BitConverter.GetBytes(value).CopyTo(buffer, offset); } }
            public float Value { get { return BitConverter.ToSingle(buffer, offset + 4); } set { BitConverter.GetBytes(value).CopyTo(buffer, offset + 4); } }

            readonly byte[] buffer;
            readonly int offset;

            public TimeFloat(byte[] buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;
            }

            public override string ToString()
            {
                return string.Format("Time={0} Value={1}", Time, Value);
            }
        }

        public sealed class TimeColor 
        {
            public float Time { get { return BitConverter.ToSingle(buffer, offset); } set { BitConverter.GetBytes(value).CopyTo(buffer, offset); } }
            public ColorAlphaAdapter Color { get; private set; }

            readonly byte[] buffer;
            readonly int offset;

            public TimeColor(byte[] buffer, int offset)
            {
                this.buffer = buffer;
                this.offset = offset;

                Color = new ColorAlphaAdapter(new ColorData(this));
            }

            public override string ToString()
            {
                return string.Format("Time={0} Color={1}", Time, Color);
            }

            sealed class ColorData : IColorAlphaFloatAdaptable
            {
                public float Red { get { return BitConverter.ToSingle(owner.buffer, owner.offset + 4); } set { BitConverter.GetBytes(value).CopyTo(owner.buffer, owner.offset + 4); } }
                public float Green { get { return BitConverter.ToSingle(owner.buffer, owner.offset + 8); } set { BitConverter.GetBytes(value).CopyTo(owner.buffer, owner.offset + 8); } }
                public float Blue { get { return BitConverter.ToSingle(owner.buffer, owner.offset + 12); } set { BitConverter.GetBytes(value).CopyTo(owner.buffer, owner.offset + 12); } }
                public float Alpha { get { return BitConverter.ToSingle(owner.buffer, owner.offset + 16); } set { BitConverter.GetBytes(value).CopyTo(owner.buffer, owner.offset + 16); } }

                readonly TimeColor owner;

                public ColorData(TimeColor owner)
                {
                    this.owner = owner;
                }

                public override string ToString()
                {
                    return string.Format("({0},{1},{2},{3})", Red, Green, Blue, Alpha);
                }
            }
        }

        sealed class TimeFloatCoupleArray : Compound
        {
            List<ByteArray> elements = new List<ByteArray>(new ByteArray[128]);

            public byte[] GetElement(int index)
            {
                if (elements[index] == null)
                    return null;

                return elements[index].Bytes;
            }

            internal override void ReadCompoundField(RecordReader reader, string fieldName, int depth)
            {
                byte first = (byte)fieldName[0];
                if (first < 128)
                {
                    elements[first] = new ByteArray();
                    elements[first].Bytes = reader.ReadBytesToEnd();
                }
                else
                {
                    throw new InvalidOperationException("Dynamic field is out of range: " + fieldName + " (" + first + ")");
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                // Save mult and add fields interleaved
                // WriteField will ensure null elements are not written
                for (int i = 0; i < 64; i++)
                {
                    string multFieldName = (char)i + "IAD";
                    writer.WriteField(elements[i], multFieldName);
                    string addFieldName = (char)(i + 64) + "IAD";
                    writer.WriteField(elements[i + 64], addFieldName);
                }
            }

            public override Field CopyField()
            {
                // Deep copy the all byte arrays
                return new TimeFloatCoupleArray()
                {
                    elements = new List<ByteArray>(elements.Select(e => e.CopyField()).Cast<ByteArray>())
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (TimeFloatCoupleArray)other;
                for (int i = 0; i < 128; i++)
                {
                    // Both are null, keep looking for mismatch
                    if (elements[i] == null && cast.elements[i] == null)
                        continue;

                    // Only one of them is null, not equal
                    if (elements[i] != null || cast.elements[i] != null)
                        return false;

                    // This element is not equal
                    if (!elements[i].Equals(cast.elements[i]))
                        return false;
                }

                return true;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }

            public override string ToString()
            {
                return string.Format("Count={0}", elements.Where(e => e != null).Count() / 2);
            }
        }

        sealed class MainData : Field
        {
            public Flags Flags { get; set; }
            public float Duration { get; set; }
            public int[] Sizes1 { get; private set; }
            public BlurFlags BlurFlags { get; set; }
            public float BlurCenterX { get; set; }
            public float BlurCenterY { get; set; }
            public int[] Sizes2 { get; private set; }
            public DepthOfFieldFlags DepthOfFieldFlags { get; set; }
            public int[] Sizes3 { get; private set; }
            public int[] Sizes4 { get; private set; }

            public MainData()
            {
                Sizes1 = new int[48];
                Sizes2 = new int[3];
                Sizes3 = new int[4];
            }

            internal override void ReadField(RecordReader reader)
            {
                Flags = (Flags)reader.ReadUInt32();
                Duration = reader.ReadSingle();
                for (int i = 0; i < Sizes1.Length; i++)
                    Sizes1[i] = reader.ReadInt32();
                BlurFlags = (BlurFlags)reader.ReadUInt32();
                BlurCenterX = reader.ReadSingle();
                BlurCenterY = reader.ReadSingle();
                for (int i = 0; i < Sizes2.Length; i++)
                    Sizes2[i] = reader.ReadInt32();
                DepthOfFieldFlags = (DepthOfFieldFlags)reader.ReadUInt32();
                for (int i = 0; i < Sizes3.Length; i++)
                    Sizes3[i] = reader.ReadInt32();

                if (reader.Context.GameTitle == Games.Fallout4 && !reader.IsEndOfSegment)
                {
                    // Two new values in Fallout 4
                    Sizes4 = new int[2];
                    for (int i = 0; i < Sizes4.Length; i++)
                        Sizes4[i] = reader.ReadInt32();
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((uint)Flags);
                writer.Write(Duration);
                for (int i = 0; i < Sizes1.Length; i++)
                    writer.Write(Sizes1[i]);
                writer.Write((uint)BlurFlags);
                writer.Write(BlurCenterX);
                writer.Write(BlurCenterY);
                for (int i = 0; i < Sizes2.Length; i++)
                    writer.Write(Sizes2[i]);
                writer.Write((uint)DepthOfFieldFlags);
                for (int i = 0; i < Sizes3.Length; i++)
                    writer.Write(Sizes3[i]);
                if (Sizes4 != null)
                    for (int i = 0; i < Sizes4.Length; i++)
                        writer.Write(Sizes4[i]);
            }

            public override Field CopyField()
            {
                return new MainData()
                {
                    Flags = Flags,
                    Duration = Duration,
                    Sizes1 = new List<int>(Sizes1).ToArray(),
                    BlurFlags = BlurFlags,
                    BlurCenterX = BlurCenterX,
                    BlurCenterY = BlurCenterY,
                    Sizes2 = new List<int>(Sizes2).ToArray(),
                    DepthOfFieldFlags = DepthOfFieldFlags,
                    Sizes3 = new List<int>(Sizes3).ToArray(),
                    Sizes4 = Sizes4 == null ? null : new List<int>(Sizes4).ToArray(),
                };
            }

            public override string ToString()
            {
                return string.Format("Flags={0}", Flags);
            }

            public override bool Equals(Field other)
            {
                var cast = (MainData)other;
                return Flags == cast.Flags && Duration == cast.Duration && Sizes1.SequenceEqual(cast.Sizes1) && BlurFlags == cast.BlurFlags &&
                    BlurCenterX == cast.BlurCenterX && BlurCenterY == cast.BlurCenterY && Sizes2.SequenceEqual(cast.Sizes2) &&
                    DepthOfFieldFlags == cast.DepthOfFieldFlags && Sizes3.SequenceEqual(cast.Sizes3) && Sizes4.SequenceEqual(cast.Sizes4);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        [Flags]
        enum Flags : uint
        {
            Animatable = 0x01,
        }

        [Flags]
        enum BlurFlags : uint
        {
            UseTarget = 0x01,
        }

        [Flags]
        enum DepthOfFieldFlags : uint
        {
            UseTarget = 0x0001,
            Unknown1 = 0x0002,
            Unknown2 = 0x0004,
            Unknown3 = 0x0008,
            Unknown4 = 0x0010,
            Unknown5 = 0x0020,
            Unknown6 = 0x0040,
            Unknown7 = 0x0080,
            ModeFront = 0x0100,
            ModeBack = 0x0200,
            NoSky = 0x0400,
            BlurRadiusBit2 = 0x0800,
            BlurRadiusBit1 = 0x1000,
            BlurRadiusBit0 = 0x2000,
        }
    }
}
