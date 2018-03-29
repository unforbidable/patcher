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

namespace Patcher.Data.Models.Code.Generated
{
    [Obsolete("Use generated version of the Variable class")]
    public abstract class Variable
    {
        public int Index { get; private set; }
        public Type Type { get; private set; }
        
        protected Variable(int index, Type type)
        {
            Index = index;
            Type = type;
        }

        public object GetValue(int index)
        {
            if (index != Index)
            {
                throw new InvalidOperationException(string.Format("The current value index is {0} and value index {1} cannot be retrieved.", Index, index));
            }
            else
            {
                return DoGetValue(index);
            }
        }

        public T As<T>()
        {
            EnsureTypeMatch(typeof(T));
            return (T)GetValue(Index);
        }

        public T ConvertTo<T>()
        {
            return (T)Convert.ChangeType(GetValue(Index), typeof(T));
        }

        protected virtual object DoGetValue(int index)
        {
            throw new NotImplementedException(string.Format("Getting value index {0} not implemented", index));
        }

        protected void EnsureTypeMatch(Type assignedType)
        {
            if (!assignedType.IsAssignableFrom(Type))
                throw new InvalidOperationException(string.Format("The current value type {0} cannot be assigned to type {1}.", Type.FullName, assignedType.FullName));
        }

        public override string ToString()
        {
            return GetValue(Index).ToString();
        }
    }

    [Obsolete("Use generated version of the Variable class")]
    public class Variable<T1, T2> : Variable
    {
        private T1 value1;

        private Variable(T1 value) : base(0, typeof(T1))
        {
            value1 = value;
        }

        public static implicit operator T1(Variable<T1, T2> variable)
        {
            variable.EnsureTypeMatch(typeof(T1));
            return variable.value1;
        }

        public static implicit operator Variable<T1, T2>(T1 value)
        {
            return new Variable<T1, T2>(value);
        }

        private T2 value2;

        public Variable(T2 value) : base(1, typeof(T2))
        {
            value2 = value;
        }

        public static implicit operator T2(Variable<T1, T2> variable)
        {
            variable.EnsureTypeMatch(typeof(T2));
            return variable.value2;
        }

        public static implicit operator Variable<T1, T2>(T2 value)
        {
            return new Variable<T1, T2>(value);
        }

        protected override object DoGetValue(int index)
        {
            switch (index)
            {
                case 0: return value1;
                case 1: return value2;
            }

            return base.DoGetValue(index);
        }
    }
}
