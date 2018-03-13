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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Patcher.Data.Models.Loading
{
    /// <summary>
    /// Handles the reading of parts of model files into model objects.
    /// </summary>
    public class ModelReader
    {
        /// <summary>
        /// Gets the file this ModelReader instance is reading.
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Gets the XML element this ModelReader instance is currently at.
        /// </summary>
        public XElement Element { get; private set; }

        /// <summary>
        /// Gets the ModelResolvel that is used to resolve model objects.
        /// </summary>
        public ModelResolver Resolver { get; private set; }

        public ModelReader(string path, XElement element, ModelResolver resolver)
        {
            File = path;
            Element = element;
            Resolver = resolver;
        }
        
        /// <summary>
        /// Creates a copy of the ModelReader that is targeted at another element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private ModelReader EnterElement(XElement element)
        {
            return new ModelReader(File, element, Resolver);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", File, Element.GetAbsoluteXPath());
        }

        public GameModel ReadGameModel(IEnumerable<string> files)
        {
            // TODO: Ensure element name 'game'

            string name = ReadValue("name");
            string basePlugin = ReadValue("base-plugin");
            int latestFormVersion = ReadInt("latest-form-version");
            string pluginsFileLocation = ReadGrandChildValue("plugins", "file-location");
            string pluginsMatchLine = ReadGrandChildValue("plugins", "match-line");
            string archivesExtension = ReadGrandChildValue("archives", "extension");
            string stringsDefaultLanguage = ReadGrandChildValue("strings", "default-language");

            if (pluginsFileLocation != null)
            {
                // Replace placeholders in the path to the plugin file
                pluginsFileLocation = pluginsFileLocation.Replace("{LOCALAPPDATA}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                pluginsFileLocation = pluginsFileLocation.Replace("{APPDATA}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                pluginsFileLocation = pluginsFileLocation.Replace("{MYDOCUMENTS}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                pluginsFileLocation = pluginsFileLocation.Replace("{NAME}", name ?? string.Empty);
            }

            var loader = new ModelLoader();

            return new GameModel(name, basePlugin, latestFormVersion, pluginsFileLocation, pluginsMatchLine, archivesExtension, stringsDefaultLanguage, loader.LoadFiles(files));
        }

        public IModel ReadDocumentRootModel(string id)
        {
            switch (Element.Name.LocalName)
            {
                case "record":
                    Log.Fine("Loading record model {0}", id);
                    return ReadRecord(id);

                case "enum":
                    Log.Fine("Loading enum model {0}", id);
                    return ReadEnum();

                case "struct":
                    Log.Fine("Loading struct model {0}", id);
                    return ReadStruct();

                case "field":
                    Log.Fine("Loading field model {0}", id);
                    return ReadField();

                default:
                    throw new ModelLoadingException(string.Format("Unexpected document root element '{0}'", Element.Name), Element);
            }
        }

        public EnumModel ReadEnum()
        {
            // TODO: Ensure element name 'enum'

            string name = ReadValue("name");
            string description = ReadValue("description");
            Type baseType = ReadEnumType("type") ?? typeof(int);
            bool isFlags = HasValueTrue("flags");
            var members = GetGrandChildren("members").Select(e => EnterElement(e).ReadEnumMember(baseType, isFlags));

            return new EnumModel(name, description, baseType, isFlags, members);
        }

        public EnumMemberModel ReadEnumMember(Type type, bool isFlags)
        {
            // TODO: Ensure element name 'member'

            string name = ReadValue("name");
            string description = ReadValue("description");
            string value = ReadValue("value");
            value = ModelLoadingHelper.ReparseValue(value, type, isFlags ? ReparseOptions.Hexadecimal | ReparseOptions.LeadingZeros : ReparseOptions.None);

            return new EnumMemberModel(name, description, value);
        }

        public StructModel ReadStruct()
        {
            // TODO: Ensure element name 'struct'

            string name = ReadValue("name");
            string description = ReadValue("description");
            var members = GetGrandChildren("members").Select(e => EnterElement(e).ReadMember());

            return new StructModel(name, description, members);
        }

        public FieldGroupModel ReadFieldGroup()
        {
            // TODO: Ensure element name 'group'

            string name = ReadValue("name");
            string description = ReadValue("description");
            var fields = GetGrandChildren("fields").Select(e => EnterElement(e).ReadField());

            return new FieldGroupModel(name, description, fields);
        }

        public RecordModel ReadRecord(string id)
        {
            // TODO: Ensure element name 'record'

            string formType = id.ToUpper();
            string name = ReadValue("name");
            string displayName = ReadValue("display-name");
            string description = ReadValue("description");
            var fields = GetGrandChildren("fields").Select(e => EnterElement(e).ReadField());

            return new RecordModel(formType, displayName, name, description, fields);
        }

        public IEnumerable<FunctionModel> ReadFunctions()
        {
            List<FunctionModel> functions = new List<FunctionModel>();
            foreach (var functionElement in Element.Elements("function"))
            {
                functions.Add(EnterElement(functionElement).ReadFunction());
            }
            return functions;
        }

        private FunctionModel ReadFunction()
        {
            short index = ReadShort("index");
            string name = ReadValue("name");
            string description = ReadValue("description");

            List<FunctionParamModel> parameters = new List<FunctionParamModel>();
            foreach (var paramElement in GetGrandChildren("params"))
            {
                parameters.Add(EnterElement(paramElement).ReadFunctionParam());
            }

            return new FunctionModel(index, name, description, parameters);
        }

        private FunctionParamModel ReadFunctionParam()
        {
            string type = ReadValue("type");
            if (!string.IsNullOrEmpty(type))
            {
                FunctionParamType functionMemberType;
                if (FunctionParamType.TryFindKnownTargetType(type, out functionMemberType))
                {
                    // Param is a primitive type
                    return new FunctionParamModel(functionMemberType);
                }

                FormReference formReference;
                if (FormReference.TryParse(type, out formReference))
                {
                    // Param is a form reference
                    return new FunctionParamModel(formReference);
                }

                // Param needs to be resolved (as an enum)
                var model = new FunctionParamModel(null);
                Resolver.MarkModelForResolution(model, type, File, Element);
                return model;
            }
            else
            {
                return new FunctionParamModel(null);
                //throw new ModelLoadingException("Function parameter specification is incomplete, 'type' expected.", Element);
            }
        }

        private TargetModel ReadTarget()
        {
            var targetElement = Element.Element("as");
            var targetTypeAttribute = Element.Attribute("as-type");
            var targetElementReader = targetElement != null ? EnterElement(targetElement) : null;
            var targetStructElement = targetElement != null ? targetElement.Element("struct") : null;

            // TODO: Read any target attributes

            string type = targetElementReader != null ? targetElementReader.ReadChildValue("type") : targetTypeAttribute != null ? targetTypeAttribute.Value : null; 
            if (!string.IsNullOrEmpty(type))
            {
                var id = TypeIdentifier.FromString(type);

                // Try to find and set appropriate member type
                TargetType targetType;
                if (TargetType.TryFindKnownTargetType(id.Identifier, out targetType))
                {
                    return new TargetModel(targetType, id.IsArray, id.ArrayLength);
                }

                FormReference formReference;
                if (FormReference.TryParse(id.Identifier, out formReference))
                {
                    return new TargetModel(formReference, id.IsArray, id.ArrayLength);
                }

                Log.Fine("Member type '{0}' is not recornized and needs to be resolved.", id.Identifier);
                var targetModel = new TargetModel(null, id.IsArray, id.ArrayLength);
                Resolver.MarkModelForResolution(targetModel, id.Identifier, File, Element);
                return targetModel;
            }
            else if (targetStructElement != null)
            {
                var structModel = EnterElement(targetStructElement).ReadStruct();
                return new TargetModel(structModel, false, 0);
            }
            else
            {
                // No target specification
                return null;
            }            
        }

        public MemberModel ReadMember()
        {
            string name = ReadValue("name");
            string description = ReadValue("description");
            bool isVirtual = HasValueTrue("virtual");
            bool isHidden = HasValueTrue("hidden");
            bool isArray = false;
            int arrayLength = 0;
            MemberType memberType = null;

            // Read target model (as)
            TargetModel targetModel = ReadTarget();

            // Read type ID element name if nor 'member' else child element 'type'
            string type = Element.Name != "member" ? Element.Name.LocalName : ReadChildValue("type");
            if (type != null)
            {
                var id = TypeIdentifier.FromString(type);
                isArray = id.IsArray;
                arrayLength = id.ArrayLength;

                // Try to find and set appropriate member type
                if (!MemberType.TryFindKnownMemberType(id.Identifier, out memberType))
                {
                    Log.Fine("Member type '{0}' is not recornized and needs to be resolved.", id.Identifier);
                    var memberModel = new MemberModel(name, description, null, targetModel, isHidden, isVirtual, isArray, arrayLength);
                    Resolver.MarkModelForResolution(memberModel, id.Identifier, File, Element);
                    return memberModel;
                }
            }
            else
            {
                // member type is unspecified
            }

            return new MemberModel(name, description, memberType, targetModel, isHidden, isVirtual, isArray, arrayLength);
        }

        public FieldModel ReadField()
        {
            string key = ReadValue("key");
            string name = ReadValue("name");
            string description = ReadValue("description");
            bool isHidden = HasValueTrue("hidden");
            bool isVirtual = HasValueTrue("virtual");
            bool isList = HasValueTrue("list");
            bool isArray = false;
            int arrayLength = 0;

            // Read target model (as)
            TargetModel targetModel = ReadTarget();

            var structElement = Element.Element("struct");
            var groupElement = Element.Element("group");

            // Read type ID element name if nor 'field' else child element 'type'
            string type = Element.Name != "field" ? Element.Name.LocalName : ReadChildValue("type");
            if (type != null)
            {
                var id = TypeIdentifier.FromString(type);
                isArray = id.IsArray;
                arrayLength = id.ArrayLength;

                // Try to find and set appropriate member type
                MemberType memberType;
                bool found = MemberType.TryFindKnownMemberType(id.Identifier, out memberType);

                var fieldModel = new FieldModel(key, name, description, memberType, targetModel, isHidden, isVirtual, isList, isArray, arrayLength);

                if (!found)
                {
                    Log.Fine("Field type '{0}' is not recornized and needs to be resolved.", id.Identifier);
                    Resolver.MarkModelForResolution(fieldModel, id.Identifier, File, Element);
                }

                return fieldModel;
            }
            else if (structElement != null)
            {
                // Field is a structure
                var structModel = EnterElement(structElement).ReadStruct();
                return new FieldModel(key, name, description, structModel, targetModel, isHidden, isVirtual, isList, isArray, arrayLength);                                
            }
            else if (groupElement != null)
            {
                // Field is a field group 
                var groupModel = EnterElement(groupElement).ReadFieldGroup();
                return new FieldModel(key, name, description, groupModel, targetModel, isHidden, isVirtual, isList, isArray, arrayLength);
            }
            else
            {
                throw new ModelLoadingException("Field specification is incomplete, one of 'group', 'struct' or 'type' expected.", Element);
            }
        }

        private IEnumerable<XElement> GetGrandChildren(string name)
        {
            return Element.Elements(name).SelectMany(e => e.Elements());
        }

        private int ReadInt(string name)
        {
            string value = ReadValue(name);
            return int.Parse(value);
        }

        private short ReadShort(string name)
        {
            string value = ReadValue(name);
            return short.Parse(value);
        }

        private string ReadValue(string name)
        {
            // Read value from child element or attribute
            return ReadChildValue(name) ?? ReadAttributeValue(name);
        }

        private string ReadChildValue(string name)
        {
            return Element.Elements(name).Select(e => e.Value).SingleOrDefault();
        }

        private string ReadGrandChildValue(string childName, string grandChildName)
        {
            return Element.Elements(childName).SelectMany(e => e.Elements(grandChildName).Select(f => f.Value)).SingleOrDefault();
        }

        private string ReadAttributeValue(string name)
        {
            return Element.Attributes().Where(a => a.Name == name).Select(a => a.Value).SingleOrDefault();
        }

        private Type ReadEnumType(string name)
        {
            string value = ReadValue(name);
            return value != null ? ModelLoadingHelper.ParseEnumType(value) : null;
        }

        private bool HasValueTrue(string name)
        {
            string value = ReadValue(name);
            return value != null && value.ToLower() == "true";
        }

    }
}
