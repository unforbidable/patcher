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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher
{
    public static class Log
    {
        private static List<Logger> loggers = new List<Logger>();

        internal static void AddLogger(Logger logger)
        {
            lock (loggers)
            {
                loggers.Add(logger);
            }
        }

        private static bool IsLevelLogged(LogLevel level)
        {
            lock (loggers)
                return loggers.Where(l => l.MaxLogLevel >= level).Any();
        }

        private static void WriteLogEntry(LogEntry entry)
        {
            lock (loggers)
                foreach (var logger in loggers.Where(l => l.MaxLogLevel >= entry.Level))
                    logger.WriteLogEntry(entry);
        }


        public static void Error(string message)
        {
            if (IsLevelLogged(LogLevel.Error))
                WriteLogEntry(new LogEntry(LogLevel.Error, null, message));
        }

        public static void Error(string message, params object[] args)
        {
            if (IsLevelLogged(LogLevel.Error))
                WriteLogEntry(new LogEntry(LogLevel.Error, null, string.Format(message, args)));
        }

        public static void Warning(string message)
        {
            if (IsLevelLogged(LogLevel.Warning))
                WriteLogEntry(new LogEntry(LogLevel.Warning, null, message));
        }

        public static void Warning(string message, params object[] args)
        {
            if (IsLevelLogged(LogLevel.Warning))
                WriteLogEntry(new LogEntry(LogLevel.Warning, null, string.Format(message, args)));
        }

        public static void Info(string message)
        {
            if (IsLevelLogged(LogLevel.Info))
                WriteLogEntry(new LogEntry(LogLevel.Info, null, message));
        }

        public static void Info(string message, params object[] args)
        {
            if (IsLevelLogged(LogLevel.Info))
                WriteLogEntry(new LogEntry(LogLevel.Info, null, string.Format(message, args)));
        }

        public static void Fine(string message)
        {
            if (IsLevelLogged(LogLevel.Fine))
                WriteLogEntry(new LogEntry(LogLevel.Fine, null, message));
        }

        public static void Fine(string message, params object[] args)
        {
            if (IsLevelLogged(LogLevel.Fine))
                WriteLogEntry(new LogEntry(LogLevel.Fine, null, string.Format(message, args)));
        }
    }
}
