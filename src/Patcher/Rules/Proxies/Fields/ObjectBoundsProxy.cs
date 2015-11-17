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

using Patcher.Data.Plugins.Content;
using Patcher.Rules.Compiled.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields
{
    [Proxy(typeof(IObjectBounds))]
    public sealed class ObjectBoundsProxy : Proxy, IObjectBounds
    {
        internal IFeaturingObjectBounds Record { get; set; }

        public short X1
        {
            get
            {
                return Record.ObjectBounds.X1;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.X1 = value;
            }
        }

        public short Y1
        {
            get
            {
                return Record.ObjectBounds.Y1;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.Y1 = value;
            }
        }

        public short Z1
        {
            get
            {
                return Record.ObjectBounds.Z1;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.Z1 = value;
            }
        }

        public short X2
        {
            get
            {
                return Record.ObjectBounds.X2;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.X2 = value;
            }
        }

        public short Y2
        {
            get
            {
                return Record.ObjectBounds.Y2;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.Y2 = value;
            }
        }

        public short Z2
        {
            get
            {
                return Record.ObjectBounds.Z2;
            }
            set
            {
                EnsureWritable();
                Record.ObjectBounds.Z2 = value;
            }
        }
    }
}
