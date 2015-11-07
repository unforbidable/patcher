﻿/// Copyright(C) 2015 Unforbidable Works
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

using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Data.Plugins.Content.Fields;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.WTHR)]
    public sealed class Wthr : GenericFormRecord
    {
        [Member(".0TX")]
        [Initialize]
        private WeatherTextures CloudTextures { get; set; }

        [Member(Names.LNAM)]
        private int CloudTextureCount { get; set; }

        [Member(Names.MNAM)]
        [Reference(Names.SPGD)]
        public uint Precipitation { get; set; }

        [Member(Names.NNAM)]
        [Reference(Names.RFCT)]
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
        public List<uint> SkyStatics { get; set; }

        [Member(Names.IMSP)]
        private ImageSpaceData ImageSpace { get; set; }

        [Member(Names.DALC)]
        [Initialize]
        private List<ByteArray> LightDataParts { get; set; }

        [Member(Names.MODL, Names.MODT, Names.MODT)]
        private Model Aurora { get; set; }

        [Member(Names.NAM2)]
        private ByteArray Unused1 { get; set; }

        [Member(Names.NAM3)]
        private ByteArray Unused2 { get; set; }

        [Member(Names.DNAM)]
        private string CloudTexture0 { get; set; }

        [Member(Names.CNAM)]
        private string CloudTexture1 { get; set; }

        [Member(Names.ANAM)]
        private string CloudTexture2 { get; set; }

        [Member(Names.BNAM)]
        private string CloudTexture3 { get; set; }

        [Member(Names.ONAM)]
        private ByteArray AltCloudSpeedsX { get; set; }

        private byte[] allLightData = new byte[128];

        public CloudLayer GetCloudLayer(int layer)
        {
            return new CloudLayer(this, layer);
        }

        public IEnumerable<CloudLayer> GetCloudLayers()
        {
            return Enumerable.Range(0, 32).Select(i => GetCloudLayer(i));
        }

        public ColorQuad GetColor(int component)
        {
            return new ColorQuad(Colors.Bytes, component * 16);
        }

        public IEnumerable<ColorQuad> GetColors()
        {
            return Enumerable.Range(0, 17).Select(i => GetColor(i));
        }

        public AmbientLightData GetAmbientLightData()
        {
            return new AmbientLightData(this);
        }

        protected override void AfterRead(RecordReader reader)
        {
            // Ensure ComponentColors are full size
            if (Colors.Bytes.Length < 272)
            {
                byte[] temp = new byte[272];
                Colors.Bytes.CopyTo(temp, 0);
                Colors.Bytes = temp;
            }

            // Copy LightData into a single array
            for (int i = 0; i < 4; i++)
                LightDataParts[i].Bytes.CopyTo(allLightData, i * 32);
        }

        void EnsureFogDataCreated()
        {
            if (Fog == null)
                Fog = new FogData();
        }

        public float FogDayNear { get { return Fog == null ? 0f : Fog.DayNear; } set { EnsureFogDataCreated(); Fog.DayNear = value; } }
        public float FogDayFar { get { return Fog == null ? 0f : Fog.DayFar; } set { EnsureFogDataCreated(); Fog.DayFar = value; } }
        public float FogNightNear { get { return Fog == null ? 0f : Fog.NightNear; } set { EnsureFogDataCreated(); Fog.NightNear = value; } }
        public float FogNightFar { get { return Fog == null ? 0f : Fog.NightFar; } set { EnsureFogDataCreated(); Fog.NightFar = value; } }
        public float DayPow { get { return Fog == null ? 0f : Fog.DayPow; } set { EnsureFogDataCreated(); Fog.DayPow = value; } }
        public float FogNightPow { get { return Fog == null ? 0f : Fog.NightPow; } set { EnsureFogDataCreated(); Fog.NightPow = value; } }
        public float FogDayMax { get { return Fog == null ? 0f : Fog.DayMax; } set { EnsureFogDataCreated(); Fog.DayMax = value; } }
        public float FogNightMax { get { return Fog == null ? 0f : Fog.NightMax; } set { EnsureFogDataCreated(); Fog.NightMax = value; } }

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

        void EnsureImageSpaceCreated()
        {
            if (ImageSpace == null)
                ImageSpace = new ImageSpaceData();
        }

        public uint SunriseImageSpace { get { return ImageSpace == null ? 0 : ImageSpace.Sunrise; } set { EnsureImageSpaceCreated(); ImageSpace.Sunrise = value; } }
        public uint DayImageSpace { get { return ImageSpace == null ? 0 : ImageSpace.Day; } set { EnsureImageSpaceCreated(); ImageSpace.Day = value; } }
        public uint SunsetImageSpace { get { return ImageSpace == null ? 0 : ImageSpace.Sunset; } set { EnsureImageSpaceCreated(); ImageSpace.Sunset = value; } }
        public uint NightImageSpace { get { return ImageSpace == null ? 0 : ImageSpace.Night; } set { EnsureImageSpaceCreated(); ImageSpace.Night = value; } }

        sealed class ImageSpaceData : Field
        {
            public uint Sunrise { get; set; }
            public uint Day { get; set; }
            public uint Sunset { get; set; }
            public uint Night { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Sunrise = reader.ReadReference(FormKindSet.ImgsOnly);
                Day = reader.ReadReference(FormKindSet.ImgsOnly);
                Sunset = reader.ReadReference(FormKindSet.ImgsOnly);
                Night = reader.ReadReference(FormKindSet.ImgsOnly);
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
        public WeatherFlags Flags { get { return Data.Flags; } set { Data.Flags = value; } }
        public ColorAdapter LightningColor { get { if (lightningColor == null) lightningColor = new ColorAdapter(Data.LightningColor); return lightningColor; } }
        public float WindDirection { get { return Data.WindDirection; } set { Data.WindDirection = value; } }
        public float WindDirectionRange { get { return Data.WindDirectionRange; } set { Data.WindDirectionRange = value; } }

        sealed class WeatherData : Field
        {
            public float WindSpeed { get; set; }
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
            public float WindDirection { get; set; }
            public float WindDirectionRange { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                WindDirection = reader.ReadByte() / 255f;
                reader.ReadInt16(); // Skip 2 bytes
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
                reader.ReadByte(); // Skip 1 byte
                WindDirection = reader.ReadByte() * 360f / 255f;
                WindDirectionRange = reader.ReadByte() * 180f / 255f;
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

        public class AmbientLightData
        {
            public ColorQuad X1 { get; private set; }
            public ColorQuad X2 { get; private set; }
            public ColorQuad Y1 { get; private set; }
            public ColorQuad Y2 { get; private set; }
            public ColorQuad Z1 { get; private set; }
            public ColorQuad Z2 { get; private set; }
            public ColorQuad Specular { get; private set; }
            public FloatQuad FresnelPower { get; private set; }

            public AmbientLightData(Wthr weather)
            {
                X2 = new ColorQuad(weather.allLightData, 0, 32, 64, 96);
                X1 = new ColorQuad(weather.allLightData, 4, 4 + 32, 4 + 64, 4 + 96);
                Y2 = new ColorQuad(weather.allLightData, 8, 8 + 32, 8 + 64, 8 + 96);
                Y1 = new ColorQuad(weather.allLightData, 12, 12 + 32, 12 + 64, 12 + 96);
                Z2 = new ColorQuad(weather.allLightData, 16, 16 + 32, 16 + 64, 16 + 96);
                Z1 = new ColorQuad(weather.allLightData, 20, 20 + 32, 20 + 64, 20 + 96);
                Specular = new ColorQuad(weather.allLightData, 24, 24 + 32, 24 + 64, 24 + 96);
                FresnelPower = new FloatQuad(weather.allLightData, 28, 28 + 32, 28 + 64, 28 + 96);
            }
        }

        public class CloudLayer
        {
            public bool IsEnabled { get { return (weather.DisabledLayerFlags & layerBitMask) == 0; } set { SetEnabled(value); } }
            public string Texture { get { return weather.CloudTextures[layerNumber]; } set { weather.CloudTextures[layerNumber] = value; } }
            public float SpeedX { get { return GetSpeedX(); } set { SetSpeedX(value); } }
            public float SpeedY { get { return GetSpeedY(); } set { SetSpeedY(value); } }
            public ColorQuad Colors { get; private set; }
            public FloatQuad Alphas { get; private set; }

            readonly Wthr weather;
            readonly int layerNumber;
            readonly uint layerBitMask;

            public CloudLayer(Wthr weather, int layerNumber)
            {
                this.weather = weather;
                this.layerNumber = layerNumber;

                layerBitMask = (uint)1 << layerNumber;

                Colors = new ColorQuad(weather.CloudColors.Bytes, layerNumber * 16);
                Alphas = new FloatQuad(weather.CloudAlphas.Bytes, layerNumber * 16);
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

        public class ColorQuad
        {
            public ColorAdapter Sunrise { get; private set; }
            public ColorAdapter Day { get; private set; }
            public ColorAdapter Sunset { get; private set; }
            public ColorAdapter Night { get; private set; }

            public ColorQuad(byte[] buffer, int offset)
                : this(buffer, offset, offset + 4, offset + 8, offset + 12)
            {
            }

            public ColorQuad(byte[] buffer, int sunrise, int day, int sunset, int night)
            {
                Sunrise = new ColorAdapter(buffer, sunrise);
                Day = new ColorAdapter(buffer, day);
                Sunset = new ColorAdapter(buffer, sunset);
                Night = new ColorAdapter(buffer, night);
            }
        }

        public class FloatQuad
        {
            public float Sunrise { get { return GetAlpha(sunrise); } set { SetAlpha(sunrise, value); } }
            public float Day { get { return GetAlpha(day); } set { SetAlpha(day, value); } }
            public float Sunset { get { return GetAlpha(sunset); } set { SetAlpha(sunset, value); } }
            public float Night { get { return GetAlpha(night); } set { SetAlpha(night, value); } }

            readonly byte[] buffer;
            readonly int sunrise;
            readonly int day;
            readonly int sunset;
            readonly int night;

            public FloatQuad(byte[] buffer, int offset)
                : this(buffer, offset, offset + 4, offset + 8, offset + 12)
            {
            }

            public FloatQuad(byte[] buffer, int sunrise, int day, int sunset, int night)
            {
                this.buffer = buffer;
                this.sunrise = sunrise;
                this.day = day;
                this.sunset = sunset;
                this.night = night;
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
    }
}