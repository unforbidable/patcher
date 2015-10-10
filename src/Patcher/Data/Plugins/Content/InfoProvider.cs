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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    static class InfoProvider
    {
        static IDictionary<string, CompoundInfo> compounds = new SortedDictionary<string, CompoundInfo>();
        static IDictionary<string, RecordInfo> records = new SortedDictionary<string, RecordInfo>();
        static IDictionary<string, FieldInfo> fields = new SortedDictionary<string, FieldInfo>();

        public static CompoundInfo GetCompoundInfo(Type type)
        {
            lock (compounds)
            {
                if (!compounds.ContainsKey(type.FullName))
                {
                    compounds.Add(type.FullName, new CompoundInfo(type));
                }
                return compounds[type.FullName];
            }
        }

        public static RecordInfo GetRecordInfo(Type type)
        {
            lock (records)
            {
                if (!records.ContainsKey(type.FullName))
                {
                    records.Add(type.FullName, new RecordInfo(type));
                }
                return records[type.FullName];
            }
        }

        public static FieldInfo GetFieldInfo(Type type)
        {
            if (!typeof(Field).IsAssignableFrom(type))
                throw new InvalidOperationException("Cannot retrieve field info of a filed that is not derived from Field class");

            lock (fields)
            {
                if (!fields.ContainsKey(type.FullName))
                {
                    fields.Add(type.FullName, new FieldInfo(type));
                }
                return fields[type.FullName];
            }
        }

    }
}
