/// Copyright(C) 2018 Unforbidable Works
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Patcher.Data.Models.Loading
{
    public sealed class ModelLoadingException : Exception
    {
        public XElement Element { get; private set; }

        public ModelLoadingException(string message)
            : base(message)
        {
        }

        public ModelLoadingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ModelLoadingException(string message, XElement element)
            : base(message)
        {
            Element = element;
        }

        public ModelLoadingException(string message, XElement element, Exception innerException)
            : base(message, innerException)
        {
            Element = element;
        }
    }
}
