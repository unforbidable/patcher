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

            // Meta data class
            ns.Types.Add(GetMetaDataClass(models));

            // Prepare a namespace for each game model
            foreach (var model in models)
            {
                code.Namespaces.Add(GetModelNamespace(model));
            }

            return code;
        }

        private CodeClass GetMetaDataClass(IEnumerable<GameModel> models)
        {
            var cls = new CodeClass("ModelMetaData");
            cls.IsStatic = true;
            cls.Comment = "Model meta data";

            cls.Members.Add(new CodeField("string", "ProgramVersion")
            {
                Value = Program.GetProgramFullVersionInfo(),
                IsPublic = true,
                IsStatic = true
            });

            string serialized = new ModelSerializer().SerializeModel(models);
            string serializedHash = ModelHelper.GetModelHash(serialized);

            cls.Members.Add(new CodeField("string", "ModelData")
            {
                Value = serialized,
                IsPublic = true,
                IsStatic = true
            });

            cls.Members.Add(new CodeField("string", "ModelHash")
            {
                Value = serializedHash,
                IsPublic = true,
                IsStatic = true
            });

            return cls;
        }

        private CodeNamespace GetModelNamespace(GameModel model)
        {
            string nsName = string.Format("Patcher.Data.Models.{0}", model.Name.Replace(" ", string.Empty));
            var ns = new CodeNamespace(nsName);
            ns.Comment = string.Format("Data model for {0}", model.Name);

            return ns;
        }
    }
}
