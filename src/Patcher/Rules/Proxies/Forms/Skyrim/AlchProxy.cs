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
using System.Threading.Tasks;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Data.Plugins.Content.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IAlch))]
    public sealed class AlchProxy : FormProxy<Alch>, IAlch
    {
        public string Description
        {
            get
            {
                return record.Description;
            }

            set
            {
                EnsureWritable();
                record.Description = value;
            }
        }

        public IFormCollection<IKywd> Keywords
        {
            get
            {
                return Provider.CreateFormCollectionProxy<IKywd>(Mode, record.Keywords.Items);
            }
            set
            {
                EnsureWritable();
                record.Keywords.Items = value.ToFormIdList();
            }
        }

        public string FullName
        {
            get
            {
                return record.FullName;
            }

            set
            {
                EnsureWritable();
                record.FullName = value;
            }
        }

        public IObjectBounds ObjectBounds
        {
            get
            {
                return record.CreateObjectBoundsProxy(this);
            }

            set
            {
                EnsureWritable();
                record.UpdateFromObjectBoundsProxy(value);
            }
        }

        public ISndr PickUpSound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.PickUpSound);
            }
            set
            {
                EnsureWritable();
                record.PickUpSound = value.ToFormId();
            }
        }

        public ISndr PutDownSound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.PutDownSound);
            }
            set
            {
                EnsureWritable();
                record.PutDownSound = value.ToFormId();
            }
        }

        public ISndr UseSound
        {
            get
            {
                return Provider.CreateReferenceProxy<ISndr>(record.UseSound);
            }
            set
            {
                EnsureWritable();
                record.UseSound = value.ToFormId();
            }
        }

        public int Value
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

        public float Weight
        {
            get
            {
                return record.Weight;
            }

            set
            {
                EnsureWritable();
                record.Weight = value;
            }
        }

        public string WorldModel
        {
            get
            {
                return record.WorldModel;
            }

            set
            {
                EnsureWritable();
                record.WorldModel = value;
            }
        }

        public PotionTypes Type
        {
            get
            {
                return (record.PotionFlags & PotionFlags.FoodMedicinePoisonMask).ToPotionType();
            }

            set
            {
                EnsureWritable();
                record.PotionFlags = (record.PotionFlags & ~PotionFlags.FoodMedicinePoisonMask) | value.ToPotionFlags();
            }
        }

        public IEffectCollection Effects
        {
            get
            {
                return record.CreateEffectCollectionProxy(this);
            }

            set
            {
                EnsureWritable();
                record.UpdateFromConditionCollectionProxy(value);
            }
        }
    }
}
