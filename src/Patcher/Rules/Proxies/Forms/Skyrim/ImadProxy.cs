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

using Patcher.Data.Plugins.Content.Records;
using Patcher.Rules.Compiled.Forms.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Proxies.Fields;

namespace Patcher.Rules.Proxies.Forms.Skyrim
{
    [Proxy(typeof(IImad))]
    public sealed class ImadProxy : FormProxy<Imad>, IImad
    {
        public IEnumerable<ITimeFloat> BrightnessAdditives
        {
            get
            {
                return record.GetBrightnessAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> BrightnessMultipliers
        {
            get
            {
                return record.GetBrightnessMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> ContrastAdditives
        {
            get
            {
                return record.GetContrastAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> ContrastMultipliers
        {
            get
            {
                return record.GetContrastMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> SaturationAdditives
        {
            get
            {
                return record.GetSaturationAdd().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }

        public IEnumerable<ITimeFloat> SaturationMultipliers
        {
            get
            {
                return record.GetSaturationMult().Select(i => Provider.CreateProxy<TimeFloatProxy>(Mode).With(i));
            }
        }
    }
}
