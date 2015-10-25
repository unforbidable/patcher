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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    class HelperAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type InterfaceType { get; private set; }
        public bool DebugModeOnly { get; private set; }

        public HelperAttribute(string name, Type interfaceType)
            : this(name, interfaceType, false)
        {
        }

        public HelperAttribute(string name, Type interfaceType, bool debugModeOnly)
        {
            Name = name;
            InterfaceType = interfaceType;
            DebugModeOnly = debugModeOnly;
        }
    }
}
