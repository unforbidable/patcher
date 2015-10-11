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

using Patcher.Data.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Patcher.Rules.Methods
{
    /// <summary>
    /// Describes a rule method that is used to create a new form.
    /// </summary>
    class InsertMethod
    {
        /// <summary>
        /// Gets or sets the type of the new form.
        /// </summary>
        public FormKind InsertedFormKind { get; set; }

        /// <summary>
        /// Gets or sets the value that indicates whether compatible form content is copied from the original form when a new form is created.
        /// </summary>
        public bool Copy { get; set; }

        public string Description { get; set; }
        public Func<object, object, bool> Method { get; set; }
    }
}
