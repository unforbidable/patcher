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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Patcher.Data.Plugins.Content
{
    sealed class FieldInfo
    {
        readonly Func<Field> creator;

        public FieldInfo(Type type)
        {
            if (!typeof(Field).IsAssignableFrom(type))
            {
                throw new ArgumentException("Field type is not derived from Field: " + type.FullName);
            }

            creator = Expression.Lambda<Func<Field>>(Expression.New(type.GetConstructor(Type.EmptyTypes))).Compile();
        }

        public Field CreateInstance()
        {
            return creator.Invoke();          
        }
    }
}
