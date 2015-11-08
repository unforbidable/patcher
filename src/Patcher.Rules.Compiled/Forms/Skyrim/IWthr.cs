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

using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Weather</b> form.
    /// </summary>
    public interface IWthr : IForm
    {
        /// <summary>
        /// Gets or sets the <b>Shader Particle Geometry</b> use for rain or snow.
        /// </summary>
        ISpgd PrecipitationParticle { get; set; }
        /// <summary>
        /// Gets or sets the <b>Visual Effect</b> for this <b>Weather</b>.
        /// </summary>
        IRfct VisualEffect { get; set; }
        /// <summary>
        /// Gets or sets the collection of sounds associated with this <b>Weather</b>.
        /// </summary>
        IWeatherSoundCollection Sounds { get; set; }
        /// <summary>
        /// Gets or sets the collection of <b>Static</b> objects which make up the sky for this <b>Weather</b>.
        /// </summary>
        IFormCollection<IStat> SkyStatics { get; set; }
        /// <summary>
        /// Gets or sets the near fog distance during the day.
        /// </summary>
        float FogDayNear { get; set; }
        /// <summary>
        /// Gets or sets the far fog distance during the day.
        /// </summary>
        float FogDayFar { get; set; }
        /// <summary>
        /// Gets or sets the near fog distance during the night.
        /// </summary>
        float FogNightNear { get; set; }
        /// <summary>
        /// Gets or sets the far fog distance during the night.
        /// </summary>
        float FogNightFar { get; set; }
        /// <summary>
        /// Gets or sets the power of fog distance during the day.
        /// </summary>
        float FogDayPow { get; set; }
        /// <summary>
        /// Gets or sets the power of fog distance during the night.
        /// </summary>
        float FogNightPow { get; set; }
        /// <summary>
        /// Gets or sets the maximum fog distance during the night.
        /// </summary>
        float FogDayMax { get; set; }
        /// <summary>
        /// Gets or sets the maximum fog distance during the night.
        /// </summary>
        float FogNightMax { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the sunrise.
        /// </summary>
        IImgs SunriseImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the day.
        /// </summary>
        IImgs DayImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the sunset.
        /// </summary>
        IImgs SunsetImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the night.
        /// </summary>
        IImgs NightImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the speed of wind for this <b>Weather</b>.
        /// </summary>
        float WindSpeed { get; set; }
        /// <summary>
        /// Gets or sets the trannsition delta of this <b>Weather</b>.
        /// </summary>
        float TransDelta { get; set; }
        /// <summary>
        /// Gets or sets the sun glare intensity for this <b>Weather</b>.
        /// </summary>
        float SunGlare { get; set; }
        /// <summary>
        /// Gets or sets the multiplier of the damage caused by the sun to vampires.
        /// </summary>
        float SunDamage { get; set; }
        /// <summary>
        /// Gets or sets the point of transition into this <b>Weather</b> when the precipitation begins.
        /// </summary>
        float PrecipitationBeginFadeIn { get; set; }
        /// <summary>
        /// Gets or sets the point of transition from this <b>Weather</b> when the precipitation ends.
        /// </summary>
        float PrecipitationEndFadeOut { get; set; }
        /// <summary>
        /// Gets or sets the point of transition into this <b>Weather</b> when the lightning begins.
        /// </summary>
        float ThunderBeginFadeIn { get; set; }
        /// <summary>
        /// Gets or sets the point of transition from this <b>Weather</b> when the lightning ends.
        /// </summary>
        float ThunderEndFadeOut { get; set; }
        /// <summary>
        /// Gets or sets the frequency of lightnings. The lesser value the more often lightnings occur.
        /// </summary>
        float ThunderFrequency { get; set; }
        /// <summary>
        /// Gets or sets the color of the lightning during this <b>Weather</b>.
        /// </summary>
        IColor LightningColor { get; set; }
        /// <summary>
        /// Gets or sets the angle that indicates the wind direction.
        /// </summary>
        float WindDirection { get; set; }
        /// <summary>
        /// Gets or sets the angle that indicates the allowed deviation of wind direction.
        /// </summary>
        float WindDirectionRange { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether this weather is pleasant.
        /// </summary>
        bool IsPleasant { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether this weather is cloudy.
        /// </summary>
        bool IsCloudy { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether this weather is rainy.
        /// </summary>
        bool IsRainy { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether this weather is snowy.
        /// </summary>
        bool IsSnowy { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether the aurora follows the sun.
        /// </summary>
        bool AuroraFollowsSun { get; set; }
        /// <summary>
        /// Gets or sets the value which determines whether the <b>Weather</b> effects are always visible.
        /// </summary>
        bool EffectsAlwaysVisible { get; set; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative X-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsX1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive X-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsX2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Y-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsY1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Y-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsY2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Z-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsZ1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Z-axis direction.
        /// </summary>
        IWeatherColorQuad AmbientColorsZ2 { get; }
        /// <summary>
        /// Gets the specular lighting colors.
        /// </summary>
        IWeatherColorQuad SpecularColors { get; }
        /// <summary>
        /// Gets the Fresnel powers.
        /// </summary>
        IWeatherFresnelQuad FresnelPowers { get; }
        /// <summary>
        /// Retrieves the specified cloud layer (0-31).
        /// </summary>
        /// <param name="layer">The number of the cloud layer.</param>
        /// <returns></returns>
        IWeatherCloudLayer GetCloudLayer(int layer);
        /// <summary>
        /// Retrieves the colors which defines specified component of this <b>Weather</b> (0-16).
        /// </summary>
        /// <param name="component">The number of the component.</param>
        /// <returns></returns>
        IWeatherColorQuad GetColors(int component);
        /// <summary>
        /// Retreives an enumerable collection of all cloud layers.
        /// </summary>
        IEnumerable<IWeatherCloudLayer> CloudLayers { get; }
        /// <summary>
        /// Retreives an enumerable collection of colors of all components.
        /// </summary>
        IEnumerable<IWeatherColorQuad> Colors { get; }
    }
}
