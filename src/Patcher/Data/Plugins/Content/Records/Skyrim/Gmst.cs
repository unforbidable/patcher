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

using Patcher.Data.Plugins.Content.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.GMST)]
    public sealed class Gmst : GenericFormRecord
    {
        [Member(Names.DATA)]
        [Initialize]
        [Lazy]
        private ByteArray Data { get; set; }

        char Type { get { return !string.IsNullOrEmpty(EditorId) ? EditorId[0] : '\0'; } }

        public dynamic Value
        {
            get
            {
                switch (Type)
                {
                    case 's':
                        // Ignore data if localized string has been fetched
                        return Encoding.UTF8.GetString(Data.Bytes, 0, Data.Bytes.Length - 1);

                    case 'i':
                        return BitConverter.ToInt32(Data.Bytes, 0);

                    case 'f':
                        return BitConverter.ToSingle(Data.Bytes, 0);

                    case 'b':
                        return BitConverter.ToBoolean(Data.Bytes, 0);

                    default:
                        throw new InvalidOperationException("Game Setting EditorId must be set before its value can be accessed");
                }
            }

            set
            {
                switch (Type)
                {
                    case 's':
                        Data.Bytes = GetStringZeroTerminatedBytes((string)value);
                        break;

                    case 'i':
                        Data.Bytes = BitConverter.GetBytes((int)value);
                        break;

                    case 'f':
                        Data.Bytes = BitConverter.GetBytes((float)value);
                        break;

                    case 'b':
                        // Store bool as int32
                        Data.Bytes = BitConverter.GetBytes((value ? 1 : 0));
                        break;

                    default:
                        throw new InvalidOperationException("Game Setting EditorId must be set before its value can be set");
                }
            }
        }

        private static byte[] EmptyStringBytes = { 0 };

        private byte[] GetStringZeroTerminatedBytes(string value)
        {
            // Because Bytes filed in Binary class is ummutable (as Gmst.Data at least)
            // (ummutable as in the array is replaced as a whole and its bytes are never changed)
            // we can use the same byte array for all empty strings
            if (value.Length == 0)
                return EmptyStringBytes;

            // Allocate +1 byte to mark string termination
            byte[] bytes = new byte[Encoding.UTF8.GetByteCount(value) + 1];
            Encoding.UTF8.GetBytes(value, 0, value.Length, bytes, 0);
            return bytes;
        }

        protected override void AfterRead(RecordReader reader)
        {
            // Pull localized string if needed and replace data
            if (Type == 's' && reader.PluginFlags.HasFlag(PluginFlags.Localized))
            {
                uint index = BitConverter.ToUInt32(Data.Bytes, 0);
                string localized = reader.GetLocalizedString(LocalizedStringGroups.Strings, index);
                Data.Bytes = GetStringZeroTerminatedBytes(localized);
            }
        }

        protected override void OnEditorIdChanged(string oldEditorId)
       {
            if (!string.IsNullOrEmpty(EditorId))
            {
                char oldType = !string.IsNullOrEmpty(oldEditorId) ? EditorId[0] : '\0';
                if (oldType != Type)
                {
                    // When type changed or set for the first time
                    // Initialize value
                    switch (Type)
                    {
                        case 's':
                            Value = string.Empty;
                            break;

                        case 'i':
                        case 'f':
                            Value = 0;
                            break;

                        case 'b':
                            Value = false;
                            break;

                        default:
                            // Unknown type enconutered
                            Log.Warning("Game Setting EditorID should start with letter s, i or f: " + EditorId);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (Type != '\0')
                return string.Format("{0}:{1}", Type, Value);
            else
                return "(uninitialized)";
        }
    }
}
