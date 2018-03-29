// Copyright(C) 2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Code.Building
{
    public static class CodeBuilderHelper
    {
        public static string ModifiersToString(CodeModifiers modifiers)
        {
            return string.Join("", Enum.GetValues(modifiers.GetType()).Cast<Enum>().Where(v => modifiers.HasFlag(v) && Convert.ToInt64(v) != 0).Select(v => v.ToString().ToLower() + " "));
        }

        public static void ValidateTypeName(string name)
        {
            // TODO: Ensure type name is valid
        }

        public static void ValidateMemberName(string name)
        {
            // TODO: Ensure member name is valid
        }

        public static void ValidateNamespaceName(string name)
        {
            // TODO: Ensure namespace name is valid
        }

        public static void ValidateNameUniqueInCollection(string name, ICollection<CodeItem> collection)
        {
            // TODO: Ensure name is unique in the collection
        }

        public static void ValidateNameNotEqualToOwner(string name, CodeItem owner)
        {
            // TODO: Ensure name is not equal to that of the owner
        }
    }
}
