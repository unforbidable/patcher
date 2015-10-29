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
using Patcher.Rules.Compiled.Constants;
using Patcher.Rules.Compiled.Constants.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    static class EnumConverter
    {
        public static GlobalVariableType ToGlobalVariableType(this Types value)
        {
            return ConvertByName<GlobalVariableType>(value);
        }

        public static Types ToType(this GlobalVariableType value)
        {
            return ConvertByName<Types>(value);
        }

        public static Skills ToSkill(this ArmorSkillUsage value)
        {
            return ConvertByName<Skills>(value);
        }

        public static ArmorSkillUsage ToArmorSkillUsage(this Skills value)
        {
            return ConvertByName<ArmorSkillUsage>(value);
        }

        public static ActorValue ToActorValue(this Skills value)
        {
            return ConvertByName<ActorValue>(value);
        }

        public static Skills ToSkill(this ActorValue value)
        {
            return ConvertByName<Skills>(value);
        }

        public static ActorValue ToActorValue(this Resistances value)
        {
            return ConvertByName<ActorValue>(value);
        }

        public static Resistances ToResistance(this ActorValue value)
        {
            return ConvertByName<Resistances>(value);
        }

        public static BodyNodes ToBodyNodes(this BodyParts value)
        {
            // Body node and body part are 1:1
            return (BodyNodes)value;
        }

        public static BodyParts ToBodyParts(this BodyNodes value)
        {
            // Body node and body part are 1:1
            return (BodyParts)value;
        }

        public static WeaponType ToType(this WeaponTypes value)
        {
            return ConvertByName<WeaponType>(value);
        }

        public static WeaponTypes ToTypes(this WeaponType value)
        {
            return ConvertByName<WeaponTypes>(value);
        }

        public static ScriptPropertyType ToScriptPropertType(this Types value)
        {
            return ConvertByName<ScriptPropertyType>(value);
        }

        public static PotionFlags ToPotionFlags(this PotionTypes value)
        {
            return ConvertByName<PotionFlags>(value);
        }

        public static PotionTypes ToPotionType(this PotionFlags value)
        {
            return ConvertByName<PotionTypes>(value);
        }

        public static ProjectileType ToProjectileType(this ProjectileTypes value)
        {
            return ConvertByName<ProjectileType>(value);
        }

        public static ProjectileTypes ToProjectileTypes(this ProjectileType value)
        {
            return ConvertByName<ProjectileTypes>(value);
        }

        public static SoundLevel ToSoundLevel(this SoundLevels value)
        {
            return ConvertByName<SoundLevel>(value);
        }

        public static SoundLevels ToSoundLevels(this SoundLevel value)
        {
            return ConvertByName<SoundLevels>(value);
        }

        public static AttackAnimation ToAttackAnimation(this Animations value)
        {
            return ConvertByName<AttackAnimation>(value);
        }

        public static Animations ToAnimations(this AttackAnimation value)
        {
            return ConvertByName<Animations>(value);
        }

        private static T ConvertByName<T>(Enum value)
        {
            // TODO: Possibly cache conversion results or at least names and values
            Type target = typeof(T);
            string fromName = value.ToString();
            var names = Enum.GetNames(target);
            var values = Enum.GetValues(target);
            for (int i = 0; i < names.Length; i++)
            {
                if (fromName == names[i])
                    return (T)values.GetValue(i);
            }
            throw new ArgumentException(value.GetType().Name + "." + value + " is not a valid value in this context.");
        }

        private static Enum ConvertUsing(IDictionary<Enum, Enum> map, Enum value)
        {
            if (!map.ContainsKey(value))
                throw new ArgumentException(value.GetType().Name + "." + value + " is not a valid value in this context.");

            return map[value];
        }
    }
}
