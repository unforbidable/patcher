// Copyright(C) 2015,2016,2017,2018 Unforbidable Works
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or(at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization.Json
{
    public class JsonToken
    {
        readonly bool quoted;
        readonly string text;
        readonly char c;

        public string Text { get { return text ?? c.ToString(); } }
        public bool IsQuoted { get { return quoted; } }
        public bool IsText { get { return text != null; } }
        public bool IsBeginArray { get { return c == '['; } }
        public bool IsEndArray { get { return c == ']'; } }
        public bool IsBeginObject { get { return c == '{'; } }
        public bool IsEndObject { get { return c == '}'; } }
        public bool IsSeparator { get { return c == ','; } }
        public bool IsAssignment { get { return c == ':'; } }

        public JsonToken(char c)
        {
            this.c = c;
        }

        public JsonToken(string text, bool quoted)
        {
            this.text = text;
            this.quoted = quoted;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
