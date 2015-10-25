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

using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Projectile</b> form.
    /// </summary>
    public interface IProj : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Projectile</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }

        /// <summary>
        /// Gets or sets the in-game name of this <b>Projectile</b>.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Projectile</b>.
        /// </summary>
        string WorldModel { get; set; }

        /// <summary>
        /// Gets or sets the type of this <b>Projectile</b>.
        /// </summary>
        ProjectileTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the sound level of this <b>Projectile</b> in regards to detection.
        /// </summary>
        SoundLevels SoundLevel { get; set; }

        /// <summary>
        /// Gets or sets the travel speed of this <b>Projectile</b>.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// Gets or sets the gravitation pull applied on this <b>Projectile</b>.
        /// </summary>
        float Gravity { get; set; }

        /// <summary>
        /// Gets or sets the maximum range this <b>Projectile</b> can travel.
        /// </summary>
        float Range { get; set; }

        /// <summary>
        /// Gets or sets the force caused by the impact of this <b>Projectile</b>.
        /// </summary>
        float ImpactForce { get; set; }

        /// <summary>
        /// Gets or sets the chance a tracer will be drawn for this <b>Projectile</b>.
        /// </summary>
        float TracerChance { get; set; }

        /// <summary>
        /// Gets or sets the beam fade duration of this <b>Projectile</b>.
        /// </summary>
        float FadeDuration { get; set; }

        /// <summary>
        /// Gets or sets the spread of the effective cone caused by this <b>Projectile</b>. 
        /// </summary>
        float ConeSpread { get; set; }

        /// <summary>
        /// Gets or sets the collision radius of this <b>Projectile</b>.
        /// </summary>
        float CollisionRadius { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of this <b>Projectile</b>.
        /// </summary>
        float Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the interval at which this <b>Projectile</b> can be launched again.
        /// </summary>
        float RelaunchInterval { get; set; }

        /// <summary>
        /// Gets or sets the <b>Light</b> caused by this <b>Projectile</b>.
        /// </summary>
        IForm Light { get; set; }

        /// <summary>
        /// Gets or sets the default weapon source of this <b>Projectile</b>.
        /// </summary>
        IForm DefaultWeaponSource { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> caused by this <b>Projectile</b>.
        /// </summary>
        IForm Sound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> which is played when this <b>Projectile</b> is disabled.
        /// </summary>
        IForm DisableSound { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> can be disabled.
        /// </summary>
        bool CanBeDisabled { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> travels at a supersonic speed.
        /// </summary>
        bool IsSupersonic { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> can be picked up.
        /// </summary>
        bool CanBePickedUp { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> pins limbs when critical effect is triggered.
        /// </summary>
        bool PinsLimbsOnCritical { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> can pass through small transparent objects.
        /// </summary>
        bool CanPassThroughSmallTransparent { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the auto-aim is enabled for this <b>Projectile</b>.
        /// </summary>
        bool IsAutoAimEnabled { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the hit scan is enabled for this <b>Projectile</b>.
        /// </summary>
        bool IsHitScanEnabled { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> is explosive.
        /// </summary>
        bool IsExplosive { get; set; }

        /// <summary>
        /// Gets or sets the <b>Explosion</b> caused by this <b>Projectile</b>.
        /// </summary>
        IForm Explosion { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the explosion caused by this <b>Projectile</b> is triggered on the impact.
        /// </summary>
        bool ExplodesOnImpact { get; set; }

        /// <summary>
        /// Gets or sets the timer which triggers the explosion of this <b>Projectile</b>.
        /// </summary>
        float ExplosionTimer { get; set; }

        /// <summary>
        /// Gets or sets the proximity which triggers the explosion of this <b>Projectile</b>.
        /// </summary>
        float ExplosionProximity { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> of the countdown timer that triggers the explosion of this <b>Projectile</b>.
        /// </summary>
        IForm ExplosionCountdownSound { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Projectile</b> causes a muzzle flash.
        /// </summary>
        bool HasMuzzleFlash { get; set; }

        /// <summary>
        /// Gets or sets the muzzle flash <b>Light</b> caused by this <b>Projectile</b>.
        /// </summary>
        IForm MuzzleFlashLight { get; set; }

        /// <summary>
        /// Gets or sets the path to the model (.nif file) of the muzzle flash caused by this <b>Projectile</b>.
        /// </summary>
        string MuzzleFlashModel { get; set; }

        /// <summary>
        /// Gets or sets the duration of the muzzle flash caused by this <b>Projectile</b>.
        /// </summary>
        float MuzzleFlashDuration { get; set; }

        /// <summary>
        /// Gets or sets the decal <b>Texture</b> of this <b>Projectile</b>.
        /// </summary>
        IForm DecalData { get; set; }

        /// <summary>
        /// Gets or sets the <b>Collision</b> layer of this <b>Projectile</b>.
        /// </summary>
        IForm CollisionLayer { get; set; }
    }
}
