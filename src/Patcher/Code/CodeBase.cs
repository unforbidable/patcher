// Copyright(C) 2018 Unforbidable Works
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

using Patcher.Code.Building;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Patcher.Code
{
    /// <summary>
    /// Represents the whole code base.
    /// </summary>
    public sealed class CodeBase
    {
        /// <summary>
        /// Gets the collection of namespace names to be built into using statements.
        /// </summary>
        public ICollection<string> Using { get; private set; }

        /// <summary>
        /// Gets the collection of all namespaces in this code base.
        /// </summary>
        public CodeNamespaceCollection Namespaces { get; private set; }

        public CodeBase()
        {
            Using = new Collection<string>();
            Namespaces = new CodeNamespaceCollection(this);
        }

        public string BuildCode(bool singleFile)
        {
            var builder = new CodeBuilder();
            builder.Options.SingleFile = singleFile;

            if (Using.Count > 0)
            {
                foreach (var u in Using)
                {
                    builder.AppendLine("using {0};", u);
                }
                builder.AppendLine();
            }

            foreach (var ns in Namespaces)
            {
                ns.BuildCode(builder);
            }

            return builder.ToString();
        }
    }
}
