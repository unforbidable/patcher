﻿/// Copyright(C) 2015 Unforbidable Works
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

using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Objects.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Skyrim
{
    [Proxy(typeof(IGlob))]
    public sealed class GlobProxy : FormProxy<Glob>, IGlob
    {
        public bool IsConstant { get { EnsureReadable(); return record.IsConstant; } set { EnsureWritable(); record.IsConstant = value; } }
        public char Type { get { EnsureReadable(); return record.Type; } set { EnsureWritable(); record.Type = value; } }
        public dynamic Value { get { EnsureReadable(); return record.Value; } set { EnsureWritable(); WarnIfConstant(); record.Value = value; } }

        private void WarnIfConstant()
        {
            if (record.IsConstant)
                Log.Warning("Value of a constant Global Variable has been changed: {0}", this);
        }
    }
}
