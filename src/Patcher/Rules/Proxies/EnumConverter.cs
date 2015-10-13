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
using Patcher.Rules.Compiled.Constants.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    static class EnumConverter
    {
        public static Skill ToSkill(this ArmorSkillUsage value)
        {
            return (Skill)ConvertUsing(usageToSkillMap, value);
        }

        public static ArmorSkillUsage ToArmorSkillUsage(this Skill value)
        {
            return (ArmorSkillUsage)ConvertUsing(skillToUsageMap, value);
        }

        static IDictionary<Enum, Enum> skillToUsageMap = new SortedDictionary<Enum, Enum>()
        {
            { Skill.LightArmor, ArmorSkillUsage.LightArmor },
            { Skill.HeavyArmor, ArmorSkillUsage.HeavyArmor },
            { Skill.None, ArmorSkillUsage.None }
        };

        static IDictionary<Enum, Enum> usageToSkillMap = new SortedDictionary<Enum, Enum>()
        {
            { ArmorSkillUsage.LightArmor, Skill.LightArmor },
            { ArmorSkillUsage.HeavyArmor, Skill.HeavyArmor },
            { ArmorSkillUsage.None, Skill.None }
        };

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
            { Compiled.Constants.Type.Object, ScriptPropertyType.Object },
            { Compiled.Constants.Type.String, ScriptPropertyType.String },
            { Compiled.Constants.Type.Int, ScriptPropertyType.Int },
            { Compiled.Constants.Type.Float, ScriptPropertyType.Float },
            { Compiled.Constants.Type.Bool, ScriptPropertyType.Bool },
            { Compiled.Constants.Type.ArrayOfObject, ScriptPropertyType.ArrayOfObject },
            { Compiled.Constants.Type.ArrayOfString, ScriptPropertyType.ArrayOfString },
            { Compiled.Constants.Type.ArrayOfInt, ScriptPropertyType.ArrayOfInt },
            { Compiled.Constants.Type.ArrayOfFloat, ScriptPropertyType.ArrayOfFloat },
            { Compiled.Constants.Type.ArrayOfBool, ScriptPropertyType.ArrayOfBool },
        };

        public static ScriptPropertyType ToScriptPropertType(this Compiled.Constants.Type value)
        {
            return (ScriptPropertyType)ConvertUsing(typeToScriptPropertyTypeMap, value);
        }

        private static Enum ConvertUsing(IDictionary<Enum, Enum> map, Enum value)
        {
            if (!map.ContainsKey(value))
                throw new ArgumentException(value.GetType().Name + "." + value + " is not a valid value in this context.");

            return map[value];
        }
    }
}
