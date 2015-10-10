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

using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Objects.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Rules.Proxies.Skyrim
{
    [Proxy(typeof(IAlch))]
    public sealed class AlchProxy : FormProxy<Alch>, IAlch
    {
        public string Name { get { EnsureReadable(); return record.Name; } set { EnsureWritable(); record.Name = value; } }
        public int Value { get { EnsureReadable(); return record.Consumption.Value; } set { EnsureWritable(); record.Consumption.Value = value; } }
        public float Weight { get { EnsureReadable(); return record.Misc.Weight; } set { EnsureWritable(); record.Misc.Weight = value; } }


        // TODO: Not finished
    }
}
