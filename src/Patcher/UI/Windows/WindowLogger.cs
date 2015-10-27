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
            display.Window.WriteMessage(entry);
        }
    }
}
