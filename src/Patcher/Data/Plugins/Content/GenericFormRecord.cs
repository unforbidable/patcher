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
using System.Threading.Tasks;

namespace Patcher.Data.Plugins.Content
{
    public abstract class GenericFormRecord : Record, IEquatable<GenericFormRecord>
    {
        [Member(Names.EDID)]
        [Order(-1)]
        [Lazy]
        public string EditorId
        {
            get { return editorId; }
            set
            {
                string old = editorId;
                if (RaiseEditorIdChangingEvent(old, value))
                {
                    editorId = value;
                    RaiseEditorIdChangedEvent(old);
                }
            }
        }
        private string editorId;

        internal event Action<string> EditorIdChanged;

        internal bool IsRecordCompressed { get { return HasFlag(RecordFlags.Compressed); } }

        public override string ToString()
        {
            return string.Format("Editor ID=\"{0}\"", editorId);
        }

        public GenericFormRecord CopyRecord()
        {
            var recinfo = InfoProvider.GetRecordInfo(GetType());
            var other = recinfo.CreateInstance();

            BeforeCopy(other);

            // Copy raw flags and version
            other.RawFlags = RawFlags;
            other.Version = Version;

            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            compinfo.Copy(this, other);

            AfterCopy(other);

            return other;
        }

        public bool Equals(GenericFormRecord other)
        {
            // Compare raw flags
            if (RawFlags != other.RawFlags)
                return false;

            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            return compinfo.Equate(this, other);
        }

        public IEnumerable<uint> GetReferencedFormIds()
        {
            var compinfo = InfoProvider.GetCompoundInfo(GetType());
            return compinfo.GetReferencedFormIds(this);
        }

        protected virtual void BeforeCopy(GenericFormRecord copy)
        {
        }

        protected virtual void AfterCopy(GenericFormRecord copy)
        {
        }

        protected virtual bool OnEditorIdChanging(string previousEditorId, string newEditorId)
        {
            return true;
        }

        protected virtual void OnEditorIdChanged(string previousEditorId)
        {
        }

        bool RaiseEditorIdChangingEvent(string previousEditorId, string newEditorId)
        {
            return OnEditorIdChanging(previousEditorId, newEditorId);
        }

        void RaiseEditorIdChangedEvent(string previousEditorId)
        {
            OnEditorIdChanged(previousEditorId);

            var handler = EditorIdChanged;
            if (handler != null)
            {
                handler(previousEditorId);
            }
        }

    }
}
