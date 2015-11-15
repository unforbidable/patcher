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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents an <b>Image Space Adapter</b> form.
    /// </summary>
    public interface IImad : IForm
    {
        /// <summary>
        /// Retreives an enumerable collection of Saturation multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> SaturationMultipliers { get; }

        /// <summary>
        /// Retreives an enumerable collection of Saturation additives.
        /// </summary>
        IEnumerable<ITimeFloat> SaturationAdditives { get; }

        /// <summary>
        /// Retreives an enumerable collection of Brightness multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> BrightnessMultipliers { get; }

        /// <summary>
        /// Retreives an enumerable collection of Brightness additives.
        /// </summary>
        IEnumerable<ITimeFloat> BrightnessAdditives { get; }

        /// <summary>
        /// Retreives an enumerable collection of Contrast multipliers.
        /// </summary>
        IEnumerable<ITimeFloat> ContrastMultipliers { get; }

        /// <summary>
        /// Retreives an enumerable collection of Contrast additives.
        /// </summary>
        IEnumerable<ITimeFloat> ContrastAdditives { get; }
    }
}
