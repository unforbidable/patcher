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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Patcher.UI
{
    public sealed class Progress : IDisposable
    {
        public string Title { get; set; }

        Stopwatch stopwatch = new Stopwatch();

        readonly Status status;

        internal Progress(Status status, string title)
        {
            this.status = status;
            Title = title;
            stopwatch.Start();
        }

        public void Update(long current, long maximum, string text)
        {
            DoUpdate(current, maximum, text);
        }

        public void Update(long current, long maximum, string text, params object[] args)
        {
            DoUpdate(current, maximum, string.Format(text, args));
        }

        private void DoUpdate(long current, long maximum, string text)
        {
            status.UpdateProgress(this, new ProgressUpdateInfo()
            {
               Current = current,
               Maximum = maximum,
               Text = string.Format("{0}: {1}", Title, text)
            });
        }

        public void Dispose()
        {
            stopwatch.Stop();
            status.DisposeProgress(this, string.Format("{0} finished.", Title));
            Log.Fine("{0} finished in {1} ms", Title, stopwatch.ElapsedMilliseconds);
        }
    }
}
