// Copyright(C) 2015,1016,2017,2018 Unforbidable Works
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

using Patcher.Code;
using Patcher.Data.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Code
{
    public class ModelCodeBuilder
    {
        public CodeBase BuildModels(IEnumerable<GameModel> models)
        {
            var code = new CodeBase();
            code.Using.Add("System");

            // General namespace
            var ns = new CodeNamespace("Patcher.Data.Models");
            code.Namespaces.Add(ns);

            // Prepare a namespace for each game model
            foreach (var model in models)
            {
                code.Namespaces.Add(GetGameNamespace(model));
            }

            return code;
        }

        private CodeNamespace GetGameNamespace(GameModel model)
        {
            string nsName = string.Format("Patcher.Data.Models.{0}", model.Name);
            var ns = new CodeNamespace(nsName)
            {
                Comment = string.Format("Data model for {0}", model.Name)
            };

            foreach (var e in model.Models.OfType<EnumModel>())
            {
                ns.Types.Add(GetEnum(e));
            }

            return ns;
        }

        private CodeEnum GetEnum(EnumModel model)
        {
            var comment = new StringBuilder();
            if (model.IsFlags)
            {
                comment.AppendLine("[Flags]");
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            var e = new CodeEnum(model.Name)
            {
                Comment = comment.ToString(),
                Type = model.BaseType
            };

            foreach (var m in model.Members)
            {
                e.Members.Add(GetEnumMember(m));
            }

            return e;
        }

        private CodeEnumMember GetEnumMember(EnumMemberModel model)
        {
            var comment = new StringBuilder();
            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                comment.AppendLine(string.Format("[DisplayName(\"{0}\")]", model.DisplayName));
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                comment.AppendLine(string.Format("[Description(\"{0}\")]", model.Description));
            }

            return new CodeEnumMember(model.Name, model.Value)
            {
                Comment = comment.ToString()
            };
        }
    }
}
