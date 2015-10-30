using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Forms;
using Patcher.Data.Plugins.Content.Fields.Skyrim;
using Patcher.Rules.Compiled.Forms.Skyrim;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IEffect))]
    public sealed class EffectProxy : FieldProxy<Effect>, IEffect
    {
        public int Area
        {
            get
            {
                return (int)Field.Area;
            }

            set
            {
                EnsureWritable();
                Field.Area = (uint)value;
            }
        }

        public IMgef BaseEffect
        {
            get
            {
                return Provider.CreateReferenceProxy<IMgef>(Field.BaseEffect);
            }

            set
            {
                EnsureWritable();
                Field.BaseEffect = value.ToFormId();
            }
        }

        public IConditionCollection Conditions
        {
            get
            {
                return Field.CreateConditionCollectionProxy(this);
            }

            set
            {
                EnsureWritable();
                Field.UpdateFromConditionCollectionProxy(value);
            }
        }

        public int Duration
        {
            get
            {
                return (int)Field.Duration;
            }

            set
            {
                EnsureWritable();
                Field.Duration = (uint)value;
            }
        }

        public float Magnitude
        {
            get
            {
                return Field.Magnitude;
            }

            set
            {
                EnsureWritable();
                Field.Magnitude = value;
            }
        }
    }
}
