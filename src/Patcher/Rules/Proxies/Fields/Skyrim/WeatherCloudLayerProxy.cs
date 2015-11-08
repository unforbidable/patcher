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
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Proxies.Fields.Skyrim
{
    [Proxy(typeof(IWeatherCloudLayer))]
    public sealed class WeatherCloudLayerProxy : Proxy, IWeatherCloudLayer
    {
        Wthr.CloudLayer target = null;

        internal WeatherCloudLayerProxy With(Wthr.CloudLayer target)
        {
            // Reset cached subproxies
            alphas = null;
            colors = null;

            this.target = target;
            return this;
        }

        WeatherAlphaQuadProxy alphas = null;
        WeatherColorQuadProxy colors = null;

        public IWeatherAlphaQuad Alphas
        {
            get
            {
                if (alphas == null)
                    alphas = Provider.CreateProxy<WeatherAlphaQuadProxy>(Mode).With(target.Alphas);
                return alphas;
            }
        }

        public IWeatherColorQuad Colors
        {
            get
            {
                if (colors == null)
                    colors = Provider.CreateProxy<WeatherColorQuadProxy>(Mode).With(target.Colors);
                return colors;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return target.IsEnabled;
            }

            set
            {
                EnsureWritable();
                target.IsEnabled = value;
            }
        }

        public float SpeedX
        {
            get
            {
                return target.SpeedX;
            }

            set
            {
                EnsureWritable();
                target.SpeedX = value;
            }
        }

        public float SpeedY
        {
            get
            {
                return target.SpeedY;
            }

            set
            {
                EnsureWritable();
                target.SpeedY = value;
            }

        }

        public string Texture
        {
            get
            {
                return target.Texture;
            }

            set
            {
                EnsureWritable();
                target.Texture = value;
            }

        }
    }
}
