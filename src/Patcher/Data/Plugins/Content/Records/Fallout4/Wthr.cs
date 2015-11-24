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
    [Record(Names.WTHR)]
    [Game(Games.Fallout4)]
    public sealed class Wthr : GenericFormRecord
    {
        [Member(".0TX")]
        [Initialize]
        private WeatherTextures CloudTextures { get; set; }

        [Member(Names.LNAM)]
        private int CloudTextureCount { get; set; }

        [Member(Names.MNAM)]
        [Reference(Names.SPGD)]
        [Required]
        public uint PrecipitationParticle { get; set; }

        [Member(Names.NNAM)]
        [Reference(Names.RFCT)]
        [Required]
        public uint VisualEffect { get; set; }

        [Member(Names.RNAM)]
        private ByteArray CloudSpeedsY { get; set; }

        [Member(Names.QNAM)]
        private ByteArray CloudSpeedsX { get; set; }

        [Member(Names.PNAM)]
        private ByteArray CloudColors { get; set; }

        [Member(Names.JNAM)]
        private ByteArray CloudAlphas { get; set; }

        [Member(Names.NAM0)]
        [Initialize]
        private ByteArray Colors { get; set; }

        [Member(Names.NAM4)]
        [Game(Names.NAM4)]
        private ByteArray Unknown { get; set; }

        [Member(Names.FNAM)]
        private FogData Fog { get; set; }

        [Member(Names.DATA)]
        [Initialize]
        private WeatherData Data { get; set; }

        [Member(Names.NAM1)]
        private uint DisabledLayerFlags { get; set; }

        [Member(Names.SNAM)]
        public List<WeatherSoundItem> Sounds { get; set; }

        [Member(Names.TNAM)]
        [Reference(Names.STAT)]
        [Initialize]
        public List<uint> SkyStatics { get; set; }

        [Member(Names.IMSP)]
        private ReferenceArray ImageSpaces { get; set; }

        [Member(Names.WGDR)]
        private ReferenceArray GodRays { get; set; }

        [Member(Names.DALC)]
        [Initialize]
        private List<ByteArray> LightDataParts { get; set; }

        [Member(Names.MODL, Names.MODT, Names.MODT)]
        private Model Aurora { get; set; }

        [Member(Names.GNAM)]
        private ByteArray UnknownData2 { get; set; }

        [Member(Names.UNAM)]
        private ByteArray UnknownData3 { get; set; }

        [Member(Names.VNAM)]
        private ByteArray UnknownData4 { get; set; }

        [Member(Names.WNAM)]
        private ByteArray UnknownData5 { get; set; }

        private byte[] allLightData = new byte[256];

        protected override void AfterRead(RecordReader reader)
        {
            // Ensure ComponentColors are full size
            if (Colors.Bytes.Length < 608)
            {
                byte[] temp = new byte[608];

                if (Colors.Bytes.Length == 544)
                {
                    // Contains extended data, but only 17 components
                    // Simply copy data at the beginning of the buffer
                    Colors.Bytes.CopyTo(temp, 0);
                }
                else if (Colors.Bytes.Length == 272)
                {
                    // Contains old data and only 17 components
                    // Data should be copied in an interleaved fashion
                    for (int i = 0; i < Colors.Bytes.Length / 16; i++)
                    {
                        Array.Copy(Colors.Bytes, i * 16, temp, i * 32, 16);

                        // Copy dawn and dusk values to extended data
                        Array.Copy(temp, i * 32, temp, i * 32 + 16, 4);
                        Array.Copy(temp, i * 32, temp, i * 32 + 20, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 24, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 28, 4);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unexpected Weather Component Color data size: " + Colors.Bytes.Length);
                }

                Colors.Bytes = temp;
            }

            // Ensure Cloud Colors are full size
            if (CloudColors != null && CloudColors.Bytes.Length < 1024)
            {

                byte[] temp = new byte[1024];
                if (CloudColors.Bytes.Length == 512)
                {
                    //Data should be copied in an interleaved fashion
                    for (int i = 0; i < 32; i++)
                    {
                        Array.Copy(CloudColors.Bytes, i * 16, temp, i * 32, 16);

                        // Copy dawn and dusk values to extended data
                        Array.Copy(temp, i * 32, temp, i * 32 + 16, 4);
                        Array.Copy(temp, i * 32, temp, i * 32 + 20, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 24, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 28, 4);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unexpected Cloud Color data size: " + CloudColors.Bytes.Length);
                }

                CloudColors.Bytes = temp;
            }

            // Ensure Cloud Alphas are full size
            if (CloudAlphas != null && CloudAlphas.Bytes.Length < 1024)
            {
                byte[] temp = new byte[1024];

                if (CloudAlphas.Bytes.Length == 512)
                {
                    //Data should be copied in an interleaved fashion
                    for (int i = 0; i < 32; i++)
                    {
                        Array.Copy(CloudAlphas.Bytes, i * 16, temp, i * 32, 16);

                        // Copy dawn and dusk values to extended data
                        Array.Copy(temp, i * 32, temp, i * 32 + 16, 4);
                        Array.Copy(temp, i * 32, temp, i * 32 + 20, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 24, 4);
                        Array.Copy(temp, i * 32 + 8, temp, i * 32 + 28, 4);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unexpected Cloud Alpha data size: " + CloudAlphas.Bytes.Length);
                }

                CloudAlphas.Bytes = temp;
            }

            // Copy LightData into a single array
            if (LightDataParts.Count == 8)
            {
                for (int i = 0; i < 8; i++)
                    LightDataParts[i].Bytes.CopyTo(allLightData, i * 32);
            }
            else if (LightDataParts.Capacity == 4)
            {
                for (int i = 0; i < 4; i++)
                    LightDataParts[i].Bytes.CopyTo(allLightData, i * 32);

                // Also copy light data from dawn and dusk to the extended buffer
                LightDataParts[0].Bytes.CopyTo(allLightData, 4 * 32);
                LightDataParts[0].Bytes.CopyTo(allLightData, 5 * 32);
                LightDataParts[2].Bytes.CopyTo(allLightData, 6 * 32);
                LightDataParts[2].Bytes.CopyTo(allLightData, 7 * 32);
            }
            else
            {
                throw new InvalidOperationException("Unexpected Light Data item count: " + LightDataParts.Count);
            }
        }

        protected override void BeforeWrite(RecordWriter writer)
        {
            // Copy single array back into LightData
            for (int i = 0; i < LightDataParts.Count; i++)
            {
                // Ensure byte array is big enough
                if (LightDataParts[i].Bytes.Length < 32)
                    LightDataParts[i].Bytes = new byte[32];

                Array.Copy(allLightData, i * 32, LightDataParts[i].Bytes, 0, 32);
            }
        }

        protected override void AfterCopy(GenericFormRecord copy)
        {
            allLightData.CopyTo(((Wthr)copy).allLightData, 0);
            base.AfterCopy(copy);
        }

        public CloudLayer GetCloudLayer(int layer)
        {
            return new CloudLayer(this, layer);
        }

        public IEnumerable<CloudLayer> GetCloudLayers()
        {
            return Enumerable.Range(0, 32).Select(i => GetCloudLayer(i));
        }

        public ColorOctave GetColor(int component)
        {
            return new ColorOctave(Colors.Bytes, component * 32);
        }

        public IEnumerable<ColorOctave> GetColors()
        {
            return Enumerable.Range(0, 19).Select(i => GetColor(i));
        }

        public ColorOctave GetAmbientColorX2() { return new ColorOctave(allLightData, 0, 32); } 
        public ColorOctave GetAmbientColorX1() { return new ColorOctave(allLightData, 4, 32); } 
        public ColorOctave GetAmbientColorY2() { return new ColorOctave(allLightData, 8, 32); } 
        public ColorOctave GetAmbientColorY1() { return new ColorOctave(allLightData, 12, 32); } 
        public ColorOctave GetAmbientColorZ2() { return new ColorOctave(allLightData, 16, 32); } 
        public ColorOctave GetAmbientColorZ1() { return new ColorOctave(allLightData, 20, 32); } 
        public ColorOctave GetSpecularColor() { return new ColorOctave(allLightData, 24, 32); } 
        public FloatOctave GetFresnelPower() { return new FloatOctave(allLightData, 28, 32); } 

        void EnsureFogDataCreated()
        {
            if (Fog == null)
                Fog = new FogData();
        }

        public float FogDayNear { get { return Fog == null ? 0f : Fog.DayNear; } set { EnsureFogDataCreated(); Fog.DayNear = value; } }
        public float FogDayFar { get { return Fog == null ? 0f : Fog.DayFar; } set { EnsureFogDataCreated(); Fog.DayFar = value; } }
        public float FogNightNear { get { return Fog == null ? 0f : Fog.NightNear; } set { EnsureFogDataCreated(); Fog.NightNear = value; } }
        public float FogNightFar { get { return Fog == null ? 0f : Fog.NightFar; } set { EnsureFogDataCreated(); Fog.NightFar = value; } }
        public float FogDayPow { get { return Fog == null ? 0f : Fog.DayPow; } set { EnsureFogDataCreated(); Fog.DayPow = value; } }
        public float FogNightPow { get { return Fog == null ? 0f : Fog.NightPow; } set { EnsureFogDataCreated(); Fog.NightPow = value; } }
        public float FogDayMax { get { return Fog == null ? 0f : Fog.DayMax; } set { EnsureFogDataCreated(); Fog.DayMax = value; } }
        public float FogNightMax { get { return Fog == null ? 0f : Fog.NightMax; } set { EnsureFogDataCreated(); Fog.NightMax = value; } }
        public float FogUnknown1 { get { return Fog == null ? 0f : Fog.Unknown1; } set { EnsureFogDataCreated(); Fog.Unknown1 = value; } }
        public float FogUnknown2 { get { return Fog == null ? 0f : Fog.Unknown2; } set { EnsureFogDataCreated(); Fog.Unknown2 = value; } }
        public float FogUnknown3 { get { return Fog == null ? 0f : Fog.Unknown3; } set { EnsureFogDataCreated(); Fog.Unknown3 = value; } }
        public float FogUnknown4 { get { return Fog == null ? 0f : Fog.Unknown4; } set { EnsureFogDataCreated(); Fog.Unknown4 = value; } }
        public float FogUnknown5 { get { return Fog == null ? 0f : Fog.Unknown5; } set { EnsureFogDataCreated(); Fog.Unknown5 = value; } }
        public float FogUnknown6 { get { return Fog == null ? 0f : Fog.Unknown6; } set { EnsureFogDataCreated(); Fog.Unknown6 = value; } }
        public float FogUnknown7 { get { return Fog == null ? 0f : Fog.Unknown7; } set { EnsureFogDataCreated(); Fog.Unknown7 = value; } }
        public float FogUnknown8 { get { return Fog == null ? 0f : Fog.Unknown8; } set { EnsureFogDataCreated(); Fog.Unknown8 = value; } }
        public float FogUnknown9 { get { return Fog == null ? 0f : Fog.Unknown9; } set { EnsureFogDataCreated(); Fog.Unknown9 = value; } }
        public float FogUnknown10 { get { return Fog == null ? 0f : Fog.Unknown10; } set { EnsureFogDataCreated(); Fog.Unknown10 = value; } }

        sealed class FogData : Field
        {
            public float DayNear { get; set; }
            public float DayFar { get; set; }
            public float NightNear { get; set; }
            public float NightFar { get; set; }
            public float DayPow { get; set; }
            public float NightPow { get; set; }
            public float DayMax { get; set; }
            public float NightMax { get; set; }
            public float Unknown1 { get; set; }
            public float Unknown2 { get; set; }
            public float Unknown3 { get; set; }
            public float Unknown4 { get; set; }
            public float Unknown5 { get; set; }
            public float Unknown6 { get; set; }
            public float Unknown7 { get; set; }
            public float Unknown8 { get; set; }
            public float Unknown9 { get; set; }
            public float Unknown10 { get; set; }

            bool extendedData14 = true;
            bool extendedData19 = true;

            internal override void ReadField(RecordReader reader)
            {
                DayNear = reader.ReadSingle();
                DayFar = reader.ReadSingle();
                NightNear = reader.ReadSingle();
                NightFar = reader.ReadSingle();
                DayPow = reader.ReadSingle();
                NightPow = reader.ReadSingle();
                DayMax = reader.ReadSingle();
                NightMax = reader.ReadSingle();

                if (!reader.IsEndOfSegment)
                {
                    Unknown1 = reader.ReadSingle();
                    Unknown2 = reader.ReadSingle();
                    Unknown3 = reader.ReadSingle();
                    Unknown4 = reader.ReadSingle();
                    Unknown5 = reader.ReadSingle();
                    Unknown6 = reader.ReadSingle();

                    if (!reader.IsEndOfSegment)
                    {
                        Unknown7 = reader.ReadSingle();
                        Unknown8 = reader.ReadSingle();
                        Unknown9 = reader.ReadSingle();
                        Unknown10 = reader.ReadSingle();
                    }
                    else
                    {
                        extendedData19 = false;
                    }
                }
                else
                {
                    extendedData14 = false;
                }
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(DayNear);
                writer.Write(DayFar);
                writer.Write(NightNear);
                writer.Write(NightFar);
                writer.Write(DayPow);
                writer.Write(NightPow);
                writer.Write(DayMax);
                writer.Write(NightMax);
                if (extendedData14)
                {
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                    writer.Write(Unknown4);
                    writer.Write(Unknown5);
                    writer.Write(Unknown6);
                    if (extendedData19)
                    {
                        writer.Write(Unknown7);
                        writer.Write(Unknown8);
                        writer.Write(Unknown9);
                        writer.Write(Unknown10);
                    }
                }
            }

            public override Field CopyField()
            {
                return new FogData()
                {
                    DayNear = DayNear,
                    DayFar = DayFar,
                    NightNear = NightNear,
                    NightFar = NightFar,
                    DayPow = DayPow,
                    NightPow = NightPow,
                    DayMax = DayMax,
                    NightMax = NightMax,
                    Unknown1 = Unknown1,
                    Unknown2 = Unknown2,
                    Unknown3 = Unknown3,
                    Unknown4 = Unknown4,
                    Unknown5 = Unknown5,
                    Unknown6 = Unknown6,
                    Unknown7 = Unknown7,
                    Unknown8 = Unknown8,
                    Unknown9 = Unknown9,
                    Unknown10 = Unknown10
                };
            }

            public override string ToString()
            {
                return string.Format("Fog Data");
            }

            public override bool Equals(Field other)
            {
                var cast = (FogData)other;
                return DayNear == cast.DayNear && DayFar == cast.DayFar && NightNear == cast.NightNear && NightFar == cast.NightFar &&
                    DayPow == cast.DayPow && NightPow == cast.NightPow && DayMax == cast.DayMax && NightMax == cast.NightMax &&
                    Unknown1 == cast.Unknown1 && Unknown2 == cast.Unknown2 && Unknown3 == cast.Unknown3 && Unknown4 == cast.Unknown4 && Unknown5 == cast.Unknown5 &&
                    Unknown6 == cast.Unknown6 && Unknown7 == cast.Unknown7 && Unknown8 == cast.Unknown8 && Unknown9 == cast.Unknown9 && Unknown10 == cast.Unknown10;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        void EnsureImageSpaceCreated()
        {
            if (ImageSpaces == null)
            {
                ImageSpaces = new ReferenceArray();
            }
            ImageSpaces.SetLength(8);
        }

        private uint GetImageSpace(int index)
        {
            if (ImageSpaces == null)
                return 0;

            EnsureImageSpaceCreated();
            return ImageSpaces[index];
        }

        private void SetImageSpace(int index, uint value)
        {
            EnsureImageSpaceCreated();
            ImageSpaces[index] = value;
        }

        public uint DawnImageSpace { get { return GetImageSpace(0); } set { SetImageSpace(0, value); } }
        public uint DayImageSpace { get { return GetImageSpace(1); } set { SetImageSpace(1, value); } }
        public uint DuskImageSpace { get { return GetImageSpace(2); } set { SetImageSpace(2, value); } }
        public uint NightImageSpace { get { return GetImageSpace(3); } set { SetImageSpace(3, value); } }
        public uint EarlyDawnImageSpace { get { return GetImageSpace(4); } set { SetImageSpace(4, value); } }
        public uint LateDawnImageSpace { get { return GetImageSpace(5); } set { SetImageSpace(5, value); } }
        public uint EarlyDuskImageSpace { get { return GetImageSpace(6); } set { SetImageSpace(6, value); } }
        public uint LateDuskImageSpace { get { return GetImageSpace(7); } set { SetImageSpace(7, value); } }

        void EnsureGodRaysCreated()
        {
            if (ImageSpaces == null)
            {
                GodRays = new ReferenceArray();
            }
            GodRays.SetLength(8);
        }

        private uint GetGodRay(int index)
        {
            if (GodRays == null)
                return 0;

            EnsureGodRaysCreated();
            return GodRays[index];
        }

        private void SetGodRay(int index, uint value)
        {
            EnsureImageSpaceCreated();
            GodRays[index] = value;
        }

        public uint DawnGodRays { get { return GetGodRay(0); } set { SetGodRay(0, value); } }
        public uint DayGodRays { get { return GetGodRay(1); } set { SetGodRay(1, value); } }
        public uint DuskGodRays { get { return GetGodRay(2); } set { SetGodRay(2, value); } }
        public uint NightGodRays { get { return GetGodRay(3); } set { SetGodRay(3, value); } }
        public uint EarlyDawnGodRays { get { return GetGodRay(4); } set { SetGodRay(4, value); } }
        public uint LateDawnGodRays { get { return GetGodRay(5); } set { SetGodRay(5, value); } }
        public uint EarlyDuskGodRays { get { return GetGodRay(6); } set { SetGodRay(6, value); } }
        public uint LateDuskGodRays { get { return GetGodRay(7); } set { SetGodRay(7, value); } }

        ColorAdapter lightningColor = null;

        public float WindSpeed { get { return Data.WindSpeed; } set { Data.WindSpeed = value; } }
        public float TransDelta { get { return Data.TransDelta; } set { Data.TransDelta = value; } }
        public float SunGlare { get { return Data.SunGlare; } set { Data.SunGlare = value; } }
        public float SunDamage { get { return Data.SunDamage; } set { Data.SunDamage = value; } }
        public float PrecipitationBeginFadeIn { get { return Data.PrecipitationBeginFadeIn; } set { Data.PrecipitationBeginFadeIn = value; } }
        public float PrecipitationEndFadeOut { get { return Data.PrecipitationEndFadeOut; } set { Data.PrecipitationEndFadeOut = value; } }
        public float ThunderBeginFadeIn { get { return Data.ThunderBeginFadeIn; } set { Data.ThunderBeginFadeIn = value; } }
        public float ThunderEndFadeOut { get { return Data.ThunderEndFadeOut; } set { Data.ThunderEndFadeOut = value; } }
        public float ThunderFrequency { get { return Data.ThunderFrequency; } set { Data.ThunderFrequency = value; } }
        public ColorAdapter LightningColor { get { if (lightningColor == null) lightningColor = new ColorAdapter(Data.LightningColor); return lightningColor; } }
        public float WindDirection { get { return Data.WindDirection; } set { Data.WindDirection = value; } }
        public float WindDirectionRange { get { return Data.WindDirectionRange; } set { Data.WindDirectionRange = value; } }
        public float Unknown1 { get { return Data.Unknown4.HasValue ? Data.Unknown4.Value : 0f; } set { Data.Unknown4 = value; } }

        public bool IsPleasant { get { return HasWeatherFlag(WeatherFlags.Pleasant); } set { SetWeatherFlag(WeatherFlags.Pleasant, value); } }
        public bool IsCloudy { get { return HasWeatherFlag(WeatherFlags.Cloudy); } set { SetWeatherFlag(WeatherFlags.Cloudy, value); } }
        public bool IsRainy { get { return HasWeatherFlag(WeatherFlags.Rainy); } set { SetWeatherFlag(WeatherFlags.Rainy, value); } }
        public bool IsSnowy { get { return HasWeatherFlag(WeatherFlags.Snowy); } set { SetWeatherFlag(WeatherFlags.Snowy, value); } }
        public bool AuroraFollowsSun { get { return HasWeatherFlag(WeatherFlags.FollowsSunPosition); } set { SetWeatherFlag(WeatherFlags.FollowsSunPosition, value); } }
        public bool EffectsAlwaysVisible { get { return HasWeatherFlag(WeatherFlags.AlwaysVisible); } set { SetWeatherFlag(WeatherFlags.AlwaysVisible, value); } }

        bool HasWeatherFlag(WeatherFlags flag)
        {
            return Data.Flags.HasFlag(flag);
        }

        void SetWeatherFlag(WeatherFlags flag, bool value)
        {
            if (value)
            {
                // When setting weather type, unset all weather types bits first
                if ((flag & WeatherFlags.WeatherTypeMask) != 0)
                    Data.Flags &= ~WeatherFlags.WeatherTypeMask;

                Data.Flags |= flag;
            }
            else
            {
                Data.Flags &= ~flag;
            }
        }

        sealed class WeatherData : Field
        {
            public float WindSpeed { get; set; }
            private byte Unknown1 { get; set; }
            private byte Unknown2 { get; set; }
            public float TransDelta { get; set; }
            public float SunGlare { get; set; }
            public float SunDamage { get; set; }
            public float PrecipitationBeginFadeIn { get; set; }
            public float PrecipitationEndFadeOut { get; set; }
            public float ThunderBeginFadeIn { get; set; }
            public float ThunderEndFadeOut { get; set; }
            public float ThunderFrequency { get; set; }
            public WeatherFlags Flags { get; set; }
            public byte[] LightningColor { get; set; }
            private byte Unknown3 { get; set; }
            public float WindDirection { get; set; }
            public float WindDirectionRange { get; set; }
            public float? Unknown4 { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                WindSpeed = reader.ReadByte() / 255f;
                Unknown1 = reader.ReadByte();
                Unknown2 = reader.ReadByte();
                TransDelta = reader.ReadByte() / 1020f;
                SunGlare = reader.ReadByte() / 255f;
                SunDamage = reader.ReadByte() / 255f;
                PrecipitationBeginFadeIn = reader.ReadByte() / 255f;
                PrecipitationEndFadeOut = reader.ReadByte() / 255f;
                ThunderBeginFadeIn = reader.ReadByte() / 255f;
                ThunderEndFadeOut = reader.ReadByte() / 255f;
                ThunderFrequency = reader.ReadByte() / 255f;
                Flags = (WeatherFlags)reader.ReadByte();
                LightningColor = reader.ReadBytes(4);
                Unknown3 = reader.ReadByte();
                WindDirection = reader.ReadByte() * 360f / 255f;
                WindDirectionRange = reader.ReadByte() * 180f / 255f;

                // 20th byte not always present in Fallout 4
                if (!reader.IsEndOfSegment)
                    Unknown4 = reader.ReadByte() / 255f;
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((byte)(WindSpeed * 255f));
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write((byte)(TransDelta * 1020f));
                writer.Write((byte)(SunGlare * 255f));
                writer.Write((byte)(SunDamage * 255f));
                writer.Write((byte)(PrecipitationBeginFadeIn * 255f));
                writer.Write((byte)(PrecipitationEndFadeOut * 255f));
                writer.Write((byte)(ThunderBeginFadeIn * 255f));
                writer.Write((byte)(ThunderEndFadeOut * 255f));
                writer.Write((byte)(ThunderFrequency * 255f));
                writer.Write((byte)Flags);
                writer.Write(LightningColor);
                writer.Write(Unknown3);
                writer.Write((byte)(WindDirection * 255f / 360f));
                writer.Write((byte)(WindDirectionRange * 255f / 180f));

                if (Unknown4.HasValue)
                    writer.Write((byte)(Unknown4.Value * 255f));
            }

            public override Field CopyField()
            {
                return new WeatherData()
                {
                    WindSpeed = WindSpeed,
                    TransDelta = TransDelta,
                    SunGlare = SunGlare,
                    SunDamage = SunDamage,
                    PrecipitationBeginFadeIn = PrecipitationBeginFadeIn,
                    PrecipitationEndFadeOut = PrecipitationEndFadeOut,
                    ThunderBeginFadeIn = ThunderBeginFadeIn,
                    ThunderEndFadeOut = ThunderEndFadeOut,
                    ThunderFrequency = ThunderFrequency,
                    Flags = Flags,
                    LightningColor = LightningColor,
                    WindDirection = WindDirection,
                    WindDirectionRange = WindDirectionRange,
                    Unknown1 = Unknown1,
                    Unknown2 = Unknown2,
                    Unknown3 = Unknown3,
                    Unknown4 = Unknown4,
                };
            }

            public override string ToString()
            {
                return string.Format("Flags={0}", Flags);
            }

            public override bool Equals(Field other)
            {
                var cast = (WeatherData)other;
                return WindSpeed == cast.WindSpeed && TransDelta == cast.TransDelta && SunGlare == cast.SunGlare && SunDamage == cast.SunDamage &&
                    PrecipitationBeginFadeIn == cast.PrecipitationBeginFadeIn && PrecipitationEndFadeOut == cast.PrecipitationEndFadeOut &&
                    ThunderBeginFadeIn == cast.ThunderBeginFadeIn && ThunderEndFadeOut == cast.ThunderEndFadeOut && ThunderFrequency == cast.ThunderFrequency &&
                    Flags == cast.Flags && LightningColor.SequenceEqual(cast.LightningColor) && WindDirection == cast.WindDirection && WindDirectionRange == cast.WindDirectionRange &&
                    Unknown1 == cast.Unknown1 && Unknown2 == cast.Unknown2 && Unknown3 == cast.Unknown3 && Unknown4 == cast.Unknown4;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }

        sealed class WeatherTextures : DynamicArrayCompound<string>
        {
            static readonly byte IndexBase = Convert.ToByte('0');

            protected override int FieldNameToIndex(string fieldName)
            {
                if (!fieldName.EndsWith("0TX"))
                    throw new InvalidOperationException("Unexpected field name: " + fieldName);

                return Convert.ToByte(fieldName[0]) - IndexBase;
            }

            protected override string IndexToFieldName(int index)
            {
                if (index < 0 || index > 28)
                    throw new IndexOutOfRangeException("Index is out of range.");

                return string.Format("{0}0TX", Convert.ToChar(index + IndexBase));
            }
        }

        public class CloudLayer
        {
            public bool IsEnabled { get { return (weather.DisabledLayerFlags & layerBitMask) == 0; } set { SetEnabled(value); } }
            public string Texture { get { return weather.CloudTextures[layerNumber]; } set { weather.CloudTextures[layerNumber] = value; } }
            public float SpeedX { get { return GetSpeedX(); } set { SetSpeedX(value); } }
            public float SpeedY { get { return GetSpeedY(); } set { SetSpeedY(value); } }
            public ColorOctave Colors { get; private set; }
            public FloatOctave Alphas { get; private set; }

            readonly Wthr weather;
            readonly int layerNumber;
            readonly uint layerBitMask;

            public CloudLayer(Wthr weather, int layerNumber)
            {
                this.weather = weather;
                this.layerNumber = layerNumber;

                layerBitMask = (uint)1 << layerNumber;

                if (weather.CloudColors == null)
                {
                    weather.CloudColors = new ByteArray();
                    weather.CloudColors.Bytes = new byte[1024];
                }
                Colors = new ColorOctave(weather.CloudColors.Bytes, layerNumber * 32);

                if (weather.CloudAlphas == null)
                {
                    weather.CloudAlphas = new ByteArray();
                    weather.CloudAlphas.Bytes = new byte[1024];
                }
                Alphas = new FloatOctave(weather.CloudAlphas.Bytes, layerNumber * 32);
            }

            private void SetEnabled(bool value)
            {
                if (value)
                    weather.DisabledLayerFlags &= ~layerBitMask;
                else
                    weather.DisabledLayerFlags |= layerBitMask;
            }

            private void SetSpeedX(float value)
            {
                if (weather.CloudSpeedsX == null)
                {
                    weather.CloudSpeedsX = new ByteArray();
                    weather.CloudSpeedsX.Bytes = new byte[32];
                }
                weather.CloudSpeedsX.Bytes[layerNumber] = FloatToByteSpeed(value);
            }

            private float GetSpeedX()
            {
                if (weather.CloudSpeedsX == null)
                    return 0;
                else
                    return ByteToFloatSpeed(weather.CloudSpeedsX.Bytes[layerNumber]);
            }

            private void SetSpeedY(float value)
            {
                if (weather.CloudSpeedsY == null)
                {
                    weather.CloudSpeedsY = new ByteArray();
                    weather.CloudSpeedsY.Bytes = new byte[32];
                }
                weather.CloudSpeedsY.Bytes[layerNumber] = FloatToByteSpeed(value);
            }

            private float GetSpeedY()
            {
                if (weather.CloudSpeedsY == null)
                    return 0;
                else
                    return ByteToFloatSpeed(weather.CloudSpeedsY.Bytes[layerNumber]);
            }

            private float ByteToFloatSpeed(byte value)
            {
                return (float)Math.Round(value / 1270f - 0.1f, 4);
            }

            private byte FloatToByteSpeed(float value)
            {
                return (byte)Math.Min(255, Math.Max(0, (value + 0.1f) * 1270f));
            }
        }

        public class ColorOctave
        {
            public ColorAdapter Dawn { get; private set; }
            public ColorAdapter Day { get; private set; }
            public ColorAdapter Dusk { get; private set; }
            public ColorAdapter Night { get; private set; }
            public ColorAdapter EarlyDawn { get; private set; }
            public ColorAdapter LateDawn { get; private set; }
            public ColorAdapter EarlyDusk { get; private set; }
            public ColorAdapter LateDusk { get; private set; }

            public ColorOctave(byte[] buffer, int offset)
                : this(buffer, offset, 4)
            {
            }

            public ColorOctave(byte[] buffer, int offset, int stride)
            {
                Dawn = new ColorAdapter(buffer, offset);
                Day = new ColorAdapter(buffer, offset + stride);
                Dusk = new ColorAdapter(buffer, offset + stride * 2);
                Night = new ColorAdapter(buffer, offset + stride * 3);
                EarlyDawn = new ColorAdapter(buffer, offset + stride * 4);
                LateDawn = new ColorAdapter(buffer, offset + stride * 5);
                EarlyDusk = new ColorAdapter(buffer, offset + stride * 6);
                LateDusk = new ColorAdapter(buffer, offset + stride * 7);
            }
        }

        public class FloatOctave
        {
            public float Dawn { get { return GetAlpha(dawn); } set { SetAlpha(dawn, value); } }
            public float Day { get { return GetAlpha(day); } set { SetAlpha(day, value); } }
            public float Dusk { get { return GetAlpha(dusk); } set { SetAlpha(dusk, value); } }
            public float Night { get { return GetAlpha(night); } set { SetAlpha(night, value); } }
            public float EarlyDawn { get { return GetAlpha(earlyDawn); } set { SetAlpha(earlyDawn, value); } }
            public float LateDawn { get { return GetAlpha(lateDawn); } set { SetAlpha(lateDawn, value); } }
            public float EarlyDusk { get { return GetAlpha(earlyDusk); } set { SetAlpha(earlyDusk, value); } }
            public float LateDusk { get { return GetAlpha(lateDusk); } set { SetAlpha(lateDusk, value); } }

            readonly byte[] buffer;
            readonly int dawn;
            readonly int day;
            readonly int dusk;
            readonly int night;
            readonly int earlyDawn;
            readonly int lateDawn;
            readonly int earlyDusk;
            readonly int lateDusk;

            public FloatOctave(byte[] buffer, int offset)
                : this(buffer, offset, 4)
            {
            }

            public FloatOctave(byte[] buffer, int offset, int stride)
            {
                this.buffer = buffer;
                dawn = offset;
                day = offset + stride;
                dusk = offset + stride * 2;
                night = offset + stride * 3;
                earlyDawn = offset + stride * 4;
                lateDawn = offset + stride * 5;
                earlyDusk = offset + stride * 6;
                lateDusk = offset + stride * 7;
            }

            private float GetAlpha(int offset)
            {
                return BitConverter.ToSingle(buffer, offset);
            }

            private void SetAlpha(int offset, float value)
            {
                var temp = BitConverter.GetBytes(value);
                temp.CopyTo(buffer, offset);
            }
        }

        [Flags]
        public enum WeatherFlags : byte
        {
            None = 0,
            Pleasant = 0x01,
            Cloudy = 0x02,
            Rainy = 0x04,
            Snowy = 0x08,
            AlwaysVisible = 0x10,
            FollowsSunPosition = 0x20,
            WeatherTypeMask = 0x0f,
        }
    }
}
