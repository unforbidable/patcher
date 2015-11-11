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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records
{
    [Record(Names.GLOB)]
    [Game(Games.Skyrim)]
    [Game(Games.Fallout4)]
    public sealed class Glob : GenericFormRecord
    {
        public bool IsConstant { get { return HasFlag(GlobalVariableRecordFlags.Constant); } set { SetFlag(GlobalVariableRecordFlags.Constant, value); } }

        [Member(Names.FNAM)]
        public GlobalVariableType Type { get { return type; } set { GlobalVariableType old = type; type = value; OnTypeChanged(old); } }
        GlobalVariableType type;

        [Member(Names.FLTV)]
        private float InternalValue { get; set; }

        public Glob()
        {
            // Default to float for new forms
            Type = GlobalVariableType.Float;
        }

        public dynamic Value
        {
            get
            {
                switch (Type)
                {
                    case GlobalVariableType.Short:
                        return Convert.ToInt16(InternalValue);

                    case GlobalVariableType.Int:
                        return Convert.ToInt32(InternalValue);

                    case GlobalVariableType.Float:
                        return InternalValue;

                    default:
                        throw new InvalidOperationException("Global type must be set before its value can be accessed");
                }
            }

            set
            {
                switch (Type)
                {
                    case GlobalVariableType.Short:
                        InternalValue = Convert.ToSingle(Convert.ToInt16(value));
                        break;

                    case GlobalVariableType.Int:
                        InternalValue = Convert.ToSingle(Convert.ToInt32(value));
                        break;

                    case GlobalVariableType.Float:
                        InternalValue = (float)value;
                        break;

                    default:
                        throw new InvalidOperationException("Global type must be set before its value can be set");
                }
            }
        }

        private void OnTypeChanged(GlobalVariableType oldType)
        {
            if (oldType != Type)
            {
                // When type changed or set for the first time
                // Initialize value
                switch (Type)
                {
                    case GlobalVariableType.Short:
                    case GlobalVariableType.Int:
                    case GlobalVariableType.Float:
                        Value = 0;
                        break;

                    default:
                        // Unknown type enconutered
                        throw new ArgumentException("Illegal Global Variable type: " + Type);
                }
            }
        }

        public override string ToString()
        {
            if (Type != GlobalVariableType.None)
                return string.Format("{0}:{1}", Value);
            else
                return "(uninitialized)";
        }

        enum GlobalVariableRecordFlags : uint
        {
            Constant = 0x40
        }
    }
}
