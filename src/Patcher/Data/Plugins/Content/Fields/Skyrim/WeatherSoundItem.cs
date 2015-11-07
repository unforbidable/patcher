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

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class WeatherSoundItem : Field
    {
        public uint Sound { get; set; }
        public WeatherSoundType Type { get; set; }

        internal override void ReadField(RecordReader reader)
        {
            Sound = reader.ReadReference(FormKindSet.SndrOnly);
            Type = (WeatherSoundType)reader.ReadInt32();
        }

        internal override void WriteField(RecordWriter writer)
        {
            throw new NotImplementedException();
        }

        public override Field CopyField()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Field other)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
