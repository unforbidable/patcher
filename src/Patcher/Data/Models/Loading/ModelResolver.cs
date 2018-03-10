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
    /// Handles the resolution of partially specified object models.
    /// </summary>
    public class ModelResolver
    {
        public ModelLoader Loader { get; private set; }

        List<UnresolvedModel> unresolvedModels = new List<UnresolvedModel>();

        public ModelResolver(ModelLoader loader)
        {
            Loader = loader;
        }

        public void MarkModelForResolution(IResolvable model, string id, string file, XElement element)
        {
            // Add a model object to be resolved later
            unresolvedModels.Add(new UnresolvedModel(model, id, file, element));
        }

        public void EnsureModelResolved()
        {
            Log.Info("Resolving model");

            int succeeded = 0;
            int failed = 0;

            foreach (var unresolved in unresolvedModels)
            {
                var model = Loader.GetModel(unresolved.Id);
                if (model is EnumModel)
                {
                    // Resolve to enum
                    var enumModel = (EnumModel)model;
                    var unresolvedModel = unresolved.Model as IResolvableFrom<EnumModel>;
                    if (unresolvedModel != null)
                    {
                        unresolvedModel.ResolveFrom(enumModel);
                        Log.Fine("{0} {1} resolved to enum '{2}'", unresolved.Name, unresolved.Id, enumModel.Name);
                        succeeded++;
                    }
                    else
                    {
                        Log.Error("Enumeration '{0}' cannot be used to resolve {1}", enumModel.Name, unresolved);
                        failed++;
                    }
                }
                else if (model is FieldModel)
                {
                    // Resolve to a field
                    var fieldModel = (FieldModel)model;
                    var unresolvedModel = unresolved.Model as IResolvableFrom<FieldModel>;
                    if (unresolvedModel != null)
                    {
                        unresolvedModel.ResolveFrom(fieldModel);
                        Log.Fine("{0} {1} resolved to field '{2}'", unresolved.Name, unresolved.Id, fieldModel.Name);
                        succeeded++;
                    }
                    else
                    {
                        Log.Error("Field '{0}' cannot be used to resolve {1}", fieldModel.Name, unresolved);
                        failed++;
                    }
                }
                else if (model is StructModel)
                {
                    // Resolve to a structure
                    var structModel = (StructModel)model;
                    var unresolvedModel = unresolved.Model as IResolvableFrom<StructModel>;
                    if (unresolvedModel != null)
                    {
                        unresolvedModel.ResolveFrom(structModel);
                        Log.Fine("{0} {1} resolved to struct '{2}'", unresolved.Name, unresolved.Id, structModel.Name);
                        succeeded++;
                    }
                    else
                    {
                        Log.Error("Struct '{0}' cannot be used to resolve {1}", structModel.Name, unresolved);
                        failed++;
                    }
                }
                else
                {
                    Log.Error("Unable to resolve {0}", unresolved);
                    failed++;
                }
            }

            Log.Info("Resolved {0} model objects", succeeded);
            if (failed > 0)
            {
                Log.Warning("Failed to resolve {0} model objects", failed);
            }
        }

        class UnresolvedModel
        {
            public string Name { get; private set; }
            public string Id { get; private set; }
            public IResolvable Model { get; private set; }
            public string File { get; private set; }
            public XElement Element { get; private set; }

            static Dictionary<Type, string> modelNames = new Dictionary<Type, string>()
            {
                { typeof(MemberModel), "Member" },
                { typeof(FieldModel), "Field" },
                { typeof(TargetModel), "Target" },
                { typeof(FunctionParamModel), "Function Parameter" }
            };

            public UnresolvedModel(IResolvable model, string id, string file, XElement element)
            {
                Model = model;
                Id = id;
                File = file;
                Element = element;

                Name = modelNames[model.GetType()];
            }

            public override string ToString()
            {
                return string.Format("{0} {1} in file {2} \nat {3} \n{4}", Name, Id, File, Element.GetAbsoluteXPath(), Element);
            }
        }
    }
}
