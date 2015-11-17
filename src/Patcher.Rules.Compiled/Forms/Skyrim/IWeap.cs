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
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Weapon</b> form.
    /// </summary>
    public interface IWeap : IForm
    {       
        /// <summary>
        /// Gets or sets the <see cref="IScriptCollection"/> containing <see cref="IScript">Scripts</see> attached to this <b>Weapon</b>.
        /// </summary>
        IScriptCollection Scripts { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Weapon</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }

        /// <summary>
        /// Gets or sets the in-game name of this <b>Weapon</b>.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the <b>Enchantment</b> of this <b>Weapon</b>.
        /// </summary>
        IEnch Enchantment { get; set; }

        /// <summary>
        /// Gets or sets the charge of the enchantment of this <b>Weapon</b>.
        /// </summary>
        int EnchantmentCharge { get; set; }

        /// <summary>
        /// Gets or sets the value of this <b>Weapon</b>.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the weight of this <b>Weapon</b>.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Gets or sets the damage caused by this <b>Weapon</b>.
        /// </summary>
        int Damage { get; set; }

        /// <summary>
        /// Gets or sets the number of projectiles launched when a single <b>Ammo</b> object is fired with this <b>Weapon</b>.
        /// </summary>
        int NumberOfProjectiles { get; set; }

        /// <summary>
        /// Gets or sets the additional damage caused by this <b>Weapon</b> on a critical hit.
        /// </summary>
        int CriticalDamage { get; set; }

        /// <summary>
        /// Gets or sets the critical chance multiplier of this <b>Weapon</b>.
        /// </summary>
        float CriticalChanceMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the amount of stagger applied on a hit by this <b>Weapon</b>.
        /// </summary>
        float Stagger { get; set; }

        /// <summary>
        /// Gets or sets the reach of this <b>Weapon</b>.
        /// </summary>
        float Reach { get; set; }

        /// <summary>
        /// Gets or sets the speed of this <b>Weapon</b>.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// Gets or sets the minimal effective range of this <b>Weapon</b>.
        /// </summary>
        float MinRange { get; set; }

        /// <summary>
        /// Gets or sets the maximal effective range of this <b>Weapon</b>.
        /// </summary>
        float MaxRange { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the AI is to ignore combat style multipliers for this <b>Weapon</b>.
        /// </summary>
        bool IsRangeFixed { get; set; }

        /// <summary>
        /// Gets or sets the <b>Weapon</b> that will be used as the template.
        /// </summary>
        IWeap TemplateWeapon { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Weapon</b>.
        /// </summary>
        string WorldModel { get; set; }

        /// <summary>
        /// Gets or sets the <b>Static</b> object that will be shown when this <b>Weapon</b> is used in first person.
        /// </summary>
        IStat FirstPersonModel { get; set; }

        /// <summary>
        /// Gets or sets the path to the inventory image (.dds file) that will be used for this <b>Weapon</b>.
        /// </summary>
        string InventoryImage { get; set; }

        /// <summary>
        /// Gets or sets the path to the message icon (.dds file) that will be used for this <b>Weapon</b>.
        /// </summary>
        string MessageIcon { get; set; }

        /// <summary>
        /// Gets or sets the <b>Impact Data Set</b> used when attacking with this <b>Weapon</b>.
        /// </summary>
        IIpds ImpactDataSet { get; set; }

        /// <summary>
        /// Gets or sets the <b>Impact Data Set</b> used when bashing or blocking with this <b>Weapon</b>.
        /// </summary>
        IIpds BlockImpactDataSet { get; set; }

        /// <summary>
        /// Gets or sets the alternate <b>Material</b> used when bashing or blocking with this <b>Weapon</b>.
        /// </summary>
        IMatt AlternateBlockMaterial { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is picked up.
        /// </summary>
        ISndr PickUpSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is put down.
        /// </summary>
        ISndr PutDownSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is used.
        /// </summary>
        ISndr AttackSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is used in first person.
        /// </summary>
        ISndr FirstPersonAttackSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> loop that plays when this <b>Weapon</b> is used.
        /// </summary>
        ISndr AttackLoopSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> misses or runs out of ammo.
        /// </summary>
        ISndr AttackFailSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is idle.
        /// </summary>
        ISndr IdleSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is equipped.
        /// </summary>
        ISndr EquipSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Weapon</b> is unequipped.
        /// </summary>
        ISndr UnequipSound { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Skills">Skill</see> that will be used when this <b>Weapon</b> is wielded.
        /// </summary>
        Skills Skill { get; set; }

        /// <summary>
        /// Gets or sets the <b>Equip Type</b> of this <b>Weapon</b>.
        /// </summary>
        IEqup EquipType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IKywd}"/> containing <b>Keywords</b> associated with this <b>Weapon</b>.
        /// </summary>
        IFormCollection<IKywd> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the description of this <b>Weapon</b> that will override the enchantment description if provided.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> can be used by the player character.
        /// </summary>
        bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets the sound level of this <b>Weapon</b> in regards to detection.
        /// </summary>
        SoundLevels SoundLevel { get; set; }

        /// <summary>
        /// Gets or sets the attack animation of this <b>Weapon</b>.
        /// </summary>
        Animations AttackAnimation { get; set; }

        /// <summary>
        /// Gets or sets the increase or decrease of the speed of the attack animations used by this <b>Weapon</b>.
        /// </summary>
        float AttackAnimationMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the type of this <b>Weapon</b>.
        /// </summary>
        WeaponTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the kind of resistence that can resist the damage caused by this <b>Weapon</b>.
        /// </summary>
        Resistances Resistence { get; set; }

        /// <summary>
        /// Gets or sets the critical effect e.i. <b>Spell</b> caused by this <b>Weapon</b>.
        /// </summary>
        ISpel CriticalEffect { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether the critical effect of this <b>Weapon</b> is always applied on death.
        /// </summary>
        bool IsCriticalEffectTriggeredOnDeath { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> can cause a limb dismemberment.
        /// </summary>
        bool CanCauseDismemberment { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> can cause a limb to explode.
        /// </summary>
        bool CanCauseLimbExplosion { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether NPCs need ammo while using this <b>Weapon</b> (bows only).
        /// </summary>
        bool IsAmmoUsed { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> can be put down.
        /// </summary>
        bool CanDrop { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether NPCs can use this <b>Weapon</b> during normal combat.
        /// </summary>
        bool CanUseInNormalCombat { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> is hidden when sheathed.
        /// </summary>
        bool IsBackpackHidden { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether attacking someone with this <b>Weapon</b> is considered a minor crime only (i.e. not an assault).
        /// </summary>
        bool IsMinorCrime { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether attacking someone with this <b>Weapon</b> does not cause the target become hostile.
        /// </summary>
        bool IsNonHostile { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> can be used and picked up by the player character only.
        /// </summary>
        bool IsPlayerOnly { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Weapon</b> has been conjured.
        /// </summary>
        bool IsBoundWeapon { get; set; }
    }
}
