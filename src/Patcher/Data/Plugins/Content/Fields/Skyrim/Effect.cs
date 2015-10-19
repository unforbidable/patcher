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

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public sealed class Effect : Compound, IFeaturingConditions
    {
        [Member(Names.EFID)]
        [Reference(Names.MGEF)]
        public uint BaseEffect { get; set; }

        [Member(Names.EFIT)]
        [Initialize]
        private EffectData Data { get; set; }

        [Member(Names.CTDA, Names.CIS1, Names.CIS2)]
        [Initialize]
        public List<Condition> Conditions { get; set; }

        public float Magnitude { get { return Data.Magnitude; } set { Data.Magnitude = value; } }
        public uint Area { get { return Data.Area; } set { Data.Area = value; } }
        public uint Duration { get { return Data.Duration; } set { Data.Duration = value; } }

        public override string ToString()
        {
            return string.Format("{0}", BaseEffect);
        }

        public sealed class EffectData : Field
        {
            public float Magnitude { get; set; }
            public uint Area { get; set; }
            public uint Duration { get; set; }

            internal override void ReadField(RecordReader reader)
            {
                Magnitude = reader.ReadSingle();
                Area = reader.ReadUInt32();
                Duration = reader.ReadUInt32();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Magnitude);
                writer.Write(Area);
                writer.Write(Duration);
            }

            public override Field CopyField()
            {
                return new EffectData()
                {
                    Magnitude = Magnitude,
                    Area = Area,
                    Duration = Duration
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (EffectData)other;
                return Magnitude == cast.Magnitude && Area == cast.Area && Duration == cast.Duration;
            }

            public override string ToString()
            {
                return string.Format("Magnitude={0} Area={1} Duration={2}", Magnitude, Area, Duration);
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield break;
            }
        }
    }
}
