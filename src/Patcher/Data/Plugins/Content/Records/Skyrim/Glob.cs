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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.GLOB)]
    public sealed class Glob : GenericFormRecord
    {
        [Member(Names.FNAM)]
        public char Type { get { return type; } set { char old = type; type = value; OnTypeChanged(old); } }
        char type;

        [Member(Names.FLTV)]
        private float InternalValue { get; set; }

        public dynamic Value
        {
            get
            {
                switch (Type)
                {
                    case 's':
                        return Convert.ToInt16(InternalValue);

                    case 'l':
                        return Convert.ToInt32(InternalValue);

                    case 'f':
                        return InternalValue;

                    default:
                        throw new InvalidOperationException("Global type must be set before its value can be accessed");
                }
            }

            set
            {
                switch (Type)
                {
                    case 's':
                        InternalValue = Convert.ToSingle(Convert.ToInt16(value));
                        break;

                    case 'l':
                        InternalValue = Convert.ToSingle(Convert.ToInt32(value));
                        break;

                    case 'f':
                        InternalValue = (float)value;
                        break;

                    default:
                        throw new InvalidOperationException("Global type must be set before its value can be set");
                }
            }
        }

        private void OnTypeChanged(char oldType)
        {
            if (oldType != Type)
            {
                // When type changed or set for the first time
                // Initialize value
                switch (Type)
                {
                    case 's':
                    case 'l':
                    case 'f':
                        Value = 0;
                        break;

                    default:
                        // Unknown type enconutered
                        Log.Warning("Global type must be either 's', 'l' or 'f'");
                        type = oldType;
                        break;
                }
            }
        }

        public override string ToString()
        {
            if (Type != '\0')
                return string.Format("{0}:{1}", Value);
            else
                return "(uninitialized)";
        }
    }
}
