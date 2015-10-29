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

using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IWeap))]
    public sealed class WeapProxy : FormProxy<Weap>, IWeap
    {
        public IForm AlternateBlockMaterial
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.AlternateBlockMaterial);
            }
            set
            {
                EnsureWritable();
                record.AlternateBlockMaterial = value.ToFormId();
            }
        }

        public Animations AttackAnimation
        {
            get
            {
                return record.AttackAnimation.ToAnimations();
            }

            set
            {
                EnsureWritable();
                record.AttackAnimation = value.ToAttackAnimation();
            }
        }

        public float AttackAnimationMultiplier
        {
            get
            {
                return record.AttackAnimationMultiplier;
            }

            set
            {
                EnsureWritable();
                record.AttackAnimationMultiplier = value;
            }
        }

        public IForm AttackFailSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.AttackFailSound);
            }
            set
            {
                EnsureWritable();
                record.AttackFailSound = value.ToFormId();
            }
        }

        public IForm AttackLoopSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.AttackLoopSound);
            }
            set
            {
                EnsureWritable();
                record.AttackLoopSound = value.ToFormId();
            }
        }

        public IForm AttackSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.AttackSound);
            }
            set
            {
                EnsureWritable();
                record.AttackSound = value.ToFormId();
            }
        }

        public IForm BlockImpactDataSet
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.BlockImpactDataSet);
            }
            set
            {
                EnsureWritable();
                record.BlockImpactDataSet = value.ToFormId();
            }
        }

        public bool CanCauseDismemberment
        {
            get
            {
                return record.CanCauseDismemberment;
            }

            set
            {
                EnsureWritable();
                record.CanCauseDismemberment = value;
            }
        }

        public bool CanCauseLimbExplosion
        {
            get
            {
                return record.CanCauseLimbExplosion;
            }

            set
            {
                EnsureWritable();
                record.CanCauseLimbExplosion = value;
            }
        }

        public bool CanDrop
        {
            get
            {
                return record.CanDrop;
            }

            set
            {
                EnsureWritable();
                record.CanDrop = value;
            }
        }

        public float CriticalChanceMultiplier
        {
            get
            {
                return record.CriticalChanceMultiplier;
            }

            set
            {
                EnsureWritable();
                record.CriticalChanceMultiplier = value;
            }
        }

        public int CriticalDamage
        {
            get
            {
                return record.CriticalDamage;
            }

            set
            {
                EnsureWritable();
                record.CriticalDamage = (ushort)value;
            }
        }

        public IForm CriticalEffect
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.CriticalEffect);
            }
            set
            {
                EnsureWritable();
                record.CriticalEffect = value.ToFormId();
            }
        }

        public int Damage
        {
            get
            {
                return record.Damage;
            }

            set
            {
                EnsureWritable();
                record.Damage = (short)value;
            }
        }

        public string Description
        {
            get
            {
                return record.Description;
            }

            set
            {
                EnsureWritable();
                record.Description = value;
            }
        }

        public IForm Enchantment
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.Enchantment);
            }
            set
            {
                EnsureWritable();
                record.Enchantment = value.ToFormId();
            }
        }

        public int EnchantmentCharge
        {
            get
            {
                return record.EnchantmentAmount.HasValue ? (int)record.EnchantmentAmount : 0;
            }

            set
            {
                EnsureWritable();
                record.EnchantmentAmount = (ushort)value;
            }
        }

        public IForm EquipSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.EquipSound);
            }
            set
            {
                EnsureWritable();
                record.EquipSound = value.ToFormId();
            }
        }

        public IForm EquipType
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.EquipType);
            }
            set
            {
                EnsureWritable();
                record.EquipType = value.ToFormId();
            }
        }

        public IForm FirstPersonAttackSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.AttackSound2D);
            }
            set
            {
                EnsureWritable();
                record.AttackSound2D = value.ToFormId();
            }
        }

        public IStat FirstPersonModel
        {
            get
            {
                return Provider.CreateReferenceProxy<IStat>(record.FirstPersonModel);
            }
            set
            {
                EnsureWritable();
                record.FirstPersonModel = value.ToFormId();
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

        public IForm IdleSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.IdleSound);
            }
            set
            {
                EnsureWritable();
                record.IdleSound = value.ToFormId();
            }
        }

        public IForm ImpactDataSet
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.ImpactDataSet);
            }
            set
            {
                EnsureWritable();
                record.ImpactDataSet = value.ToFormId();
            }
        }

        public string InventoryImage
        {
            get
            {
                return record.InventoryImage;
            }

            set
            {
                EnsureWritable();
                record.InventoryImage = value;
            }
        }

        public bool IsAmmoUsed
        {
            get
            {
                return record.IsAmmoUsed;
            }

            set
            {
                EnsureWritable();
                record.IsAmmoUsed = value;
            }
        }

        public bool IsBackpackHidden
        {
            get
            {
                return record.IsBackpackHidden;
            }

            set
            {
                EnsureWritable();
                record.IsBackpackHidden = value;
            }
        }

        public bool IsBoundWeapon
        {
            get
            {
                return record.IsBoundWeapon;
            }

            set
            {
                EnsureWritable();
                record.IsBoundWeapon = value;
            }
        }

        public bool IsCriticalEffectTriggeredOnDeath
        {
            get
            {
                return record.IsCriticalEffectTriggeredOnDeath;
            }

            set
            {
                EnsureWritable();
                record.IsCriticalEffectTriggeredOnDeath = value;
            }
        }

        public bool IsMinorCrime
        {
            get
            {
                return record.IsMinorCrime;
            }

            set
            {
                EnsureWritable();
                record.IsMinorCrime = value;
            }
        }

        public bool IsNonHostile
        {
            get
            {
                return record.IsNonHostile;
            }

            set
            {
                EnsureWritable();
                record.IsNonHostile = value;
            }
        }

        public bool IsPlayable
        {
            get
            {
                return record.IsPlayable;
            }

            set
            {
                EnsureWritable();
                record.IsPlayable = value;
            }
        }

        public bool IsPlayerOnly
        {
            get
            {
                return record.IsPlayerOnly;
            }

            set
            {
                EnsureWritable();
                record.IsPlayerOnly = value;
            }
        }

        public bool IsRangeFixed
        {
            get
            {
                return record.IsRangeFixed;
            }

            set
            {
                EnsureWritable();
                record.IsRangeFixed = value;
            }
        }

        public bool CanUseInNormalCombat
        {
            get
            {
                return record.CanUseInNormalCombat;
            }

            set
            {
                EnsureWritable();
                record.CanUseInNormalCombat = value;
            }
        }

        public IFormCollection<IKywd> Keywords
        {
            get
            {
                return Provider.CreateFormCollectionProxy<IKywd>(Mode, record.Keywords.Items);
            }
            set
            {
                EnsureWritable();
                record.Keywords.Items = value.ToFormIdList();
            }
        }

        public float MaxRange
        {
            get
            {
                return record.MaxRange;
            }

            set
            {
                EnsureWritable();
                record.MaxRange = value;
            }
        }

        public string MessageIcon
        {
            get
            {
                return record.MessageIcon;
            }

            set
            {
                EnsureWritable();
                record.MessageIcon = value;
            }
        }

        public float MinRange
        {
            get
            {
                return record.MinRange;
            }

            set
            {
                EnsureWritable();
                record.MinRange = value;
            }
        }

        public int NumberOfProjectiles
        {
            get
            {
                return record.NumberOfProjectiles;
            }

            set
            {
                EnsureWritable();
                record.NumberOfProjectiles = (byte)value;
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

        public IForm PickUpSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.PickUpSound);
            }
            set
            {
                EnsureWritable();
                record.PickUpSound = value.ToFormId();
            }
        }

        public IForm PutDownSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.PutDownSound);
            }
            set
            {
                EnsureWritable();
                record.PutDownSound = value.ToFormId();
            }
        }

        public float Reach
        {
            get
            {
                return record.Reach;
            }

            set
            {
                EnsureWritable();
                record.Reach = value;
            }

        }

        public Resistances Resistence
        {
            get
            {
                return record.Resist.ToResistance();
            }

            set
            {
                EnsureWritable();
                record.Resist = value.ToActorValue();
            }
        }

        public IScriptCollection Scripts
        {
            get
            {
                return record.CreateVirtualMachineAdapterProxy(this);
            }
            set
            {
                EnsureWritable();
                record.UpdateFromVirtualMachineAdapterProxy(value);
            }
        }

        public Skills Skill
        {
            get
            {
                return record.Skill.ToSkill();
            }

            set
            {
                EnsureWritable();
                record.Skill = value.ToActorValue();
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

        public float Stagger
        {
            get
            {
                return record.Stagger;
            }

            set
            {
                EnsureWritable();
                record.Stagger = value;
            }

        }

        public IWeap TemplateWeapon
        {
            get
            {
                return Provider.CreateReferenceProxy<IWeap>(record.TemplateWeapon);
            }
            set
            {
                EnsureWritable();
                record.TemplateWeapon = value.ToFormId();
            }
        }

        public WeaponTypes Type
        {
            get
            {
                return record.WeaponType.ToTypes();
            }

            set
            {
                EnsureWritable();
                record.WeaponType = value.ToType();
            }
        }

        public IForm UnequipSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.UnequipSound);
            }
            set
            {
                EnsureWritable();
                record.UnequipSound = value.ToFormId();
            }
        }

        public int Value
        {
            get
            {
                return record.Value;
            }

            set
            {
                EnsureWritable();
                record.Value = value;
            }

        }

        public float Weight
        {
            get
            {
                return record.Weight;
            }

            set
            {
                EnsureWritable();
                record.Weight = value;
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
