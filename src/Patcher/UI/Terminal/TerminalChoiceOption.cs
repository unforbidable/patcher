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

namespace Patcher.UI.Terminal
{
    sealed class TerminalChoiceOption
    {
        public ConsoleKey Key { get; private set; }
        public string Name { get; private set; }
        public ChoiceOption Option { get; set; }

        static Dictionary<ChoiceOption, TerminalChoiceOption> options = new Dictionary<ChoiceOption, TerminalChoiceOption>()
        {
            { ChoiceOption.Cancel, new TerminalChoiceOption() { Option = ChoiceOption.Cancel, Key = ConsoleKey.Escape, Name = "Esc" } },
            { ChoiceOption.Yes, new TerminalChoiceOption() { Option = ChoiceOption.Yes, Key = ConsoleKey.Y, Name = "Y" } },
            { ChoiceOption.No, new TerminalChoiceOption() { Option = ChoiceOption.No, Key = ConsoleKey.N, Name = "N" } },
            { ChoiceOption.Ok, new TerminalChoiceOption() { Option = ChoiceOption.Ok, Key = ConsoleKey.Enter, Name = "Enter" } }
        };

        public static TerminalChoiceOption GetOption(ChoiceOption option)
        {
            return options[option];
        }
    }
}
