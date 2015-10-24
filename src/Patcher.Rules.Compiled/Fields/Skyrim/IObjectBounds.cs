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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Fields.Skyrim
{
    /// <summary>
    /// Specifies the dimensions of an object.
    /// </summary>
    public interface IObjectBounds
    {
        /// <summary>
        /// Gets or sets the negative X-axis dimension.
        /// </summary>
        short X1 { get; set; }
        /// <summary>
        /// Gets or sets the negative Y-axis dimension.
        /// </summary>
        short Y1 { get; set; }
        /// <summary>
        /// Gets or sets the negative Z-axis dimension.
        /// </summary>
        short Z1 { get; set; }
        /// <summary>
        /// Gets or sets the positive X-axis dimension.
        /// </summary>
        short X2 { get; set; }
        /// <summary>
        /// Gets or sets the positive Y-axis dimension.
        /// </summary>
        short Y2 { get; set; }
        /// <summary>
        /// Gets or sets the positive Z-axis dimension.
        /// </summary>
        short Z2 { get; set; }
    }
}
