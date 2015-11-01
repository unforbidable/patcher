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

using Patcher.Rules.Compiled.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies
{
    public abstract class Proxy
    {
        internal ProxyProvider Provider { get; set; }
        internal ProxyMode Mode { get; set; }

        protected void EnsureWritable()
        {
            // Only Target form is writable
            if (Mode != ProxyMode.Target)
            {
                // Provide a specific reason depending on the proxy mode
                if (Mode == ProxyMode.Source)
                    throw new RuntimeException("The Source form cannot be modified.", RuntimeError.ReadOnlyProxy);

                if (Mode == ProxyMode.Discovered)
                    throw new RuntimeException("Forms retrieved via Forms.FindForm() and similar cannot be modified.", RuntimeError.ReadOnlyProxy);

                if (Mode == ProxyMode.Referenced)
                    throw new RuntimeException("Forms retrieved from a references cannot be modified.", RuntimeError.ReadOnlyProxy);
            }
        }
    }
}
