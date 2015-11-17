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

using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Data.Plugins.Content.Fields;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.PROJ)]
    [Game(Games.Skyrim)]
    public sealed class Proj : GenericFormRecord, IFeaturingObjectBounds
    {
        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.MODL, Names.MODS, Names.MODT)]
        [Initialize]
        private Model ModelData { get; set; }

        [Member(Names.DEST, Names.DSTD, Names.DSTF)]
        private DestructionData Destruction { get; set; }

        [Member(Names.DATA)]
        private ProjectileData Data { get; set; }

        [Member(Names.NAM1, Names.NAM2)]
        private MuzzleFlashModel MuzzleFlashModelData { get; set; }

        [Member(Names.VNAM)]
        public SoundLevel SoundLevel { get; set; }

        public string WorldModel { get { return ModelData.Path; } set { ModelData.Path = value; } }

        public string MuzzleFlashModel
        {
            get
            {
                return MuzzleFlashModelData != null ? MuzzleFlashModelData.Path : null;
            }
            set
            {
                if (value != null)
                {
                    if (MuzzleFlashModelData == null)
                        MuzzleFlashModelData = new MuzzleFlashModel();
                    MuzzleFlashModelData.Path = value;
                }
                else
                {
                    MuzzleFlashModelData = null;
                }
            }
        }

        public void SetFlag(ProjectileFlags value, bool set)
        {
            if (set)
                Data.Flags |= value;
            else
                Data.Flags &= ~value;
        }

        public bool HasFlag(ProjectileFlags value)
        {
            return Data.Flags.HasFlag(value);
        }

        public ProjectileType Type { get { return Data.Type; } set { Data.Type = value; } }

        public float Gravity { get { return Data.Gravity; } set { Data.Gravity = value; } }
        public float Speed { get { return Data.Speed; } set { Data.Speed = value; } }
        public float Range { get { return Data.Range; } set { Data.Range = value; } }
        public float TracerChance { get { return Data.TracerChance; } set { Data.TracerChance = value; } }
        public float ExplosionProximity { get { return Data.AltExplosionTriggerProximity; } set { Data.AltExplosionTriggerProximity = value; } }
        public float ExplosionTimer { get { return Data.AltExplosionTriggerTimer; } set { Data.AltExplosionTriggerTimer = value; } }
        public float FadeDuration { get { return Data.FadeDuration; } set { Data.FadeDuration = value; } }
        public float MuzzleFlashDuration { get { return Data.MuzzleFlashDuration; } set { Data.MuzzleFlashDuration = value; } }
        public float ImpactForce { get { return Data.ImpactForce; } set { Data.ImpactForce = value; } }
        public float ConeSpread { get { return Data.ConeSpread; } set { Data.ConeSpread = value; } }
        public float CollisionRadius { get { return Data.CollisionRadius; } set { Data.CollisionRadius = value; } }
        public float Lifetime { get { return Data.Lifetime; } set { Data.Lifetime = value; } }
        public float RelaunchInterval { get { return Data.RelaunchInterval; } set { Data.RelaunchInterval = value; } }

        public uint Light { get { return Data.Light; } set { Data.Light = value; } }
        public uint MuzzleFlashLight { get { return Data.MuzzleFlashLight; } set { Data.MuzzleFlashLight = value; } }
        public uint Explosion { get { return Data.Explosion; } set { Data.Explosion = value; } }
        public uint Sound { get { return Data.Sound; } set { Data.Sound = value; } }
        public uint ExplosionCountdownSound { get { return Data.CountdownSound; } set { Data.CountdownSound = value; } }
        public uint DisableSound { get { return Data.DisableSound; } set { Data.DisableSound = value; } }
        public uint DefaultWeaponSource { get { return Data.DefaultWeaponSource; } set { Data.DefaultWeaponSource = value; } }
        public uint DecalData { get { return Data.DecalData; } set { Data.DecalData = value; } }
        public uint CollisionLayer { get { return Data.CollisionLayer; } set { Data.CollisionLayer = value; } }

        internal class ProjectileData : Field
        {
            public ProjectileFlags Flags { get; set; }
            public ProjectileType Type { get; set; }
            public float Gravity { get; set; }
            public float Speed { get; set; }
            public float Range { get; set; }
            public uint Light { get; set; }
            public uint MuzzleFlashLight { get; set; }
            public float TracerChance { get; set; }
            public float AltExplosionTriggerProximity { get; set; }
            public float AltExplosionTriggerTimer { get; set; }
            public uint Explosion { get; set; }
            public uint Sound { get; set; }
            public float MuzzleFlashDuration { get; set; }
            public float FadeDuration { get; set; }
            public float ImpactForce { get; set; }
            public uint CountdownSound { get; set; }
            public uint DisableSound { get; set; }
            public uint DefaultWeaponSource { get; set; }
            public float ConeSpread { get; set; }
            public float CollisionRadius { get; set; }
            public float Lifetime { get; set; }
            public float RelaunchInterval { get; set; }
            public uint DecalData { get; set; }
            public uint CollisionLayer { get; set; }

            bool decalDataNotLoaded = false;
            bool collisionLayerNotLoaded = false;

            internal override void ReadField(RecordReader reader)
            {
                Flags = (ProjectileFlags)reader.ReadUInt16();
                Type = (ProjectileType)reader.ReadUInt16();
                Gravity = reader.ReadSingle();
                Speed = reader.ReadSingle();
                Range = reader.ReadSingle();
                Light = reader.ReadReference(FormKindSet.Any);
                MuzzleFlashLight = reader.ReadReference(FormKindSet.Any);
                TracerChance = reader.ReadSingle();
                AltExplosionTriggerProximity = reader.ReadSingle();
                AltExplosionTriggerTimer = reader.ReadSingle();
                Explosion = reader.ReadReference(FormKindSet.Any);
                Sound = reader.ReadReference(FormKindSet.Any);
                MuzzleFlashDuration = reader.ReadSingle();
                FadeDuration = reader.ReadSingle();
                ImpactForce = reader.ReadSingle();
                CountdownSound = reader.ReadReference(FormKindSet.Any);
                DisableSound = reader.ReadReference(FormKindSet.Any);
                DefaultWeaponSource = reader.ReadReference(FormKindSet.Any);
                ConeSpread = reader.ReadSingle();
                CollisionRadius = reader.ReadSingle();
                Lifetime = reader.ReadSingle();
                RelaunchInterval = reader.ReadSingle();

                // The following field does not appear is some records
                // Example: FXDustProjectileMed
                if (!reader.IsEndOfSegment)
                    DecalData = reader.ReadReference(FormKindSet.Any);
                else
                    decalDataNotLoaded = true;

                // The following field does not appear in some records
                // Examples: EmptyProjectile, dunMarkarthWizard_SpiderControlProjectileFake
                if (!reader.IsEndOfSegment)
                    CollisionLayer = reader.ReadReference(FormKindSet.Any);
                else
                    collisionLayerNotLoaded = true;
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write((ushort)Flags);
                writer.Write((ushort)Type);
                writer.Write(Gravity);
                writer.Write(Speed);
                writer.Write(Range);
                writer.WriteReference(Light, FormKindSet.Any);
                writer.WriteReference(MuzzleFlashLight, FormKindSet.Any);
                writer.Write(TracerChance);
                writer.Write(AltExplosionTriggerProximity);
                writer.Write(AltExplosionTriggerTimer);
                writer.WriteReference(Explosion, FormKindSet.Any);
                writer.WriteReference(Sound, FormKindSet.Any);
                writer.Write(MuzzleFlashDuration);
                writer.Write(FadeDuration);
                writer.Write(ImpactForce);
                writer.WriteReference(CountdownSound, FormKindSet.Any);
                writer.WriteReference(DisableSound, FormKindSet.Any);
                writer.WriteReference(DefaultWeaponSource, FormKindSet.Any);
                writer.Write(ConeSpread);
                writer.Write(CollisionRadius);
                writer.Write(Lifetime);
                writer.Write(RelaunchInterval);

                // Write this field only if it was loaded, or if value is assigned to it
                // Or the next optional value is assigned
                if (!decalDataNotLoaded || DecalData != 0 || CollisionLayer != 0)
                    writer.WriteReference(DecalData, FormKindSet.Any);

                // Write this field only if it was loaded, or if value is assigned to it
                if (!collisionLayerNotLoaded || CollisionLayer != 0)
                    writer.WriteReference(CollisionLayer, FormKindSet.Any);
            }

            public override Field CopyField()
            {
                return new ProjectileData()
                {
                    Flags = Flags,
                    Type = Type,
                    Gravity = Gravity,
                    Speed = Speed,
                    Range = Range,
                    Light = Light,
                    MuzzleFlashLight = MuzzleFlashLight,
                    TracerChance = TracerChance,
                    AltExplosionTriggerProximity = AltExplosionTriggerProximity,
                    AltExplosionTriggerTimer = AltExplosionTriggerTimer,
                    Explosion = Explosion,
                    Sound = Sound,
                    MuzzleFlashDuration = MuzzleFlashDuration,
                    FadeDuration = FadeDuration,
                    ImpactForce = ImpactForce,
                    CountdownSound = CountdownSound,
                    DisableSound = DisableSound,
                    DefaultWeaponSource = DefaultWeaponSource,
                    ConeSpread = ConeSpread,
                    CollisionRadius = CollisionRadius,
                    Lifetime = Lifetime,
                    RelaunchInterval = RelaunchInterval,
                    DecalData = DecalData,
                    CollisionLayer = CollisionLayer
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (ProjectileData)other;
                return Flags == cast.Flags && Type == cast.Type && Gravity == cast.Gravity && Speed == cast.Speed && Range == cast.Range &&
                    Light == cast.Light && MuzzleFlashLight == cast.MuzzleFlashLight && TracerChance == cast.TracerChance &&
                    AltExplosionTriggerProximity == cast.AltExplosionTriggerProximity && AltExplosionTriggerTimer == cast.AltExplosionTriggerTimer &&
                    Explosion == cast.Explosion && Sound == cast.Sound && MuzzleFlashDuration == cast.MuzzleFlashDuration && FadeDuration == cast.FadeDuration &&
                    ImpactForce == cast.ImpactForce && CountdownSound == cast.CountdownSound && DisableSound == cast.DisableSound &&
                    DefaultWeaponSource == cast.DefaultWeaponSource && ConeSpread == cast.ConeSpread && CollisionRadius == cast.CollisionRadius &&
                    Lifetime == cast.Lifetime && RelaunchInterval == cast.RelaunchInterval && DecalData == cast.DecalData && CollisionLayer == cast.CollisionLayer;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Light;
                yield return MuzzleFlashLight;
                yield return Explosion;
                yield return Sound;
                yield return CountdownSound;
                yield return DisableSound;
                yield return DefaultWeaponSource;
                yield return DecalData;
                yield return CollisionLayer;
            }

            public override string ToString()
            {
                return Type.ToString();
            }
        }
    }
}
