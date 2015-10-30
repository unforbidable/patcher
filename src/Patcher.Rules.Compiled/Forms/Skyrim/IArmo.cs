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
using Patcher.Rules.Compiled.Fields.Skyrim;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents an <b>Armor</b> form.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Property <code>Skill</code> can be assigned values <code>Skills.LightArmor</code>, <code>Skills.HeavyArmor</code> and <code>Skills.None</code> only. 
    /// If no value is assigned the value will default to <code>Skills.LightArmor</code> for any newly created <b>Armor</b> forms.
    /// </p>
    /// <p>
    /// Property <code>BodyNodes</code> can be assigned a set of values combined with <code>|</code> operator. 
    /// To determine whether a specific body node has been set (in a if statement or a where clause) method <code>HasFlag()</code> can be used.
    /// </p>
    /// </remarks>
    public interface IArmo : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IScriptCollection"/> containing <see cref="IScript">Scripts</see> attached to this <b>Armor</b>.
        /// </summary>
        IScriptCollection Scripts { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Armor</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }

        /// <summary>
        /// Gets or sets the in-game name of this <b>Armor</b>.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the <b>Enchantment</b> of this <b>Armor</b>.
        /// </summary>
        IEnch Enchantment { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Armor</b> when the player character is a male.
        /// </summary>
        string MaleWorldModel { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Armor</b> when the player character is a female.
        /// </summary>
        string FemaleWorldModel { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Skills">Skill</see> that will be used when this <b>Armor</b> is worn.
        /// </summary>
        Skills Skill { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Constants.Skyrim.BodyNodes"/> used to determine which body parts this <b>Armor</b> will use.
        /// </summary>
        BodyNodes BodyNodes { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Armor</b> can be worn by the player character.
        /// </summary>
        bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this item is a shield.
        /// </summary>
        bool IsShield { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Armor</b> is picked up.
        /// </summary>
        ISndr PickUpSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Armor</b> is put down.
        /// </summary>
        ISndr PutDownSound { get; set; }

        /// <summary>
        /// Gets or sets the shield <b>Equip Type</b> (shields only).
        /// </summary>
        IEqup EquipType { get; set; }

        /// <summary>
        /// Gets or sets the shield bash <b>Impact Data Set</b> (shields only).
        /// </summary>
        IIpds BlockImpactDataSet { get; set; }

        /// <summary>
        /// Gets or sets the alternate <b>Material</b> used when bashing or blocking (shields only).
        /// </summary>
        IMatt AlternateBlockMaterial { get; set; }

        /// <summary>
        /// Gets or sets the <b>Race</b> that is allowed to wear this <b>Armor</b>.
        /// </summary>
        IRace Race { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IKywd}"/> containing <b>Keywords</b> associated with this <b>Armor</b>.
        /// </summary>
        IFormCollection<IKywd> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the description of this <b>Armor</b> that will override the enchantment description if provided.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IForm}"/> containing <b>Armor Addons</b> used by this <b>Armor</b>.
        /// </summary>
        IFormCollection<IForm> Models { get; set; }

        /// <summary>
        /// Gets or sets the value of this <b>Armor</b>.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the weight of this <b>Armor</b>.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Gets or sets the armor rating of this <b>Armor</b>.
        /// </summary>
        float ArmorRating { get; set; }

        /// <summary>
        /// Gets or sets the <b>Armor</b> that will be used as the template.
        /// </summary>
        IArmo TemplateArmor { get; set; }
    }
}
