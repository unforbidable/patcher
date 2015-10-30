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
using System.Windows.Input;
using System.Windows.Media;

namespace Patcher.UI
{
    public class Choice
    {
        public string Text { get; private set; }
        public string Description { get; private set; }
        public Key Key { get; private set; }
        public Color Color { get; private set; }

        public static readonly Color Green = Color.FromRgb(0x76, 0xAE, 0x61);
        public static readonly Color Red = Color.FromRgb(0xBB, 0x64, 0x80);
        public static readonly Color Blue = Color.FromRgb(0x62, 0x8B, 0xC7);
        public static readonly Color Gray = Color.FromRgb(0x9C, 0x9C, 0x9C);

        public Choice(ChoiceOption option)
        {
            switch (option)
            {
                case ChoiceOption.Yes:
                    Text = "Yes";
                    Description = "Click or press Y to agree.";
                    Key = Key.Y;
                    Color = Green;
                    break;

                case ChoiceOption.No:
                    Text = "No";
                    Description = "Click or press N to disagree.";
                    Key = Key.N;
                    Color = Red;
                    break;

                case ChoiceOption.Cancel:
                    Text = "Cancel";
                    Description = "Click or press Escape to cancel.";
                    Key = Key.Escape;
                    Color = Red;
                    break;

                case ChoiceOption.Ok:
                    Text = "OK";
                    Description = "Click or press Enter to confirm.";
                    Key = Key.Enter;
                    Color = Green;
                    break;

                case ChoiceOption.Continue:
                    Text = "Continue";
                    Description = "Click or press Space to continue.";
                    Key = Key.Space;
                    Color = Blue;
                    break;

                default:
                    throw new ArgumentException("Invalid choice option: " + option);
            }
        }

        public ChoiceOption GetOption()
        {
            ChoiceOption result;
            if (Enum.TryParse(Text, true, out result))
            {
                return result;
            }
            else
            {
                return ChoiceOption.Cancel;
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
