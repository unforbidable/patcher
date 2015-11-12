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

using Patcher.Data.Plugins.Content.Fields.Fallout4;
using Patcher.Data.Plugins.Content.Records.Fallout4;
using Patcher.Rules.Compiled.Constants.Fallout4;
using Patcher.Rules.Compiled.Fields.Fallout4;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Fallout4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields.Fallout4
{
    [Proxy(typeof(IWeatherSoundCollection))]
    public sealed class WeatherSoundCollectionProxy : FieldCollectionProxy<WeatherSoundItem>, IWeatherSoundCollection
    {
        internal Wthr Target { get; set; }

        public int Count
        {
            get
            {
                return GetFieldCount();
            }
        }

        protected override List<WeatherSoundItem> Fields
        {
            get
            {
                return Target.Sounds;
            }
        }

        public void Clear()
        {
            EnsureWritable();
            ClearFields();
        }

        public void Add(ISndr sound, WeatherSoundTypes type)
        {
            EnsureWritable();

            AddField(new WeatherSoundItem()
            {
                Sound = sound.ToFormId(),
                Type = type.ToWeatherSoundType()
            }, false);
        }

        public void Add(IWeatherSound item)
        {
            EnsureWritable();

            // Add a copy
            AddField(item.ToField(), true);
        }

        public void Remove(IWeatherSound item)
        {
            EnsureWritable();

            // Remove by object reference - retrived during an iteration
            RemoveField(item.ToField());
        }

        public IEnumerator<IWeatherSound> GetEnumerator()
        {
            return GetFieldEnumerator<IWeatherSound>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void EnsureBackingListExists()
        {
            if (Target.Sounds == null)
                Target.Sounds = new List<WeatherSoundItem>();
        }
    }
}
