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

using Patcher.Data.Models.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Patcher.Data.Models.Loading
{
    /// <summary>
    /// Handles the loading of model files into the object model tree.
    /// </summary>
    public class ModelLoader
    {
        public ModelLoader()
        {
        }

        public static GameModel LoadGameModel(string path, IEnumerable<string> files)
        {
            Log.Info("Loading game model from file {0}", path);

            var document = XDocument.Load(path);
            var reader = new ModelReader(path, document.Root, null);

            return reader.ReadGameModel(files);
        }

        public IEnumerable<IModel> LoadFiles(IEnumerable<string> files)
        {
            var models = new List<IModel>();
            var functions = new List<FunctionModel>();
            var resolver = new ModelResolver();

            // Scan all files
            foreach (string path in files)
            {
                try
                {
                    var document = XDocument.Load(path);
                    var element = document.Root;
                    string id = Path.GetFileNameWithoutExtension(path);

                    // Create reader and enter the root element
                    var reader = new ModelReader(path, element, resolver);

                    if (element.Name == "functions")
                    {
                        functions.AddRange(reader.ReadFunctions());
                    }
                    else
                    {
                        var model = reader.ReadDocumentRootModel(id);
                        resolver.AddLoadedModel(id, model);
                        models.Add(model);
                    }
                }
                catch (XmlException e)
                {
                    Log.Error("Model file {0} could not be read because: {1}", path, e.Message);
                }
                catch (ModelLoadingException e)
                {
                    Log.Error("Model file loading error occured: {0}\nin file {1}{2}", e.Message, path, e.Element != null ? string.Format("\nat {0}\n{1}", e.Element.GetAbsoluteXPath(), e.Element) : string.Empty);
                }
            }

            // Make sure all model object relations are resolved
            resolver.ResolveModels();

            // Remove member fields
            foreach (var model in models.OfType<FieldModel>().ToArray())
            {
                models.Remove(model);
            }

            // Extract field groups and structures
            var inlineModels = ExtractInlineModels(models).ToArray();
            models.AddRange(inlineModels.Where(m => !models.Contains(m)));

            // Sort models 
            var sortByNameComparer = new ModelNameComparer();
            var enums = models.OfType<EnumModel>().ToList();
            var structs = models.OfType<StructModel>().ToList();
            var records = models.OfType<RecordModel>().ToList();
            var groups = models.OfType<FieldGroupModel>().ToList();
            enums.Sort(sortByNameComparer);
            structs.Sort(sortByNameComparer);
            records.Sort(sortByNameComparer);
            groups.Sort(sortByNameComparer);

            // Sort groups and structs separatedly according to hierarchy
            var groupAffinities = GetFieldGroupModelAffinities(groups);
            groups.Sort(new ModelAffinityComparer<FieldGroupModel>(groupAffinities));
            var structAffinities = GetStructModelAffinities(structs);
            structs.Sort(new ModelAffinityComparer<StructModel>(structAffinities));

            // Put models back into one list, in order
            models = new List<IModel>(enums).Union(structs).Union(groups).Union(records).Union(functions).ToList();

            // Print out everything except function models
            foreach (var m in models.Where(m => !(m is FunctionModel)))
            {
                Log.Fine(m.ToString());
            }

            return models.ToArray();
        }

        private IEnumerable<IModel> ExtractInlineModels(List<IModel> models)
        {
            // Root all field groups and stuctures from all records and structures
            return models.OfType<RecordModel>().SelectMany(r => r.Fields).SelectMany(f => ExtractInlineModels(f))
                .Union(models.OfType<StructModel>().SelectMany(r => r.Members).SelectMany(f => ExtractInlineModels(f)));
        }

        private IEnumerable<IModel> ExtractInlineModels(FieldModel field)
        {
            var models = new List<IModel>();
            if (field.TargetModel != null && field.TargetModel.Type is StructModel)
            {
                models.Add(field.TargetModel.Type);
                models.AddRange(((StructModel)field.TargetModel.Type).Members.SelectMany(m => ExtractInlineModels(m)));
            }

            if (field.IsFieldGroup)
            {
                models.Add(field.FieldGroup);
                models.AddRange(field.FieldGroup.Fields.SelectMany(f => ExtractInlineModels(f)));
            }
            else if (field.IsStruct)
            {
                models.Add(field.Struct);
                models.AddRange(field.Struct.Members.SelectMany(m => ExtractInlineModels(m)));
            }

            return models;
        }

        private IEnumerable<IModel> ExtractInlineModels(MemberModel member)
        {
            var models = new List<IModel>();
            if (member.IsStruct)
            {
                models.Add(member.Struct);
                models.AddRange(member.Struct.Members.SelectMany(m => ExtractInlineModels(m)));
            }
            if (member.TargetModel != null && member.TargetModel.Type is StructModel)
            {
                models.Add(member.TargetModel.Type);
                models.AddRange(((StructModel)member.TargetModel.Type).Members.SelectMany(m => ExtractInlineModels(m)));
            }
            return models;
        }

        private Dictionary<FieldGroupModel, int> GetFieldGroupModelAffinities(IEnumerable<FieldGroupModel> models)
        {
            var affinities = new Dictionary<FieldGroupModel, int>();

            foreach (var m in models)
            {
                GatherFieldGroupModelAffinities(m, 1, affinities);
            }

            return affinities;
        }

        private void GatherFieldGroupModelAffinities(FieldGroupModel model, int depth, Dictionary<FieldGroupModel, int> affinities)
        {
            if (affinities.ContainsKey(model))
            {
                affinities[model] = Math.Max(affinities[model], depth);
            }
            else
            {
                affinities.Add(model, depth);
            }

            // Gather each descendant field group
            foreach (var descendant in model.Fields.Select(m => m.Type).OfType<FieldGroupModel>())
            {
                GatherFieldGroupModelAffinities(descendant, depth + 1, affinities);
            }
        }

        private Dictionary<StructModel, int> GetStructModelAffinities(IEnumerable<StructModel> models)
        {
            var affinities = new Dictionary<StructModel, int>();
            
            foreach (var m in models)
            {
                GatherStructModelAffinities(m, 1, affinities);
            }

            return affinities;
        }

        private void GatherStructModelAffinities(StructModel model, int depth, Dictionary<StructModel, int> affinities)
        {
            if (affinities.ContainsKey(model))
            {
                affinities[model] = Math.Max(affinities[model], depth);
            }
            else
            {
                affinities.Add(model, depth);
            }

            // Gather each descendant strcut and also struct as target if available
            foreach (var child in model.Members.Select(m => m.Type).OfType<StructModel>().Union(model.Members.Where(m => m.TargetModel != null).Select(m => m.TargetModel.Type).OfType<StructModel>()))
            {
                GatherStructModelAffinities(child, depth + 1, affinities);
            }
        }

        private class ModelNameComparer : IComparer<IModel>
        {
            public int Compare(IModel x, IModel y)
            {
                var xp = x as IPresentable;
                var yp = y as IPresentable;

                if (xp == null && yp == null)
                {
                    return 0;
                }
                if (xp == null)
                {
                    return -1;
                }
                else if (yp == null)
                {
                    return 1;
                }
                else
                {
                    return string.Compare(xp.Name, yp.Name);
                }
            }
        }

        private class ModelAffinityComparer<TModel> : IComparer<TModel>
        {
            readonly IDictionary<TModel, int> affinities;

            public ModelAffinityComparer(IDictionary<TModel, int> affinities)
            {
                this.affinities = affinities;
            }

            public int Compare(TModel x, TModel y)
            {
                return affinities[y].CompareTo(affinities[x]);
            }
        }
    }
}
