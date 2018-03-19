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
    public class JsonObject : IJsonPropertyValue
    {
        public Dictionary<string, IJsonPropertyValue> Properties { get; private set; } = new Dictionary<string, IJsonPropertyValue>();

        public bool HasProperty(string name)
        {
            return Properties.ContainsKey(name);
        }

        public JsonString GetPropertyString(string name)
        {
            if (!HasProperty(name))
            {
                return null;
            }
            else
            {
                return Properties[name] as JsonString;
            }
        }

        public JsonObject GetPropertyObject(string name)
        {
            if (!HasProperty(name))
            {
                return null;
            }
            else
            {
                return Properties[name] as JsonObject;
            }
        }

        public JsonArray GetPropertyArray(string name)
        {
            if (!HasProperty(name))
            {
                return null;
            }
            else
            {
                return Properties[name] as JsonArray;
            }
        }
    }
}
