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

namespace Patcher.Data.Plugins.Content.Fields.Skyrim
{
    public class DestructionData : Compound
    {
        [Member(Names.DEST)]
        private ByteArray Header { get; set; }

        [Member(Names.DSTD, Names.DSTF)]
        private List<DestructionStage> Stages { get; set; }

        protected override void AfterRead(RecordReader reader)
        {
            base.AfterRead(reader);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        class DestructionStage : Compound
        {
            [Member(Names.DSTD)]
            public ByteArray Stage { get; set; }

            [Member(Names.DSTF)]
            public ByteArray EndMarker { get; set; }

            public override string ToString()
            {
                throw new NotImplementedException();
            }
        }
    }
}
