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

using Patcher.Data.Models.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization
{
    public class ModelSerializationWriter
    {
        readonly JsonSerializationWriter writer;

        public ModelSerializationWriter(Stream stream, bool pretty)
        {
            writer = new JsonSerializationWriter(stream, pretty);
        }

        public void WriteModels(IEnumerable<GameModel> models)
        {
            writer.WriteModels(models, WriteModel);
        }

        private void WriteModel(GameModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("BasePlugin", model.BasePlugin);
            writer.WriteProperty("LatestFormVersion", model.LatestFormVersion);
            writer.WriteProperty("PluginsFileLocation", model.PluginsFileLocation);
            writer.WriteProperty("PluginsMatchLine", model.PluginsMatchLine);
            writer.WriteProperty("ArchivesExtension", model.ArchivesExtension);
            writer.WriteProperty("StringsDefaultLanguage", model.StringsDefaultLanguage);
            writer.WriteProperty("Models", model.Models, WriteRootModel);
        }

        private void WriteRootModel(IModel model)
        {
            if (model is EnumModel)
            {
                WriteModel(model as EnumModel);
            }
            else if (model is StructModel)
            {
                WriteModel(model as StructModel);
            }
            else if (model is RecordModel)
            {
                WriteModel(model as RecordModel);
            }
            else if (model is FieldGroupModel)
            {
                WriteModel(model as FieldGroupModel);
            }
            else if (model is FunctionModel)
            {
                WriteModel(model as FunctionModel);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unexpected root model {0}", model.GetType().Name));
            }
        }

        private void WriteModel(EnumModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("DisplayName", model.DisplayName);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("BaseType", model.BaseType); // Write only .Name for types
            writer.WriteProperty("IsFlags", model.IsFlags);
            writer.WriteProperty("Members", model.Members, WriteModel); // Array property - pass in the method used to write the model in the array
        }

        private void WriteModel(EnumMemberModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("Value", model.Value);
        }

        private void WriteModel(MemberModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("DisplayName", model.DisplayName);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("TargetModel", model.TargetModel, WriteModel);
            writer.WriteProperty("IsHidden", model.IsHidden);
            writer.WriteProperty("IsVirtual", model.IsVirtual);
            writer.WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                writer.WriteProperty("ArrayLength", model.ArrayLength);
                writer.WriteProperty("ArrayPrefixSize", model.ArrayPrefixSize);
            }

            if (model.IsStruct)
            {
                writer.WriteProperty("Type", model.Struct, WriteModel);
            }
            else if (model.IsMemberType)
            {
                writer.WriteProperty("Type", model.MemberType, WriteModel);
            }
        }

        private void WriteModel(FunctionModel model)
        {
            writer.WriteProperty("Index", model.Index);
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("Params", model.Params, WriteModel);
        }

        private void WriteModel(FunctionParamModel model)
        {
            writer.WriteProperty("DisplayName", model.DisplayName);
            if (model.IsFunctionParamType)
            {
                writer.WriteProperty("Type", model.FunctionParamType, WriteModel);
            }
            else if (model.IsEnumeration)
            {
                writer.WriteProperty("Type", model.Enumeration);
            }
            else if (model.IsFormReference)
            {
                writer.WriteProperty("Type", model.FormReference, WriteModel);
            }
        }

        private void WriteModel(StructModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("IsUnion", model.IsUnion);
            writer.WriteProperty("Members", model.Members, WriteModel);
        }

        private void WriteModel(FieldModel model)
        {
            writer.WriteProperty("Key", model.Key);
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("DisplayName", model.DisplayName);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("TargetModel", model.TargetModel, WriteModel);
            writer.WriteProperty("IsHidden", model.IsHidden);
            writer.WriteProperty("IsVirtual", model.IsVirtual);
            writer.WriteProperty("IsList", model.IsList);
            writer.WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                writer.WriteProperty("ArrayLength", model.ArrayLength);
            }

            if (model.IsStruct)
            {
                writer.WriteProperty("Type", model.Struct);
            }
            else if (model.IsFieldGroup)
            {
                writer.WriteProperty("Type", model.FieldGroup);
            }
            else if (model.IsMember)
            {
                writer.WriteProperty("Type", model.MemberType, WriteModel);
            }
        }

        private void WriteModel(FieldGroupModel model)
        {
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("Fields", model.Fields, WriteModel);
        }

        private void WriteModel(RecordModel model)
        {
            writer.WriteProperty("Key", model.Key);
            writer.WriteProperty("Name", model.Name);
            writer.WriteProperty("DisplayName", model.DisplayName);
            writer.WriteProperty("Description", model.Description);
            writer.WriteProperty("Fields", model.Fields, WriteModel); // Array property - pass in the method used to write the model in the array
        }

        private void WriteModel(TargetModel model)
        {
            // At most one of those will not be null - so at most one Type property will be written
            writer.WriteProperty("Type", model.Type as TargetType, WriteModel);
            writer.WriteProperty("Type", model.Type as FormReference, WriteModel);
            writer.WriteProperty("Type", model.Type as StructModel);
            writer.WriteProperty("IsArray", model.IsArray);
            if (model.IsArray)
            {
                writer.WriteProperty("ArrayLength", model.ArrayLength);
            }
        }

        private void WriteModel(ISerializableAsId model)
        {
            writer.WriteProperty("Id", model.Id);
        }
    }
}
