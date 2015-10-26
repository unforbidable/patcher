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

using Patcher.Rules.Compiled.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents an <b>Ammo</b> form.
    /// </summary>
    public interface IAmmo : IForm
    {
        /// <summary>
        /// Gets or sets the <see cref="IObjectBounds"/> of this <b>Ammo</b>.
        /// </summary>
        IObjectBounds ObjectBounds { get; set; }

        /// <summary>
        /// Gets or sets the in-game name of this <b>Ammo</b>.
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        /// Gets or sets the short name of this <b>Ammo</b>.
        /// </summary>
        string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the description of this <b>Ammo</b>.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormCollection{IKywd}"/> containing <b>Keywords</b> associated with this <b>Ammo</b>.
        /// </summary>
        IFormCollection<IKywd> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the path to the world model (.nif file) that will be used for this <b>Ammo</b>.
        /// </summary>
        string WorldModel { get; set; }

        /// <summary>
        /// Gets or sets the path to the inventory image (.dds file) that will be used for this <b>Ammo</b>.
        /// </summary>
        string InventoryImage { get; set; }

        /// <summary>
        /// Gets or sets the path to the message icon (.dds file) that will be used for this <b>Ammo</b>.
        /// </summary>
        string MessageIcon { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Ammo</b> is picked up.
        /// </summary>
        IForm PickUpSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Sound</b> that plays when this <b>Ammo</b> is put down.
        /// </summary>
        IForm PutDownSound { get; set; }

        /// <summary>
        /// Gets or sets the <b>Projectile</b> that is launched when this <b>Ammo</b> is used.
        /// </summary>
        IProj Projectile { get; set; }

        /// <summary>
        /// Gets or sets the damage caused by this <b>Ammo</b>.
        /// </summary>
        float Damage { get; set; }

        /// <summary>
        /// Gets or sets the value of this <b>Ammo</b>.
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Ammo</b> can be used by the player character.
        /// </summary>
        bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Ammo</b> is a bolt.
        /// </summary>
        bool IsBolt { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether this <b>Ammo</b> ignores normal weapon damage resistance.
        /// </summary>
        bool IgnoresNormalWeaponResistance { get; set; }

    }
}
