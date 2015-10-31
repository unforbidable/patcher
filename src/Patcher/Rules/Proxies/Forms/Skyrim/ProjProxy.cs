using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Data.Plugins.Content.Constants.Skyrim;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IProj))]
    public sealed class ProjProxy : FormProxy<Proj>, IProj
    {
        public bool CanBeDisabled
        {
            get
            {
                return record.HasFlag(ProjectileFlags.CanBeDisabled);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.CanBeDisabled, value);
            }
        }

        public bool CanBePickedUp
        {
            get
            {
                return record.HasFlag(ProjectileFlags.CanBePickedUp);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.CanBePickedUp, value);
            }
        }

        public bool CanPassThroughSmallTransparent
        {
            get
            {
                return record.HasFlag(ProjectileFlags.PassThroughSmallTransparent);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.PassThroughSmallTransparent, value);
            }
        }

        public IColl CollisionLayer
        {
            get
            {
                return Provider.CreateReferenceProxy<IColl>(record.CollisionLayer);
            }

            set
            {
                EnsureWritable();
                record.CollisionLayer = value.ToFormId();
            }
        }

        public float CollisionRadius
        {
            get
            {
                return record.CollisionRadius;
            }

            set
            {
                EnsureWritable();
                record.CollisionRadius = value;
            }
        }

        public float ConeSpread
        {
            get
            {
                return record.ConeSpread;
            }

            set
            {
                EnsureWritable();
                record.ConeSpread = value;
            }
        }

        public ITxst DecalData
        {
            get
            {
                return Provider.CreateReferenceProxy<ITxst>(record.DecalData);
            }

            set
            {
                EnsureWritable();
                record.DecalData = value.ToFormId();
            }
        }

        public IWeap DefaultWeaponSource
        {
            get
            {
                return Provider.CreateReferenceProxy<IWeap>(record.DefaultWeaponSource);
            }

            set
            {
                EnsureWritable();
                record.DefaultWeaponSource = value.ToFormId();
            }
        }

        public ISndr DisableSound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.DisableSound);
            }

            set
            {
                EnsureWritable();
                record.DisableSound = value.ToFormId();
            }
        }

        public bool IsExplosive
        {
            get
            {
                return record.HasFlag(ProjectileFlags.Explosion);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.Explosion, value);
            }
        }

        public bool ExplodesOnImpact
        {
            get
            {
                return !record.HasFlag(ProjectileFlags.AltExplosionTrigger);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.AltExplosionTrigger, !value);
            }
        }

        public IExpl Explosion
        {
            get
            {
                return Provider.CreateReferenceProxy<IExpl>(record.Explosion);
            }

            set
            {
                EnsureWritable();
                record.Explosion = value.ToFormId();
            }

        }

        public ISndr ExplosionCountdownSound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.ExplosionCountdownSound);
            }

            set
            {
                EnsureWritable();
                record.ExplosionCountdownSound = value.ToFormId();
            }

        }

        public float ExplosionProximity
        {
            get
            {
                return record.ExplosionProximity;
            }

            set
            {
                EnsureWritable();
                record.ExplosionProximity = value;
            }

        }

        public float ExplosionTimer
        {
            get
            {
                return record.ExplosionTimer;
            }

            set
            {
                EnsureWritable();
                record.ExplosionTimer = value;
            }

        }

        public float FadeDuration
        {
            get
            {
                return record.FadeDuration;
            }

            set
            {
                EnsureWritable();
                record.FadeDuration = value;
            }

        }

        public string FullName
        {
            get
            {
                return record.FullName;
            }

            set
            {
                EnsureWritable();
                record.FullName = value;
            }

        }

        public float Gravity
        {
            get
            {
                return record.Gravity;
            }

            set
            {
                EnsureWritable();
                record.Gravity = value;
            }

        }

        public bool HasMuzzleFlash
        {
            get
            {
                return record.HasFlag(ProjectileFlags.MuzzleFlash);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.MuzzleFlash, value);
            }
        }

        public float ImpactForce
        {
            get
            {
                return record.ImpactForce;
            }

            set
            {
                EnsureWritable();
                record.ImpactForce = value;
            }

        }

        public bool IsAutoAimEnabled
        {
            get
            {
                return !record.HasFlag(ProjectileFlags.DisableCombatAimCorrection);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.DisableCombatAimCorrection, !value);
            }
        }

        public bool IsHitScanEnabled
        {
            get
            {
                return record.HasFlag(ProjectileFlags.Hitscan);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.Hitscan, value);
            }
        }

        public bool IsSupersonic
        {
            get
            {
                return record.HasFlag(ProjectileFlags.Supersonic);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.Supersonic, value);
            }
        }

        public float Lifetime
        {
            get
            {
                return record.Lifetime;
            }

            set
            {
                EnsureWritable();
                record.Lifetime = value;
            }

        }

        public ILigh Light
        {
            get
            {
                return Provider.CreateReferenceProxy<ILigh>(record.Light);
            }

            set
            {
                EnsureWritable();
                record.Light = value.ToFormId();
            }

        }

        public float MuzzleFlashDuration
        {
            get
            {
                return record.MuzzleFlashDuration;
            }

            set
            {
                EnsureWritable();
                record.MuzzleFlashDuration = value;
            }

        }

        public ILigh MuzzleFlashLight
        {
            get
            {
                return Provider.CreateReferenceProxy<ILigh>(record.MuzzleFlashLight);
            }

            set
            {
                EnsureWritable();
                record.MuzzleFlashLight = value.ToFormId();
            }

        }

        public string MuzzleFlashModel
        {
            get
            {
                return record.MuzzleFlashModel;
            }

            set
            {
                EnsureWritable();
                record.MuzzleFlashModel = value;
            }

        }

        public IObjectBounds ObjectBounds
        {
            get
            {
                return record.CreateObjectBoundsProxy(this);
            }

            set
            {
                EnsureWritable();
                record.UpdateFromObjectBoundsProxy(value);
            }
        }

        public bool PinsLimbsOnCritical
        {
            get
            {
                return record.HasFlag(ProjectileFlags.PinsLimbsCriticalEffect);
            }

            set
            {
                EnsureWritable();
                record.SetFlag(ProjectileFlags.PinsLimbsCriticalEffect, value);
            }
        }

        public float Range
        {
            get
            {
                return record.Range;
            }

            set
            {
                EnsureWritable();
                record.Range = value;
            }

        }

        public float RelaunchInterval
        {
            get
            {
                return record.RelaunchInterval;
            }

            set
            {
                EnsureWritable();
                record.RelaunchInterval = value;
            }

        }

        public ISndr Sound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.Sound);
            }

            set
            {
                EnsureWritable();
                record.Sound = value.ToFormId();
            }

        }

        public SoundLevels SoundLevel
        {
            get
            {
                return record.SoundLevel.ToSoundLevels();
            }

            set
            {
                EnsureWritable();
                record.SoundLevel = value.ToSoundLevel();
            }
        }

        public float Speed
        {
            get
            {
                return record.Speed;
            }

            set
            {
                EnsureWritable();
                record.Speed = value;
            }

        }

        public float TracerChance
        {
            get
            {
                return record.TracerChance;
            }

            set
            {
                EnsureWritable();
                record.TracerChance = value;
            }

        }

        public ProjectileTypes Type
        {
            get
            {
                return record.Type.ToProjectileTypes();
            }

            set
            {
                EnsureWritable();
                record.Type = value.ToProjectileType();
            }
        }

        public string WorldModel
        {
            get
            {
                return record.WorldModel;
            }

            set
            {
                EnsureWritable();
                record.WorldModel = value;
            }

        }
    }
}
