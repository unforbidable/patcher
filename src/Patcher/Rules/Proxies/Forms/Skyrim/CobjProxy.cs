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
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Proxies.Fields.Skyrim;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(ICobj))]
    public sealed class CobjProxy : SkyrimFormProxy<Cobj>, ICobj
    {
        MaterialCollectionProxy materials = null;

        public IMaterialCollection Materials
        {
            get
            {
                if (materials == null)
                {
                    materials = Provider.CreateProxy<MaterialCollectionProxy>(Mode);
                    materials.Record = record;
                }
                return materials;
            }
            set
            {
                EnsureWritable();

                if (value == null)
                {
                    record.Materials = null;
                }
                else
                {
                    // Create list or clear
                    if (record.Materials == null)
                        record.Materials = new List<Cobj.MaterialData>();

                    var otherCollection = (MaterialCollectionProxy)value;

                    // Same collection?
                    if (record == otherCollection.Record)
                    {
                        Log.Warning("Cannot assign a material collection into itself - nothing to do.");
                    }
                    else
                    {
                        // Copy materials one by one
                        record.Materials.Clear();
                        foreach (var material in otherCollection.Record.Materials)
                        {
                            record.Materials.Add((Cobj.MaterialData)material.CopyField());
                        }
                    }
                }
            }
        }

        public IForm Result
        {
            get
            {
                if (record.Result == 0)
                    return null;
                else
                    return Provider.CreateFormProxy(record.Result, ProxyMode.Referenced);
            }
            set
            {
                EnsureWritable();
                if (value == null)
                    record.Result = 0;
                else
                    record.Result = value.FormId;
            }
        }

        public int ResultCount
        {
            get
            {
                return record.ResultQuantity;
            }
            set
            {
                EnsureWritable();
                record.ResultQuantity = (ushort)value;
            }
        }

        public IKywd Workbench
        {
            get
            {
                if (record.Workbench == 0)
                    return null;
                else
                    return Provider.CreateFormProxy<IKywd>(record.Workbench, ProxyMode.Referenced);
            }
            set
            {
                EnsureWritable();
                if (value == null)
                    record.Workbench = 0;
                else
                    record.Workbench = value.FormId;
            }
        }

        protected override void OnRecordChanged()
        {
            // When record has been changed (the proxy has been recycled)
            // Update cached material proxy
            if (materials != null)
            {
                materials.Record = record;
            }

            base.OnRecordChanged();
        }
    }
}
