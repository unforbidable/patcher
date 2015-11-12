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
using Patcher.Rules.Compiled.Fields.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Fallout4
{
    /// <summary>
    /// Represents a <b>Weather</b> form.
    /// </summary>
    public interface IWthr : IForm
    {
        /// <summary>
        /// Gets or sets the <b>Shader Particle Geometry</b> used to render rain or snow.
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
        /// Gets or sets the collection of <b>Static</b> objects which make up the sky dome of this <b>Weather</b>.
        /// </summary>
        IFormCollection<IStat> SkyStatics { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the early dawn.
        /// </summary>
        IImgs EarlyDawnImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the dawn.
        /// </summary>
        IImgs DawnImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the late dawn.
        /// </summary>
        IImgs LateDawnImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the day.
        /// </summary>
        IImgs DayImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the early dusk.
        /// </summary>
        IImgs EarlyDuskImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the dusk.
        /// </summary>
        IImgs DuskImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the late dusk.
        /// </summary>
        IImgs LateDuskImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the <b>Image Space</b> used during the night.
        /// </summary>
        IImgs NightImageSpace { get; set; }
        /// <summary>
        /// Gets or sets the speed of wind for this <b>Weather</b>.
        /// </summary>
        float WindSpeed { get; set; }
        /// <summary>
        /// Gets or sets the transition delta of this <b>Weather</b>.
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
        /// Gets or sets the point of transition into this <b>Weather</b> when the lightnings begins.
        /// </summary>
        float ThunderBeginFadeIn { get; set; }
        /// <summary>
        /// Gets or sets the point of transition from this <b>Weather</b> when the lightnings ends.
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
        /// Gets or sets the angle that specified the wind direction.
        /// </summary>
        float WindDirection { get; set; }
        /// <summary>
        /// Gets or sets the angle that specifies the allowed deviation of wind direction.
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
        IWeatherColorSet AmbientColorsX1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive X-axis direction.
        /// </summary>
        IWeatherColorSet AmbientColorsX2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Y-axis direction.
        /// </summary>
        IWeatherColorSet AmbientColorsY1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Y-axis direction.
        /// </summary>
        IWeatherColorSet AmbientColorsY2 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the negative Z-axis direction.
        /// </summary>
        IWeatherColorSet AmbientColorsZ1 { get; }
        /// <summary>
        /// Gets the colors of the ambient light in the positive Z-axis direction.
        /// </summary>
        IWeatherColorSet AmbientColorsZ2 { get; }
        /// <summary>
        /// Gets the specular lighting colors.
        /// </summary>
        IWeatherColorSet SpecularColors { get; }
        /// <summary>
        /// Gets the Fresnel powers.
        /// </summary>
        IWeatherFresnelSet FresnelPowers { get; }
        /// <summary>
        /// Retrieves the specified cloud layer (0-28).
        /// </summary>
        /// <param name="layer">The number of the cloud layer.</param>
        /// <returns></returns>
        IWeatherCloudLayer GetCloudLayer(int layer);
        /// <summary>
        /// Retrieves the colors which define thespecified component of this <b>Weather</b> (0-16).
        /// </summary>
        /// <param name="component">The number of the component.</param>
        /// <returns></returns>
        IWeatherColorSet GetColors(int component);
        /// <summary>
        /// Retreives an enumerable collection of all cloud layers which can be iterated using a <code>foreach</code> construct.
        /// </summary>
        IEnumerable<IWeatherCloudLayer> CloudLayers { get; }
        /// <summary>
        /// Retreives an enumerable collection of colors of all components which can be iterated using a <code>foreach</code> construct.
        /// </summary>
        IEnumerable<IWeatherColorSet> Colors { get; }
    }
}
