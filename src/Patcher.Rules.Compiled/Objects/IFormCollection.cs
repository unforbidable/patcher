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

namespace Patcher.Rules.Compiled.Objects
{
    public interface IFormCollection<out TForm> : IEnumerable<TForm> where TForm : IForm
    {
        int Count { get; }
        void Add(IForm form);
        void Remove(IForm form);
        bool Contains(IForm form);
        void Add(string editorId);
        void Remove(string editorId);
        bool Contains(string editorId);
        IFormCollection<TOther> Of<TOther>() where TOther : IForm;
        IFormCollection<TForm> Where(Predicate<TForm> predicate);
        void TagAll(string text);
        IFormCollection<TForm> HavingTag(string text);

        // TODO: Following methods are implemented but are not consistent with Forms.Find
        // as they lack an overload that can be used to specify the plugin name
        // For now Forms.Find() should be used but generally looking up forms by Editor ID should be preferred
        //void Add(uint formId); 
        //void Remove(uint formId);
        //bool Contains(uint formId);
    }
}
