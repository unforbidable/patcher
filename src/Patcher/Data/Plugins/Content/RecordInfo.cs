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

namespace Patcher.Data.Plugins.Content
{
    sealed class RecordInfo
    {
        readonly RecordAttribute attribute;
        public RecordAttribute Attribute { get { return attribute; } }

        readonly CompoundInfo compound;
        readonly ConstructorInfo ctor;

        static Type[] paramlessTypes = new Type[] { };
        static object[] paramlessArgs = new object[] { };

        public RecordInfo(Type type)
        {
            if (!typeof(Record).IsAssignableFrom(type))
            {
                throw new ArgumentException("Record type is not derived from Record: " + type.FullName);
            }

            attribute = type.GetCustomAttributes(typeof(RecordAttribute), false).Cast<RecordAttribute>().FirstOrDefault();// .NET 4.5: type.GetCustomAttribute<RecordAttribute>();
            if (attribute == null)
            {
                throw new ArgumentException("Record type is missing required atribute: " + type.FullName);
            }

            ctor = type.GetConstructor(paramlessTypes);
            compound = InfoProvider.GetCompoundInfo(type);
        }

        public GenericFormRecord CreateInstance()
        {
            return (GenericFormRecord)ctor.Invoke(paramlessArgs);
        }

    }
}
