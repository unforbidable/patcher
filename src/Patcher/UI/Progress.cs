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
        public string Title { get; private set; }
        public long Current { get; private set; }
        public long Total { get; private set; }
        public string Text { get; private set; }
        public bool IsCompleted { get { return Current == Total; } }

        public event EventHandler Updated;
        internal event EventHandler Disposed;

        Stopwatch stopwatch = new Stopwatch();
        int lastUpdateTicks = 0;

        const int minUpdateInterval = 50;

        internal Progress(string title)
        {
            Title = title;
            stopwatch.Start();
        }

        public void Update(long current, long total, string text)
        {
            if (CanUpdate(current, total))
            {
                DoUpdate(current, total, text);
            }
        }

        public void Update(long current, long total, string text, params object[] args)
        {
            if (CanUpdate(current, total))
            {
                DoUpdate(current, total, string.Format(text, args));
            }
        }

        private bool CanUpdate(long current, long total)
        {
            return current == 0 || current == total || Environment.TickCount - lastUpdateTicks > minUpdateInterval;
        }

        private void DoUpdate(long current, long total, string text)
        {
            Current = current;
            Total = total;
            Text = text;

            EventHandler temp = Updated;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }

            lastUpdateTicks = Environment.TickCount;
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Log.Fine("{0} finished in {1} ms", Title, stopwatch.ElapsedMilliseconds);

            EventHandler temp = Disposed;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }
    }
}
