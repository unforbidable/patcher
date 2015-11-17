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

using Patcher.Rules.Compiled.Constants.Skyrim;
using Patcher.Rules.Compiled.Fields;
using Patcher.Rules.Compiled.Fields.Skyrim;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents a <b>Potion</b> form.
    /// </summary>
    /// <remarks>
    /// <b>Potion</b> forms include drinks and food as well.
    /// </remarks>
    public interface IAlch : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Potion</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }

        /// <summary>
        /// Gets or sets the in-game name of this <b>Potion</b>.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IKywd}"/> containing <b>Keywords</b> associated with this <b>Potion</b>.
        /// </summary>
        IFormCollection<IKywd> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the description of this <b>Potion</b> that will override the enchantment description if provided.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Potion</b>.
        /// </summary>
        string WorldModel { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Potion</b> is picked up.
        /// </summary>
        ISndr PickUpSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Potion</b> is put down.
        /// </summary>
        ISndr PutDownSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Potion</b> is used.
        /// </summary>
        ISndr UseSound { get; set; }

        /// <summary>
        /// Gets or sets the weight of this <b>Potion</b>.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of this <b>Potion</b>.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the type of this <b>Potion</b>.
        /// </summary>
        PotionTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEffectCollection"/> contaning <see cref="IEffect">Effects</see> of this <b>Potion</b>.
        /// </summary>
        IEffectCollection Effects { get; set; }
    }
}
