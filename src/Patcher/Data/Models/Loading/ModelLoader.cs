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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Patcher.Data.Models.Loading
{
    public class ModelLoader
    {
        public GameModel Game { get; private set; }

        Dictionary<string, IModel> loadedModels = new Dictionary<string, IModel>();

        public ModelLoader(GameModel game)
        {
            Game = game;
        }

        public static GameModel LoadGameModel(string path)
        {
            Log.Info("Loading game model from file {0}", path);

            var document = XDocument.Load(path);
            if (document.Root.Name != "game")
            {
                throw new ModelLoadingException("Expected element 'game' in file " + path);
            }

            var gameModel = new GameModel();
            gameModel.ReadFromXml(document.Root);
            return gameModel;
        }

        public IModel GetModel(string id)
        {
            return loadedModels.ContainsKey(id) ? loadedModels[id] : null;
        }

        public void LoadFiles(IEnumerable<string> files)
        {
            var resolver = new ModelResolver(this);

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

                    switch (element.Name.LocalName)
                    {
                        case "record":
                            Log.Fine("Loading record model {0}", id);
                            loadedModels.Add(id, reader.ReadRecord(id));
                            break;

                        case "enum":
                            Log.Fine("Loading enum model {0}", id);
                            loadedModels.Add(id, reader.ReadEnum());
                            break;

                        case "struct":
                            Log.Fine("Loading struct model {0}", id);
                            loadedModels.Add(id, reader.ReadStruct());
                            break;

                        case "field":
                            Log.Fine("Loading field model {0}", id);
                            loadedModels.Add(id, reader.ReadField());
                            break;

                        case "functions":
                            // TODO: Read functions
                            break;

                        default:
                            throw new ModelLoadingException(string.Format("Unexpected root element '{0}'", element.Name));
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
            resolver.EnsureModelResolved();

            // TODO: Extract inline structures and field groups

            // TODO: Make sure the model is valid

            foreach (var pair in loadedModels.Where(p => p.Value.GetType() != typeof(FieldModel)))
            {
                Log.Fine("Loaded record model {0} as \n{1}", pair.Key, pair.Value);
            }

        }
    }
}
