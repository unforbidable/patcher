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
using Patcher.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Patcher
{
    public static class Display
    {
        static IDisplay display = null;
        static Progress currentProgess = null;

        internal static void SetDisplay(IDisplay display)
        {
            Display.display = display;
        }

        public static ChoiceOption Choice(string message, params ChoiceOption[] options)
        {
            return display.OfferChoice(message, options.Select(o => new Choice(o)).ToArray()).GetOption();
        }

        public static Progress StartProgress(string completionMessage)
        {
            if (currentProgess != null)
                throw new InvalidOperationException("The maximum number of concurent progresses has been reached.");

            currentProgess = new Progress(completionMessage);
            currentProgess.Disposed += CurrentProgess_Disposed;
            display.StartProgress(currentProgess);
            return currentProgess;
        }

        public static void WriteText(string format, params object[] args)
        {
            DoWriteText(string.Format(format, args));
        }

        public static void WriteText(string text)
        {
            DoWriteText(text);
        }

        private static void DoWriteText(string text)
        {
            StringReader reader = new StringReader(text);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                display.WriteText(line);
            }
        }

        private static void CurrentProgess_Disposed(object sender, EventArgs e)
        {
            if (currentProgess != sender)
                throw new InvalidOperationException("An unexpected progress has been disposed.");

            currentProgess = null;
        }
    }
}
