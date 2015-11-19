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

using Patcher.Rules.Compiled.Fields.Fallout4;
using Patcher.Rules.Compiled.Forms;
using Patcher.Rules.Compiled.Forms.Fallout4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers.Fallout4
{
    /// <summary>
    /// Provides access to the Engine such as the parameters.
    /// </summary>
    public interface IEngineHelper
    {
        /// <summary>
        /// Gets the specified string parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetParam(string name, string defaultValue);
        /// <summary>
        /// Gets the specified integer parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        int GetParam(string name, int defaultValue);
        /// <summary>
        /// Gets the specified floating point parameter value from command line, or returns the default value if parameter is not defined.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        float GetParam(string name, float defaultValue);
    }
}
