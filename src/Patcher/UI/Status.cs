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

namespace Patcher.UI
{
    public abstract class Status
    {
        const int maxProgressesAtOnce = 1;
        Stack<Progress> progressStack = new Stack<Progress>();

        public void Message(string text)
        {
            ShowMessage(new Message()
            {
                Text = text
            });
        }

        public Progress StartProgress(string completionMessage)
        {
            if (progressStack.Count >= maxProgressesAtOnce)
                throw new InvalidOperationException("The maximum number of concurent progresses has been reached.");

            var progress = new Progress(this, completionMessage);
            progressStack.Push(progress);
            ProgressStarted(progress);
            return progress;
        }


        internal void UpdateProgress(Progress progress, ProgressUpdateInfo update)
        {
            if (progressStack.Peek() != progress)
                throw new InvalidOperationException("Only the current progress can be updated.");

            ProgressUpdated(progress, update);
        }

        internal void DisposeProgress(Progress progress, string message)
        {
            if (progressStack.Peek() != progress)
                throw new InvalidOperationException("Only the current progress can be finsihed.");

            ProgressFinsihed(progress, message);

            progressStack.Pop();
        }

        protected abstract void ShowMessage(Message entry);
        protected abstract void ProgressStarted(Progress progress);
        protected abstract void ProgressUpdated(Progress progress, ProgressUpdateInfo update);
        protected abstract void ProgressFinsihed(Progress progress, string message);
    }
}
