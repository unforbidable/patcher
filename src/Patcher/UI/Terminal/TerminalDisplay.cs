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

using Microsoft.Win32.SafeHandles;
using Patcher.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.UI.Terminal
{
    sealed class TerminalDisplay : IDisplay
    {
        [DllImport("kernel32.dll",
           EntryPoint = "GetStdHandle",
           SetLastError = true,
           CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        int prevTextLength = 0;
        int rewindTextLength = 0;

        public event EventHandler<LineWrittenEventArgs> OnLineWritten;

        public TerminalDisplay()
        {
            // Allocate console
            AllocConsole();
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
  
            // Capture CTRL+C
            Console.CancelKeyPress += CancelKeyPress;

            Console.BufferHeight = 1200;
        }

        public void Run(Task task)
        {
            // Console task runs synchronously
            task.RunSynchronously();
        }

        private void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            DoWriteLine(ConsoleColor.DarkCyan, "Program interrupted.");
        }

        public void Shutdown()
        {
            Console.WriteLine();
            DoWriteLine(ConsoleColor.DarkGreen, "Press any key to continue.");
            ReadUnbufferedKey(true);
        }

        public void SetWindowWidth(int windowWidth)
        {
            Console.WindowWidth = windowWidth;
        }

        public void SetWindowHeight(int windowHeight)
        {
            Console.WindowHeight = windowHeight;
        }

        internal void Rewind()
        {
            Console.CursorTop--;
            rewindTextLength = prevTextLength;
        }

        internal void WriteLine(LogLevel level, string text)
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

        internal void WriteLine(WriteLineMode mode, string text)
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

        public void ShowPreRunMessage(string message, bool isError)
        {
            DoWriteLine(isError ? ConsoleColor.Red : ConsoleColor.White, message);
            Shutdown();
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

        internal ConsoleKeyInfo ReadUnbufferedKey(bool intercept)
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


        public Status GetStatus()
        {
            return new TerminalStatus(this);
        }

        public Prompt GetPrompt()
        {
            return new TerminalPrompt(this);
        }

        public Logger GetLogger(LogLevel maxLogLevel)
        {
            return new TerminalLogger(this, maxLogLevel);
        }

    }
}
