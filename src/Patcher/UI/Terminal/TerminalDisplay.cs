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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.UI.Terminal
{
    sealed class TerminalDisplay
    {
        int prevTextLength = 0;
        int rewindTextLength = 0;
        LogLevel minimalLogSeverity = LogLevel.Fine;

        public event EventHandler<LineWrittenEventArgs> OnLineWritten;

        readonly TerminalLogger logger;

        public LogLevel ConsoleLogLevel { get { return logger.MaxLogLevel; } set { logger.SetMaxLogLevel(value); } }

        public TerminalDisplay()
        {
            // Setup terminal logger
            logger = new TerminalLogger(this);
            Log.AddLogger(logger);

            // Capture CTRL+C
            Console.CancelKeyPress += CancelKeyPress;
        }

        private void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            DoWriteLine(ConsoleColor.DarkCyan, "Program interrupted.");
        }

        public void Pause()
        {
            Console.WriteLine();
            DoWriteLine(ConsoleColor.DarkGreen, "Press any key to continue.");
            ReadUnbufferedKey(true);
        }

        public void SetMinimalLogSeverity(LogLevel minimalLogSeverity)
        {
            this.minimalLogSeverity = minimalLogSeverity;
        }

        public void Rewind()
        {
            Console.CursorTop--;
            rewindTextLength = prevTextLength;
        }

        public void WriteLine(LogLevel level, string text)
        {
            switch (level)
            {
                case LogLevel.Error:
                    DoWriteLine(ConsoleColor.DarkRed, text);
                    break;

                case LogLevel.Warning:
                    DoWriteLine(ConsoleColor.DarkYellow, text);
                    break;

                case LogLevel.Fine:
                    DoWriteLine(ConsoleColor.DarkGray, text);
                    break;

                default:
                    DoWriteLine(ConsoleColor.Gray, text);
                    break;
            }

            RaiseLineWrittenEvent(WriteLineMode.Log);
        }

        public void WriteLine(WriteLineMode mode, string text)
        {
            switch (mode)
            {
                case WriteLineMode.System:
                    DoWriteLine(ConsoleColor.DarkGreen, text);
                    break;


                case WriteLineMode.Status:
                    DoWriteLine(ConsoleColor.DarkGreen, text);
                    break;

                case WriteLineMode.Prompt:
                    DoWriteLine(ConsoleColor.Magenta, text);
                    break;

                default:
                    DoWriteLine(ConsoleColor.White, text);
                    break;
            }

            RaiseLineWrittenEvent(mode);
        }

        private void RaiseLineWrittenEvent(WriteLineMode mode)
        {
            var handler = OnLineWritten;
            if (handler != null)
            {
                handler.Invoke(this, new LineWrittenEventArgs(mode));
            }
        }

        private void DoWriteLine(ConsoleColor color, string text)
        {
            lock (this)
            {
                ConsoleColor prev = Console.ForegroundColor;
                Console.ForegroundColor = color;

                if (rewindTextLength > text.Length)
                {
                    string pad = string.Concat(Enumerable.Range(0, rewindTextLength - text.Length).Select(n => " "));
                    Console.WriteLine(text + pad);
                    rewindTextLength = 0;
                }
                else
                {
                    Console.WriteLine(text);
                }

                prevTextLength = text.Length;
                rewindTextLength = 0;

                Console.ForegroundColor = prev;
            }
        }

        public ConsoleKeyInfo ReadUnbufferedKey(bool intercept)
        {
            var sw = new Stopwatch();
            sw.Start();

            // Read keys pressed before this method was called
            // This is done by consuming keys until enough time passed
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept);

                if (sw.ElapsedMilliseconds > 50)
                    return keyInfo;
            }
        }
    }
}
