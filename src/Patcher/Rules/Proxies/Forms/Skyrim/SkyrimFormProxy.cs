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
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Data.Plugins.Content.Records;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Proxies.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    /// <summary>
    /// Provides common functionality to Skyrim from proxy implementations.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class SkyrimFormProxy<TRecord> : FormProxy<TRecord> where TRecord : GenericFormRecord
    {
        protected IScriptCollection GetVirtualMachineAdapterProxy(IHasScripts record)
        {
            EnsureReadable();
            ScriptCollectionProxy proxy = Provider.CreateProxy<ScriptCollectionProxy>(Mode);
            proxy.Record = record;
            return proxy;
        }

        protected void SetVirtualMachineAdapterProxy(IHasScripts record, IScriptCollection value)
        {
            EnsureWritable();
            if (value == null || ((ScriptCollectionProxy)value).Record.VirtualMachineAdapter == null)
            {
                // Assigned null or a script collection proxy of record that has no scripts
                record.VirtualMachineAdapter = null;
            }
            else
            {
                // Assign scripts of another record are assigned, make a copy
                record.VirtualMachineAdapter = (VirtualMachineAdapter)((ScriptCollectionProxy)value).Record.VirtualMachineAdapter.CopyField();
            }
        }

        protected IObjectBounds GetObjectBoundsProxy(IHasObjectBounds record)
        {
            // Ensure ObjectBounds exist
            // If they are requested, they will be used
            if (record.ObjectBounds == null)
                record.ObjectBounds = new ObjectBounds();

            ObjectBoundsProxy proxy = Provider.CreateProxy<ObjectBoundsProxy>(Mode);
            proxy.Record = record;
            return proxy;
        }

        protected void SetObjectBoundsProxy(IHasObjectBounds record, IObjectBounds value)
        {
            EnsureWritable();

            if (value == null && ((ObjectBoundsProxy)value).Record.ObjectBounds == null)
            {
                // null as argument, or proxy of record without bounds
                record.ObjectBounds = null;
            }
            else
            {
                // Assign copy of bounds of another record
                record.ObjectBounds = (ObjectBounds)((ObjectBoundsProxy)value).Record.ObjectBounds.CopyField();
            }
        }
    }
}
