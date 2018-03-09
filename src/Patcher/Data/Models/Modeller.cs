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

using Patcher.Data.Models.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models
{
    public class Modeller
    {
        public GameModel[] Models { get; private set; }

        public Modeller()
        {
        }

        public void LoadAllModels(IDataFileProvider fileProvider)
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

                var game = ModelLoader.LoadGameModel(file.FullPath);
                game.LoadModelFiles(files);

                games.Add(game);
            }

            Log.Info("Loaded {0} game models", games.Count);

            Models = games.ToArray();
        }
    }
}
