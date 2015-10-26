using Patcher.Data.Plugins.Content.Records.Skyrim;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IAmmo))]
    public sealed class AmmoProxy : FormProxy<Ammo>, IAmmo
    {
        public float Damage
        {
            get
            {
                return record.Damage;
            }

            set
            {
                EnsureWritable();
                record.Damage = value;
            }
        }

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

        public bool IgnoresNormalWeaponResistance
        {
            get
            {
                return record.IgnoresNormalWeaponResistance;
            }

            set
            {
                EnsureWritable();
                record.IgnoresNormalWeaponResistance = value;
            }
        }

        public string InventoryImage
        {
            get
            {
                return record.InventoryImage;
            }

            set
            {
                EnsureWritable();
                record.InventoryImage = value;
            }
        }

        public bool IsBolt
        {
            get
            {
                return record.IsBolt;
            }

            set
            {
                EnsureWritable();
                record.IsBolt = value;
            }
        }

        public bool IsPlayable
        {
            get
            {
                return record.IsPlayable;
            }

            set
            {
                EnsureWritable();
                record.IsPlayable = value;
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

        public string MessageIcon
        {
            get
            {
                return record.MessageIcon;
            }

            set
            {
                EnsureWritable();
                record.MessageIcon = value;
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

        public IForm PickUpSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.PickUpSound);
            }
            set
            {
                EnsureWritable();
                record.PickUpSound = value.ToFormId();
            }
        }

        public IProj Projectile
        {
            get
            {
                return Provider.CreateReferenceProxy<IProj>(record.Projectile);
            }
            set
            {
                EnsureWritable();
                record.Projectile = value.ToFormId();
            }
        }

        public IForm PutDownSound
        {
            get
            {
                return Provider.CreateReferenceProxy<IForm>(record.PutDownSound);
            }
            set
            {
                EnsureWritable();
                record.PutDownSound = value.ToFormId();
            }
        }

        public string ShortName
        {
            get
            {
                return record.ShortName;
            }

            set
            {
                EnsureWritable();
                record.ShortName = value;
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
    }
}
