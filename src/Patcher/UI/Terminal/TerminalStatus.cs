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

namespace Patcher.UI.Terminal
{
    sealed class TerminalStatus : Status
    {
        readonly TerminalDisplay terminal;

        DateTime lastProgressDisplayed = DateTime.MinValue;
        ProgressUpdateInfo lastProgressUpdate = null;

        public TerminalStatus(TerminalDisplay terminal)
        {
            this.terminal = terminal;

            terminal.OnLineWritten += Terminal_OnLineWritten;
        }

        private void Terminal_OnLineWritten(object sender, LineWrittenEventArgs e)
        {
            // Repeats overwriten progress lines
            // (not after status messages)
            if (e.Mode != WriteLineMode.Status)
            {
                var lastUpdate = lastProgressUpdate;
                if (lastUpdate != null)
                {
                    lastProgressDisplayed = DateTime.MinValue;
                    ShowProgress(lastUpdate);
                }
            }
        }

        protected override void ShowMessage(Message message)
        {
            terminal.WriteLine(WriteLineMode.Status, message.Text);
        }

        protected override void ProgressStarted(Progress progress)
        {
            lastProgressDisplayed = DateTime.MinValue;
        }

        protected override void ProgressUpdated(Progress progress, ProgressUpdateInfo update)
        {
            lastProgressUpdate = update;
            ShowProgress(update);
        }

        protected override void ProgressFinsihed(Progress progress, string message)
        {
            lastProgressUpdate = null;
            terminal.WriteLine(WriteLineMode.Status, message);
            lastProgressDisplayed = DateTime.MinValue;
        }

        private void ShowProgress(ProgressUpdateInfo update)
        {
            // Do not show progress if to few time passed since last time, unless its the last update
            if (DateTime.Now.Subtract(lastProgressDisplayed).TotalMilliseconds < 200 && update.Current != update.Maximum)
                return;

            lock (this)
            {
                int maxlength = update.Maximum.ToString().Length;
                string zeros = string.Concat(Enumerable.Range(0, maxlength).Select(n => "#"));
                terminal.WriteLine(WriteLineMode.Status, string.Format("    {3:00.0}% [ {1," + maxlength + ":" + zeros + "0} / {2} ]{0}",
                    (!string.IsNullOrEmpty(update.Text) ? string.Format(" ({0})", update.Text) : ""),
                    update.Current,
                    update.Maximum,
                    Math.Round((double)update.Current * 100 / update.Maximum, 1)));
                terminal.Rewind();
             }

            lastProgressDisplayed = DateTime.Now;
        }
    }
}
