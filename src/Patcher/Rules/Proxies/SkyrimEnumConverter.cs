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
    static class SkyrimEnumConverter
    {
        public static Skills ToSkill(this ArmorSkillUsage value)
        {
            return EnumConverter.ConvertByName<Skills>(value);
        }

        public static ArmorSkillUsage ToArmorSkillUsage(this Skills value)
        {
            return EnumConverter.ConvertByName<ArmorSkillUsage>(value);
        }

        public static ActorValue ToActorValue(this Skills value)
        {
            return EnumConverter.ConvertByName<ActorValue>(value);
        }

        public static Skills ToSkill(this ActorValue value)
        {
            return EnumConverter.ConvertByName<Skills>(value);
        }

        public static ActorValue ToActorValue(this Resistances value)
        {
            return EnumConverter.ConvertByName<ActorValue>(value);
        }

        public static Resistances ToResistance(this ActorValue value)
        {
            return EnumConverter.ConvertByName<Resistances>(value);
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
            return EnumConverter.ConvertByName<WeaponType>(value);
        }

        public static WeaponTypes ToTypes(this WeaponType value)
        {
            return EnumConverter.ConvertByName<WeaponTypes>(value);
        }

        public static ScriptPropertyType ToScriptPropertType(this Types value)
        {
            return EnumConverter.ConvertByName<ScriptPropertyType>(value);
        }

        public static PotionFlags ToPotionFlags(this PotionTypes value)
        {
            return EnumConverter.ConvertByName<PotionFlags>(value);
        }

        public static PotionTypes ToPotionType(this PotionFlags value)
        {
            return EnumConverter.ConvertByName<PotionTypes>(value);
        }

        public static ProjectileType ToProjectileType(this ProjectileTypes value)
        {
            return EnumConverter.ConvertByName<ProjectileType>(value);
        }

        public static ProjectileTypes ToProjectileTypes(this ProjectileType value)
        {
            return EnumConverter.ConvertByName<ProjectileTypes>(value);
        }

        public static SoundLevel ToSoundLevel(this SoundLevels value)
        {
            return EnumConverter.ConvertByName<SoundLevel>(value);
        }

        public static SoundLevels ToSoundLevels(this SoundLevel value)
        {
            return EnumConverter.ConvertByName<SoundLevels>(value);
        }

        public static AttackAnimation ToAttackAnimation(this Animations value)
        {
            return EnumConverter.ConvertByName<AttackAnimation>(value);
        }

        public static Animations ToAnimations(this AttackAnimation value)
        {
            return EnumConverter.ConvertByName<Animations>(value);
        }

        public static WeatherSoundType ToWeatherSoundType(this WeatherSoundTypes value)
        {
            return EnumConverter.ConvertByName<WeatherSoundType>(value);
        }

        public static WeatherSoundTypes ToWeatherSoundTypes(this WeatherSoundType value)
        {
            return EnumConverter.ConvertByName<WeatherSoundTypes>(value);
        }
    }
}
