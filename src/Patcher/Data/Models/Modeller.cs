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

using Patcher.Data.Models.Code;
using Patcher.Data.Models.Loading;
using Patcher.Data.Models.Serialization;
using Patcher.Data.Models.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    /// <summary>
    /// Provides methods to load, compile and query the data model. 
    /// </summary>
    public class Modeller
    {
        public GameModel[] Models { get; private set; }

        public Modeller()
        {
        }

        public void LoadModels(IDataFileProvider fileProvider)
        {
            var games = new List<GameModel>();

            string modelsPath = Path.Combine(Program.ProgramFolder, Program.ProgramModelsFolder);
            Log.Info("Loading models from path {0}", modelsPath);

            // Find game model xmls in the root folder
            foreach (var file in fileProvider.FindDataFiles(modelsPath, "*.xml", false))
            {
                string id = Path.GetFileNameWithoutExtension(file.FullPath);
                string gameFolderPath = Path.Combine(Program.ProgramFolder, Program.ProgramModelsFolder, id);
                var files = fileProvider.FindDataFiles(gameFolderPath, "*.xml", true).Select(x => x.FullPath).ToArray();

                games.Add(ModelLoader.LoadGameModel(file.FullPath, files));
            }

            Log.Info("Loaded {0} game models", games.Count);

            Models = games.ToArray();
        }

        public bool ValidateModels()
        {
            bool valid = true;

            Log.Info("Validating game model");
            foreach (var model in Models)
            {
                var issues = ModelValidator.ValidateGameModel(model);
                Log.Info("Model {0} validated with {1} errors, {2} warnings and {3} notices.", model.Name, issues.Errors.Count(), issues.Warnings.Count(), issues.Notices.Count());

                // Print issues
                foreach (var i in issues)
                {
                    switch (i.Type)
                    {
                        case ModelValidatorIssueType.Error:
                            Log.Error("{0}: {1}", i.Location, i.Text);
                            break;

                        case ModelValidatorIssueType.Warning:
                            Log.Warning("{0}: {1}", i.Location, i.Text);
                            break;

                        case ModelValidatorIssueType.Notice:
                            Log.Info("{0}: {1}", i.Location, i.Text);
                            break;
                    }
                }

                valid &= !issues.Errors.Any();
            }

            if (!valid)
            {
                Log.Error("Data model is not valid due to error(s).");
            }

            return valid;
        }

        public void CompileModels()
        {
            var builder = new ModelCodeBuilder();
            var code = builder.BuildModels(Models);

            var serializer = new ModelSerializer();
            var serializedModel = serializer.SerializeModel(Models, true);

            var compiler = new ModelCodeCompiler();
            compiler.CompileCode(code, serializedModel);

            //Log.Fine("Models compiled as\n{0}", code.BuildCode(true));
        }
    }
}
