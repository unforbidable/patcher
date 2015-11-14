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

namespace Patcher.Rules.Compiled.Forms.Fallout4
{
    /// <summary>
    /// Represents a <b>Keyword</b> form.
    /// </summary>
    /// <remarks>
    /// The meaning of unknown properties is not known so use them with caution. 
    /// Unknown properties will be removed and replaced with proper properties as soon as more information is available.
    /// </remarks>
    public interface IKywd : IForm
    {
        /// <summary>
        /// Gets or sets the color of this <b>Keyword</b>.
        /// </summary>
        IColor Color { get; set; }
        /// <summary>
        /// Gets or sets an unknown number.
        /// </summary>
        int UnknownNumber { get; set; }
        /// <summary>
        /// Gets or sets the short name (only used by some keywords).
        /// </summary>
        string ShortName { get; set; }
        /// <summary>
        /// Gets or sets the full in-game name (only used by some keywords).
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Gets or sets the furniture activation rule.
        /// </summary>
        IAoru ActivationRule { get; set; }

    }
}
