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
        static IDictionary<Enum, Enum> typeToGlobalVariableTypeMap = new SortedDictionary<Enum, Enum>()
        {
            { Types.Int, GlobalVariableType.Integer },
            { Types.Short, GlobalVariableType.Short },
            { Types.Float, GlobalVariableType.Float },
        };

        static IDictionary<Enum, Enum> globalVariableTypeToTypeMap = new SortedDictionary<Enum, Enum>()
        {
            { GlobalVariableType.Integer, Types.Int },
            { GlobalVariableType.Short, Types.Short },
            { GlobalVariableType.Float, Types.Float },
        };

        public static GlobalVariableType ToGlobalVariableType(this Types value)
        {
            return (GlobalVariableType)ConvertUsing(typeToGlobalVariableTypeMap, value);
        }

        public static Types ToType(this GlobalVariableType value)
        {
            return (Types)ConvertUsing(globalVariableTypeToTypeMap, value);
        }

        static IDictionary<Enum, Enum> skillToUsageMap = new SortedDictionary<Enum, Enum>()
        {
            { Skills.LightArmor, ArmorSkillUsage.LightArmor },
            { Skills.HeavyArmor, ArmorSkillUsage.HeavyArmor },
            { Skills.None, ArmorSkillUsage.None }
        };

        static IDictionary<Enum, Enum> usageToSkillMap = new SortedDictionary<Enum, Enum>()
        {
            { ArmorSkillUsage.LightArmor, Skills.LightArmor },
            { ArmorSkillUsage.HeavyArmor, Skills.HeavyArmor },
            { ArmorSkillUsage.None, Skills.None }
        };

        public static Skills ToSkill(this ArmorSkillUsage value)
        {
            return (Skills)ConvertUsing(usageToSkillMap, value);
        }

        public static ArmorSkillUsage ToArmorSkillUsage(this Skills value)
        {
            return (ArmorSkillUsage)ConvertUsing(skillToUsageMap, value);
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

        static IDictionary<Enum, Enum> typeToScriptPropertyTypeMap = new SortedDictionary<Enum, Enum>()
        {
            { Types.Object, ScriptPropertyType.Object },
            { Types.String, ScriptPropertyType.String },
            { Types.Int, ScriptPropertyType.Int },
            { Types.Float, ScriptPropertyType.Float },
            { Types.Bool, ScriptPropertyType.Bool },
            { Types.ArrayOfObject, ScriptPropertyType.ArrayOfObject },
            { Types.ArrayOfString, ScriptPropertyType.ArrayOfString },
            { Types.ArrayOfInt, ScriptPropertyType.ArrayOfInt },
            { Types.ArrayOfFloat, ScriptPropertyType.ArrayOfFloat },
            { Types.ArrayOfBool, ScriptPropertyType.ArrayOfBool },
        };

        public static ScriptPropertyType ToScriptPropertType(this Types value)
        {
            return (ScriptPropertyType)ConvertUsing(typeToScriptPropertyTypeMap, value);
        }

        static IDictionary<Enum, Enum> potionTypeToFlags = new SortedDictionary<Enum, Enum>()
        {
            { PotionTypes.Auto, PotionFlags.None },
            { PotionTypes.Food, PotionFlags.Food },
            { PotionTypes.Medicine, PotionFlags.Medicine },
            { PotionTypes.Poison, PotionFlags.Poison }
        };

        static IDictionary<Enum, Enum> potionFlagsToType = new SortedDictionary<Enum, Enum>()
        {
            { PotionFlags.None, PotionTypes.Auto },
            { PotionFlags.Food, PotionTypes.Food },
            { PotionFlags.Medicine, PotionTypes.Medicine },
            { PotionFlags.Poison, PotionTypes.Poison }
        };

        public static PotionFlags ToPotionFlags(this PotionTypes value)
        {
            return (PotionFlags)ConvertUsing(potionTypeToFlags, value);
        }

        public static PotionTypes ToPotionType(this PotionFlags value)
        {
            return (PotionTypes)ConvertUsing(potionFlagsToType, value);
        }

        static IDictionary<Enum, Enum> projectileTypesToType = new SortedDictionary<Enum, Enum>()
        {
            { ProjectileTypes.Arrow, ProjectileType.Arrow },
            { ProjectileTypes.Barrier, ProjectileType.Barrier },
            { ProjectileTypes.Beam, ProjectileType.Beam },
            { ProjectileTypes.Cone, ProjectileType.Cone },
            { ProjectileTypes.Flame, ProjectileType.Flame },
            { ProjectileTypes.Lobber, ProjectileType.Lobber },
            { ProjectileTypes.Missile, ProjectileType.Missile }
        };

        static IDictionary<Enum, Enum> projectileTypeToTypes = new SortedDictionary<Enum, Enum>()
        {
            { ProjectileType.Arrow, ProjectileTypes.Arrow },
            { ProjectileType.Barrier, ProjectileTypes.Barrier },
            { ProjectileType.Beam, ProjectileTypes.Beam },
            { ProjectileType.Cone, ProjectileTypes.Cone },
            { ProjectileType.Flame, ProjectileTypes.Flame },
            { ProjectileType.Lobber, ProjectileTypes.Lobber },
            { ProjectileType.Missile, ProjectileTypes.Missile }
        };

        public static ProjectileType ToProjectileType(this ProjectileTypes value)
        {
            return (ProjectileType)ConvertUsing(projectileTypesToType, value);
        }

        public static ProjectileTypes ToProjectileTypes(this ProjectileType value)
        {
            return (ProjectileTypes)ConvertUsing(projectileTypeToTypes, value);
        }

        static IDictionary<Enum, Enum> soundLevelsToLevel = new SortedDictionary<Enum, Enum>()
        {
            { SoundLevels.Loud, SoundLevel.Loud },
            { SoundLevels.Normal, SoundLevel.Normal },
            { SoundLevels.Silent, SoundLevel.Silent },
            { SoundLevels.VeryLoud, SoundLevel.VeryLoud }
        };

        static IDictionary<Enum, Enum> soundLevelToLevels = new SortedDictionary<Enum, Enum>()
        {
            { SoundLevel.Loud, SoundLevels.Loud },
            { SoundLevel.Normal, SoundLevels.Normal },
            { SoundLevel.Silent, SoundLevels.Silent },
            { SoundLevel.VeryLoud, SoundLevels.VeryLoud }
        };

        public static SoundLevel ToSoundLevel(this SoundLevels value)
        {
            return (SoundLevel)ConvertUsing(soundLevelsToLevel, value);
        }

        public static SoundLevels ToSoundLevels(this SoundLevel value)
        {
            return (SoundLevels)ConvertUsing(soundLevelToLevels, value);
        }

        private static Enum ConvertUsing(IDictionary<Enum, Enum> map, Enum value)
        {
            if (!map.ContainsKey(value))
                throw new ArgumentException(value.GetType().Name + "." + value + " is not a valid value in this context.");

            return map[value];
        }
    }
}
