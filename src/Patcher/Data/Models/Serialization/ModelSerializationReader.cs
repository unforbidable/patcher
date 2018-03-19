// Copyright(C) 2015,2016,2017,2018 Unforbidable Works
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

using Patcher.Data.Models.Loading;
using Patcher.Data.Models.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization
{
    public class ModelSerializationReader
    {
        readonly JsonSerializationReader reader;

        public ModelSerializationReader(Stream stream)
        {
            reader = new JsonSerializationReader(stream);
        }

        public IEnumerable<GameModel> ReadGameModels()
        {
            return reader.ReadModels(ReadGameModel);
        }

        private GameModel ReadGameModel()
        {
            string name = reader.ReadPropertyString("Name");
            string basePlugin = reader.ReadPropertyString("BasePlugin");
            int latestFormVersion = reader.ReadPropertyInt32("LatestFormVersion");
            string pluginsFileLocation = reader.ReadPropertyString("PluginsFileLocation");
            string pluginsMatchLine = reader.ReadPropertyString("PluginsMatchLine");
            string archivesExtension = reader.ReadPropertyString("ArchivesExtension");
            string stringsDefaultLanguage = reader.ReadPropertyString("StringsDefaultLanguage");
            var models = reader.ReadPropertyArray("Models", ReadRootModel);

            return new GameModel(name, basePlugin, latestFormVersion, pluginsFileLocation, pluginsMatchLine, archivesExtension, stringsDefaultLanguage, models);
        }

        private IModel ReadRootModel()
        {
            switch (reader.CurrentObjectTypeName)
            {
                case "EnumModel":
                    return ReadEnumModel();

                case "StructModel":
                    return ReadStructModel();

                    // TODO: Add all root models: RecordModel, FunctionModel, FieldGroup

                default:
                    //throw new NotImplementedException(string.Format("Reading model objects of type '{0}' has not been implemented.", reader.CurrentObjectTypeName));
                    return null;
            }
        }

        private StructModel ReadStructModel()
        {
            string name = reader.ReadPropertyString("Name");
            string description = reader.ReadPropertyString("Description");
            var members = reader.ReadPropertyArray("Members", ReadMemberModel);

            return new StructModel(name, description, members);
        }

        private MemberModel ReadMemberModel()
        {
            string name = reader.ReadPropertyString("Name");
            string displayName = reader.ReadPropertyString("DisplayName");
            string description = reader.ReadPropertyString("Description");
            var type = reader.ReadPropertyObject("Type", ReadMemberType);
            var targetModel = reader.ReadPropertyObject("TargetModel", ReadTargetModel);
            bool isHidden = reader.ReadPropertyBoolean("IsHidden");
            bool isVirtual = reader.ReadPropertyBoolean("IsVirtual");
            bool isArray = reader.ReadPropertyBoolean("IsArray");
            int arrayLength = reader.ReadPropertyInt32("ArrayLength");
            int arrayPrefixSize = reader.ReadPropertyInt32("ArrayPrefixSize");

            return new MemberModel(name, displayName, description, type, targetModel, isHidden, isVirtual, isArray, arrayLength, arrayPrefixSize);
        }

        private ICanRepresentMember ReadMemberType()
        {
            switch (reader.CurrentObjectTypeName)
            {
                case "StructModel":
                    return ReadStructModel();

                case "MemberType":
                    // Get member type by ID
                    return MemberType.GetKnownType(reader.ReadPropertyString("Id"));

                default:
                    throw new NotImplementedException(string.Format("Reading member type objects of type '{0}' has not been implemented.", reader.CurrentObjectTypeName));
            }
        }

        private TargetModel ReadTargetModel()
        {
            var type = reader.ReadPropertyObject("Type", ReadTargetType);
            bool isArray = reader.ReadPropertyBoolean("IsArray");
            int arrayLength = reader.ReadPropertyInt32("ArrayLength");

            return new TargetModel(type, isArray, arrayLength);
        }

        private ICanRepresentTarget ReadTargetType()
        {
            switch (reader.CurrentObjectTypeName)
            {
                case "TargetType":
                    // Get target type by ID
                    return TargetType.GetKnownType(reader.ReadPropertyString("Id"));

                case "FormReference":
                    // Get reference by ID
                    return FormReference.Parse(reader.ReadPropertyString("Id"));

                default:
                    throw new NotImplementedException(string.Format("Reading target type objects of type '{0}' has not been implemented.", reader.CurrentObjectTypeName));
            }
        }

        private EnumModel ReadEnumModel()
        {
            string name = reader.ReadPropertyString("Name");
            string description = reader.ReadPropertyString("Description");
            string baseType = reader.ReadPropertyString("BaseType");
            Type resolvedBaseType = GetType().Assembly.GetType(baseType);
            bool isFlags = reader.ReadPropertyBoolean("IsFlags");
            var members = reader.ReadPropertyArray("Members", ReadEnumMemberModel);

            return new EnumModel(name, description, resolvedBaseType, isFlags, members);
        }

        private EnumMemberModel ReadEnumMemberModel()
        {
            string name = reader.ReadPropertyString("Name");
            string displayName = reader.ReadPropertyString("DisplayName");
            string description = reader.ReadPropertyString("Description");
            string value = reader.ReadPropertyString("Value");

            return new EnumMemberModel(name, displayName, description, value);
        }
    }
}
