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
    sealed class CompoundInfo
    {
        public Type Type { get; private set; }

        readonly IDictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
        public IDictionary<string, MemberInfo> Members { get { return members; } }

        readonly ISet<string> fieldNames = new HashSet<string>();
        public ISet<string> FieldNames { get { return fieldNames; } }

        public CompoundInfo(Type type)
        {
            Type = type;

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                MemberAttribute memberAttribute = prop.GetCustomAttributes(typeof(MemberAttribute), true).Cast<MemberAttribute>().FirstOrDefault();// .NET 4.5: prop.GetCustomAttribute<MemberAttribute>(true);
                if (memberAttribute != null)
                {
                    var meminfo = new MemberInfo(memberAttribute, prop);
                    foreach (string name in memberAttribute.FieldNames)
                    {
                        if (fieldNames.Contains(name))
                            throw new InvalidProgramException("Duplicate field name: " + name);

                        fieldNames.Add(name);
                        members.Add(name, meminfo);
                    }
                }
            }
        }

        public void Copy(object from, object to)
        {
            foreach (var meminfo in members.Values.Distinct())
            {
                object value = meminfo.CopyValue(from);
                if (meminfo.IsListType)
                    meminfo.SetList(to, value);
                else
                    meminfo.SetValue(to, value);
            }
        }

        public IEnumerable<uint> GetReferencedFormIds(object target)
        {
            foreach (var meminfo in members.Values.Distinct())
            {
                foreach (var formId in meminfo.GetReferencedFormIds(target))
                    yield return formId;
            }
        }

        public bool Equate(object one, object other)
        {
            foreach (var meminfo in members.Values.Distinct())
            {
                if (!meminfo.Equate(one, other))
                    return false;
            }
            return true;
        }
    }
}
