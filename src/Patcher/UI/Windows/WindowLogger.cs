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

using Patcher.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.UI.Windows
{
    class WindowLogger : Logger
    {
        readonly WindowDisplay display;
        LogLevel maxLogLevel;

        public WindowLogger(WindowDisplay display, LogLevel maxLogLevel)
        {
            this.display = display;
            this.maxLogLevel = maxLogLevel;
        }

        internal override LogLevel MaxLogLevel
        {
            get
            {
                return maxLogLevel;
            }
        }

        internal override void WriteLogEntry(LogEntry entry)
        {
            display.Window.WriteLogEntry(entry);
        }
    }
}
