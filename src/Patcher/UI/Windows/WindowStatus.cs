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
