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
    [Flags]
    enum RecordFlags : uint
    {
        None = 0x0,
        Unnamed0 = 0x1,
        Unnamed1 = 0x2,
        Unnamed2 = 0x4,
        Unnamed3 = 0x8,
        Unnamed4 = 0x10,
        Unnamed5 = 0x20,
        Unnamed6 = 0x40,
        Unnamed7 = 0x80,
        Unnamed8 = 0x100,
        Unnamed9 = 0x200,
        Unnamed10 = 0x400,
        Unnamed11 = 0x800,
        Unnamed12 = 0x1000,
        Unnamed13 = 0x2000,
        Unnamed14 = 0x4000,
        Unnamed15 = 0x8000,
        Unnamed16 = 0x10000,
        Unnamed17 = 0x20000,
        Compressed = 0x00040000,
        Unnamed19 = 0x80000,
        Unnamed20 = 0x100000,
        Unnamed21 = 0x200000,
        Unnamed22 = 0x400000,
        Unnamed23 = 0x800000,
        Unnamed24 = 0x1000000,
        Unnamed25 = 0x2000000,
        Unnamed26 = 0x4000000,
        Unnamed27 = 0x8000000,
        Unnamed28 = 0x10000000,
        Unnamed29 = 0x20000000,
        Unnamed30 = 0x40000000,
        Unnamed31 = 0x80000000
    }
}
