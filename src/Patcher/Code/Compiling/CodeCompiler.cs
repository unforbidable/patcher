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
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.Code.Compiling
{
    public class CodeCompiler
    {
        Dictionary<string, CodeBase> codes = new Dictionary<string, CodeBase>();
        Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>();

        readonly string output;
        readonly string[] references;

        public CodeCompiler(string output, params string[] references)
        {
            this.output = output;
            this.references = references;
        }

        public void AddCode(string filename, CodeBase code)
        {
            codes.Add(filename, code);
        }

        public void AddResource(string filename, byte[] resource)
        {
            resources.Add(filename, resource);
        }

        public bool Compile()
        {
            var parameters = new CompilerParameters();
            parameters.OutputAssembly = output;
            parameters.ReferencedAssemblies.AddRange(references);
            parameters.IncludeDebugInformation = true;
            parameters.GenerateExecutable = false;

            foreach (var pair in codes)
            {
                File.WriteAllText(pair.Key, pair.Value.BuildCode(true));
            }

            foreach (var pair in resources)
            {
                File.WriteAllBytes(pair.Key, pair.Value);
                parameters.EmbeddedResources.Add(pair.Key);
            }

            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var results = csc.CompileAssemblyFromFile(parameters, codes.Keys.ToArray());

            foreach (var error in results.Errors)
            {
                Log.Error("Code compiler: {0}", error);
            }

            if (results.Errors.HasErrors)
            {
                Log.Error("Unable to compile model due to errors.");
            }

            return !results.Errors.HasErrors;
        }
    }
}
