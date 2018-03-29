// Copyright(C) 2015,2016,2017,2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    [Flags]
    public enum CodeModifiers
    {
        None = 0,
        Public = 1,
        Protected = 2,
        Internal = 4,
        Private = 8,
        Static = 16,
        Const = 32,
        Sealed = 64,
        Abstract = 128,
        Virtual = 256,
        Override = 512,
        Implicit = 1024,
        Explicit = 2048,
        Operator = 4096
    }
}
