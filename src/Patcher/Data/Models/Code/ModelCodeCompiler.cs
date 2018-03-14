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

using Microsoft.CSharp;
using Patcher.Code;
using Patcher.Code.Compiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Patcher.Data.Models.Code
{
    public class ModelCodeCompiler
    {
        const string modelAssemblyFilename = "Patcher.Data.Models.dll";
        const string modelSourceFilename = "models.cs";
        const string meraDataSourceFilename = "models.metadata.cs";
        const string modelDeflatedFilename = "models.json.gzip";

        public void CompileCode(CodeBase code, byte[] serializedModel)
        {
            var compiler = new CodeCompiler(modelAssemblyFilename);

            // Add model code
            compiler.AddCode(modelSourceFilename, code);

            // Add meta data class code
            string serializedModelsHash = ModelHelper.GetModelHash(serializedModel);
            var metaDataCode = GetMetaDataCode(serializedModelsHash, modelDeflatedFilename);
            compiler.AddCode(meraDataSourceFilename, metaDataCode);

            // Compress data model and add it as a resource
            using (var memoryStream = new MemoryStream())
            {
                using (var deflate = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    deflate.Write(serializedModel, 0, serializedModel.Length);
                }

                compiler.AddResource(modelDeflatedFilename, memoryStream.ToArray());
            }

            if (compiler.Compile())
            {
                // This is to test the DLL and the embedded model
                Assembly assembly = Assembly.LoadFrom(modelAssemblyFilename);
                var stream = assembly.GetManifestResourceStream(modelDeflatedFilename);
                var ms = new MemoryStream();
                using (var deflate = new GZipStream(stream, CompressionMode.Decompress))
                {
                    deflate.CopyTo(ms);
                }
                byte[] loadedModel = ms.ToArray();

                string serializedHash = ModelHelper.GetModelHash(serializedModel);
                string loadedHash = ModelHelper.GetModelHash(loadedModel);

                string serialzed = Encoding.UTF8.GetString(serializedModel);
                string loaded = Encoding.UTF8.GetString(loadedModel);

                if (serializedHash != loadedHash)
                {
                    Log.Error("Loaded model is different from the compiled model.");
                }
            }
        }

        private CodeBase GetMetaDataCode(string serializedModelHash, string modelResourceName)
        {
            var code = new CodeBase();
            var ns = new CodeNamespace("Data.Patcher.Models");
            code.Namespaces.Add(ns);

            var cls = new CodeClass("ModelMetaData");
            cls.IsStatic = true;
            cls.Comment = "Model meta data";

            cls.Members.Add(new CodeField("string", "ProgramVersion")
            {
                Value = Program.GetProgramFullVersionInfo(),
                IsPublic = true,
                IsConst = true
            });
            cls.Members.Add(new CodeField("string", "ModelHash")
            {
                Value = serializedModelHash,
                IsPublic = true,
                IsConst = true
            });
            cls.Members.Add(new CodeField("string", "ModelResourceName")
            {
                Value = modelResourceName,
                IsPublic = true,
                IsConst = true
            });

            ns.Types.Add(cls);
            return code;
        }
    }
}
