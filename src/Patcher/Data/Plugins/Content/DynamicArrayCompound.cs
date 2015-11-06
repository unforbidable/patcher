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
using System.Text;

namespace Patcher.Data.Plugins.Content
{
    public abstract class DynamicArrayCompound<T> : Compound where T : class
    {
        List<T> elements = new List<T>();

        public int Length { get { return elements.Count; } }

        internal override void ReadCompoundField(RecordReader reader, string fieldName, int depth)
        {
            int index = FieldNameToIndex(fieldName);

            // Ensure list size
            if (index >= elements.Count)
                elements.AddRange(new T[index - elements.Count + 1]);

            var type = typeof(T);
            if (type == typeof(string))
            {
                elements[index] = (T)(object)reader.ReadStringZeroTerminated();
            }
            else
            {
                throw new InvalidOperationException("Unsupported Dynamic Array Compund element type: " + type.FullName);
            }
        }

        protected abstract int FieldNameToIndex(string fieldName);
        protected abstract string IndexToFieldName(int index);

        public override string ToString()
        {
            return string.Format("Length={0}", Length);
        }
    }
}
