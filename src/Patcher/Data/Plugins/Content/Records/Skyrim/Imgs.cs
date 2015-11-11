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
    [Game(Games.Skyrim)]
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

        ColorAdapter color = null;
        public ColorAdapter TintColor
        {
            get
            {
                EnsureTintExists();
                if (color == null)
                    color = new ColorAdapter(Tint);
                return color;
            }
        }

        private void EnsureHdrExists()
        {
            if (Hdr == null)
                Hdr = new HdrData();
        }

        private void EnsureCinematicExists()
        {
            if (Cinematic == null)
                Cinematic = new CinematicData();
        }

        private void EnsureTintExists()
        {
            if (Tint == null)
                Tint = new TintData();
        }

        private void EnsureDofExists()
        {
            if (Dof == null)
                Dof = new DofData();
        }

        static readonly HdrData DefaultHdr = new HdrData();
        static readonly CinematicData DefaultCinematic = new CinematicData();
        static readonly TintData DefaultTint = new TintData();
        static readonly DofData DefaultDof = new DofData();

        public float EyeAdaptSpeed { get { return (Hdr ?? DefaultHdr).EyeAdaptSpeed; } set { EnsureHdrExists(); Hdr.EyeAdaptSpeed = value; } }
        public float BloomBlurRadius { get { return (Hdr ?? DefaultHdr).BloomBlurRadius; } set { EnsureHdrExists(); Hdr.BloomBlurRadius = value; } }
        public float BloomThreshold { get { return (Hdr ?? DefaultHdr).BloomThreshold; } set { EnsureHdrExists(); Hdr.BloomThreshold = value; } }
        public float BloomScale { get { return (Hdr ?? DefaultHdr).BloomScale; } set { EnsureHdrExists(); Hdr.BloomScale = value; } }
        public float ReceiveBloomThreshold { get { return (Hdr ?? DefaultHdr).ReceiveBloomThreshold; } set { EnsureHdrExists(); Hdr.ReceiveBloomThreshold = value; } }
        public float White { get { return (Hdr ?? DefaultHdr).White; } set { EnsureHdrExists(); Hdr.White = value; } }
        public float SunlightScale { get { return (Hdr ?? DefaultHdr).SunlightScale; } set { EnsureHdrExists(); Hdr.SunlightScale = value; } }
        public float SkyScale { get { return (Hdr ?? DefaultHdr).SkyScale; } set { EnsureHdrExists(); Hdr.SkyScale = value; } }
        public float EyeAdaptStrength { get { return (Hdr ?? DefaultHdr).EyeAdaptStrength; } set { EnsureHdrExists(); Hdr.EyeAdaptStrength = value; } }

        public float Saturation { get { return (Cinematic ?? DefaultCinematic).Saturation; } set { EnsureCinematicExists(); Cinematic.Saturation = value; } }
        public float Brightness { get { return (Cinematic ?? DefaultCinematic).Brightness; } set { EnsureCinematicExists(); Cinematic.Brightness = value; } }
        public float Contrast { get { return (Cinematic ?? DefaultCinematic).Contrast; } set { EnsureCinematicExists(); Cinematic.Contrast = value; } }

        public float TintAmount { get { return (Tint ?? DefaultTint).Amount; } set { EnsureTintExists(); Tint.Amount = value; } }

        public float DepthOfFieldStrength { get { return (Dof ?? DefaultDof).Strength; } set { EnsureDofExists(); Dof.Strength = value; } }
        public float DepthOfFieldRange { get { return (Dof ?? DefaultDof).Range; } set { EnsureDofExists(); Dof.Range = value; } }
        public float DepthOfFieldDistance { get { return (Dof ?? DefaultDof).Distance; } set { EnsureDofExists(); Dof.Distance = value; } }
        public float DepthOfFieldBlurRadius { get { return (Dof ?? DefaultDof).GetRadius(); } set { EnsureDofExists(); Dof.SetRadius(value); } }
        public bool IsDepthOfFieldSkyDisabled { get { return (Dof ?? DefaultDof).GetNoSky(); } set { EnsureDofExists(); Dof.SetNoSky(value); } }

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
                return Saturation == cast.Saturation && Brightness == cast.Brightness && Contrast == cast.Contrast;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        sealed class TintData : Field, IColorFloatAdaptable
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
            private float Radius { get; set; }

            public float GetRadius()
            {
                if (Radius == 0)
                    return 0;
                else
                    return (float)Math.Floor((Radius - 2) / 8);
            }

            public void SetRadius(float value)
            {
                value = (float)Math.Min(7, Math.Max(0, Math.Floor(value)));
                if (GetNoSky())
                    Radius = value * 8 + 6;
                else
                    Radius = value * 8 + 2;
            }

            public bool GetNoSky()
            {
                return Radius == 0 || Radius % 8 == 6;
            }

            public void SetNoSky(bool value)
            {
                if (value && !GetNoSky())
                    Radius += 4;
                else if (!value && GetNoSky())
                    Radius -= 4;
            }

            internal override void ReadField(RecordReader reader)
            {
                Strength = reader.ReadSingle();
                Distance = reader.ReadSingle();
                Range = reader.ReadSingle();

                if (!reader.IsEndOfSegment)
                {
                    Radius = reader.ReadSingle();
                }
            }
            
            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Strength);
                writer.Write(Distance);
                writer.Write(Range);

                if (Radius != 0)
                {
                    // Write radius only if initialized
                    writer.Write(Radius);
                }
            }

            public override Field CopyField()
            {
                return new DofData()
                {
                    Strength = Strength,
                    Distance = Distance,
                    Range = Range,
                    Radius = Radius
                };
            }

            public override string ToString()
            {
                return string.Format("Strength={0} Distance={1}", Strength, Distance);
            }

            public override bool Equals(Field other)
            {
                var cast = (DofData)other;
                return Strength == cast.Strength && Distance == cast.Distance && Range == cast.Range && Radius == cast.Radius;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }
    }
}
