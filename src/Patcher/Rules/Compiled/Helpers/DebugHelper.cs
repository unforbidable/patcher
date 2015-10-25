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
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.\

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.Rules.Compiled.Helpers
{
    [Helper("Debug", typeof(IDebugHelper))]
    sealed class DebugHelper : IDebugHelper
    {
        readonly CompiledRuleContext context;

        public DebugHelper(CompiledRuleContext context)
        {
            this.context = context;
        }

        public void Break()
        {
            Debugger.Break();
        }

        public void Message(string text)
        {
            Log.Fine("[MESSAGE] {0}", text);
        }

        public void Assert(bool condition, string text)
        {
            if (!condition)
                throw new CompiledRuleAssertException(text);
        }

        public void Dump(object value)
        {
            var dumper = new ObjectDumper("[DUMP]");
            dumper.DumpObject(value);
        }

        public void Dump(object value, string name)
        {
            var dumper = new ObjectDumper("[DUMP]");
            dumper.DumpObject(name, value);
        }

    }
}
