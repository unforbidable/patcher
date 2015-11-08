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

        public T this[int index] { get { return index >= Length ? null : elements[index]; } set { EnsureLength(index + 1); elements[index] = value; } }

        internal override void ReadCompoundField(RecordReader reader, string fieldName, int depth)
        {
            int index = FieldNameToIndex(fieldName);
            EnsureLength(index + 1);
            elements[index] = reader.ReadField<T>();
        }

        internal override void WriteField(RecordWriter writer)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                string fieldName = IndexToFieldName(i);
                writer.WriteField(elements[i], fieldName);
            }
        }

        void EnsureLength(int length)
        {
            // Ensure list size
            if (length >= elements.Count)
                elements.AddRange(new T[length - elements.Count]);
        }

        protected abstract int FieldNameToIndex(string fieldName);
        protected abstract string IndexToFieldName(int index);

        public override Field CopyField()
        {
            var instance = (DynamicArrayCompound<T>)Activator.CreateInstance(GetType());
            instance.elements = new List<T>(elements);
            return instance;
        }

        public override bool Equals(Field other)
        {
            var cast = (DynamicArrayCompound<T>)other;
            if (elements.Count != cast.elements.Count)
                return false;
            return elements.SequenceEqual(cast.elements);
        }

        public override IEnumerable<uint> GetReferencedFormIds()
        {
            yield break;
        }

        public override string ToString()
        {
            return string.Format("Length={0}", Length);
        }
    }
}
