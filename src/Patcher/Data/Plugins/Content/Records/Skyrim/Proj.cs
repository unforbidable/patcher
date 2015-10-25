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
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.PROJ)]
    public sealed class Proj : GenericFormRecord, IFeaturingObjectBounds
    {
        [Member(Names.OBND)]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.FULL)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string FullName { get; set; }

        [Member(Names.MODL, Names.MODS, Names.MODT)]
        [Initialize]
        private Model Model { get; set; }

        [Member(Names.DEST, Names.DSTD, Names.DSTF)]
        private DestructionData Destruction { get; set; }

        [Member(Names.DATA)]
        private ProjectileData Data { get; set; }

        [Member(Names.NAM1, Names.NAM2)]
        private MuzzleFlashModel MuzzleFlashModel { get; set; }

        [Member(Names.VNAM)]
        public SoundLevel SoundLevel { get; set; }

        class ProjectileData : Field
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

            bool decalDataNotLoaded;
            bool collisionLayerNotLoaded;

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
                throw new NotImplementedException();
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
