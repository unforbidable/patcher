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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins
{
    class PluginReferenceMapper : IReferenceMapper
    {
        readonly byte[] localToContext;
        readonly byte[] contextToLocal;

        public PluginReferenceMapper(Plugin plugin)
        {
            var masters = plugin.MasterFiles.ToList();
            var all = plugin.Context.Plugins.Select(p => p.FileName).ToList();

            localToContext = new byte[masters.Count + 1];
            for (int i = 0; i < masters.Count; i++)
            {
                localToContext[i] = (byte)all.IndexOf(masters[i]);
            }
            localToContext[masters.Count] = plugin.Context.Plugins.GetPluginNumber(plugin);

            contextToLocal = new byte[all.Count];
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i] == plugin.FileName)
                {
                    contextToLocal[i] = (byte)masters.Count;
                }
                else
                {
                    int index = masters.IndexOf(all[i]);
                    if (index == -1)
                        contextToLocal[i] = byte.MaxValue;
                    else
                        contextToLocal[i] = (byte)index;
                }
            }
        }

        public uint LocalToContext(uint formId)
        {
            byte p = (byte)(formId >> 24);
            if (p >= localToContext.Length)
            {
                p = (byte)(localToContext.Length - 1);
            }
            return (formId & 0xFFFFFF) | ((uint)localToContext[p] << 24);
        }

        public uint ContexToLocal(uint formId)
        {
            byte p = (byte)(formId >> 24);
            if (p >= contextToLocal.Length || contextToLocal[p] == byte.MaxValue)
            {
                throw new InvalidOperationException("Cannot map reference to mod that doesn't have original plugin as master.");
            }
            return (formId & 0xFFFFFF) | ((uint)contextToLocal[p] << 24);
        }
    }
}