/// Copyright(C) 2018 Unforbidable Works
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

using Patcher.Data.Models.Loading;
using Patcher.Data.Models.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Model object that represents a member of an enum.
    /// </summary>
    public class EnumMemberModel : IPresentable, INamed
    {
        /// <summary>
        /// The name of the member.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The name of the member, as displayed by the GUI.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The description of the member.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The value of the member.
        /// </summary>
        public string Value { get; private set; }

        public EnumMemberModel(string name, string displayName, string description, string value)
        {
            Name = name;
            DisplayName = displayName ?? name;
            Description = description;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }
    }
}
