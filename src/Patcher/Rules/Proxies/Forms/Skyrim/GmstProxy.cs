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
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Constants;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IGmst))]
    public sealed class GmstProxy : FormProxy<Gmst>, IGmst
    {
        public Compiled.Constants.Type Type
        {
            get
            {
                char c = string.IsNullOrEmpty(record.EditorId) ? '\0' : record.EditorId[0];
                switch (c)
                {
                    case 's':
                        return Compiled.Constants.Type.String;

                    case 'b':
                        return Compiled.Constants.Type.Bool;

                    case 'i':
                        return Compiled.Constants.Type.Int;

                    case 'f':
                        return Compiled.Constants.Type.Float;

                    default:
                        return Compiled.Constants.Type.None;
                }
            
            }
        }

        public dynamic Value
        {
            get
            {
                return record.Value;
            }
            set
            {
                EnsureWritable();
                record.Value = value;
            }
        }
    }
}
