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
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IFlst))]
    public sealed class FlstProxy : FormProxy<Flst>, IFlst
    {
        FormCollectionProxy<IForm> forms = null;

        public IFormCollection<IForm> Items { get { return GetForms(); } set { EnsureWritable(); SetForms(value); } }

        private void SetForms(IFormCollection<IForm> value)
        {
            record.Items = value.Select(f => f.FormId).ToList();

            // Reset cached proxy
            forms = null;
        }

        private IFormCollection<IForm> GetForms()
        {
            if (forms == null)
            {
                forms = Provider.CreateFormCollectionProxy<IForm>(Mode, record.Items);
            }
            return forms;
        }


    }
}
