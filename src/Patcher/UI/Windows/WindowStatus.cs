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

namespace Patcher.UI.Windows
{
    sealed class WindowStatus : Status
    {
        readonly WindowDisplay display;

        public WindowStatus(WindowDisplay display)
        {
            this.display = display;
        }

        protected override void ProgressFinsihed(Progress progress, string message)
        {
            display.Window.ShowStatusMessage(message);
            display.Window.UpdateProgress(1f, 1f);
        }

        protected override void ProgressStarted(Progress progress)
        {
            display.Window.ShowStatusMessage(progress.Title);
            display.Window.UpdateProgress(0f, 1f);
        }

        DateTime lastTimeStatusUpdate = DateTime.MinValue;

        protected override void ProgressUpdated(Progress progress, ProgressUpdateInfo update)
        {
            var elapsed = DateTime.Now.Subtract(lastTimeStatusUpdate);
            if (elapsed.TotalMilliseconds > 1)
            {
                display.Window.ShowStatusMessage(progress.Title);
                display.Window.UpdateProgress((float)update.Current / update.Maximum, 1f);

                lastTimeStatusUpdate = DateTime.Now;
            }
        }

        protected override void ShowMessage(Message entry)
        {
            display.Window.ShowStatusMessage(entry.Text);
        }
    }
}
