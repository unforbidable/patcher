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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Data.Models.Serialization
{
    public class ModelSerializer
    {
        public string SerializeModel(IEnumerable<GameModel> models)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new ModelSerializationWriter(ms);
                writer.Pretty = false;
                writer.WriteModels(models);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public IEnumerable<GameModel> DeserializeModel(string modelData)
        {
            // TODO: Deserialize model
            return Enumerable.Empty<GameModel>();
        }
    }
}
