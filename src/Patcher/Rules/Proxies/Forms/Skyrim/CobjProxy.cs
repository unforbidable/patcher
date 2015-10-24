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
    public sealed class CobjProxy : FormProxy<Cobj>, ICobj
    {
        public IMaterialCollection Materials
        {
            get
            {
                var proxy = Provider.CreateProxy<MaterialCollectionProxy>(Mode);
                proxy.Target = record;
                return proxy;
            }
            set
            {
                EnsureWritable();

                if (value == null)
                    throw new ArgumentNullException("value", "Cannot set material collection to false.");

                var otherCollection = (MaterialCollectionProxy)value;

                if (otherCollection == null)
                {
                    // Nothing to copy
                }
                else if (record.Materials == otherCollection.Target.Materials)
                {
                    // Same collection?
                    Log.Warning("Cannot copy a collection into itself.");
                }
                else
                {
                    // Copy materials
                    record.Materials = otherCollection.CopyFieldCollection();
                }
            }
        }

        public IConditionCollection Conditions
        {
            get
            {
                return record.CreateConditionCollectionProxy(this);
            }

            set
            {
                EnsureWritable();
                record.UpdateFromConditionCollectionProxy(value);
            }
        }

        public IForm Result
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.Result);
            }
            set
            {
                EnsureWritable();
                record.Result = value.ToFormId();
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
                return Provider.CreateReferenceProxy<IKywd>(record.Workbench);
            }
            set
            {
                EnsureWritable();
                record.Workbench = value.ToFormId();
            }
        }
    }
}
