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

namespace Patcher.Data.Plugins.Content
{
    sealed class RecordEntry
    {
        public long FilePosition { get; internal set; }
        public string Signature { get; internal set; }
        public uint FormId { get; internal set; }
        public uint ParentRecordFormId { get; set; }

        public void CopyTo(RecordEntry other)
        {
            other.FilePosition = FilePosition;
            other.Signature = Signature;
            other.FormId = FormId;
            other.ParentRecordFormId = ParentRecordFormId;
        }
    }
}
