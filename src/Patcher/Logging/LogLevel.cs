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
using System.Threading.Tasks;

namespace Patcher.Logging
{
    /// <summary>
    /// Specifies the log message levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Represents log level value that includes to no messages.
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents log level value that includes only errors.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Represents log level value that includes errors and warnings.
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Represents log level value that includes errors, warnings and status messages.
        /// </summary>
        Info = 3,
        /// <summary>
        /// Represents log level value that includes all messages.
        /// </summary>
        Fine = 4
    }
}
