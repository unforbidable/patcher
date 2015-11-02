/// Copyright(C) 2015 Unforbidable Works
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

using Microsoft.CSharp;
using Patcher.Data.Plugins;
using Patcher.Rules.Compiled;
using Patcher.Rules.Methods;
using Patcher.UI;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;

namespace Patcher.Rules
{
    sealed class RuleCompiler
    {
        static string[] IllegalCodeTokens = { "System.", "Microsoft.", "Patcher.", ".GetType(", "typeof(" };

        const string NamespaceName = "Patcher.Rules.Compiled.Generated";
        const string VersionClassName = "Version";
        const string VersionFieldName = "VersionInfo";

        readonly RuleEngine engine;
        readonly string pluginFileName;
        readonly string cachePath;
        readonly string generatedAssemblyPath;

        int ruleClassCounter = 0;

        public bool HasRules { get { return units.Count > 0; } }
        public IEnumerable<CompiledRule> CompiledRules { get { return units.Select(u => u.Rule); } }

        List<RuleCompilationUnit> units = new List<RuleCompilationUnit>();

        public static IEnumerable<string> GetIllegalCodeTokens()
        {
            return IllegalCodeTokens.Select(t => t);
        }

        public static string GetCompilerErrorHint(CompilerError error)
        {
            switch (error.ErrorNumber)
            {
                case "CS0029":
                    return "The value which is being assigned to a variable or property is not the correct type.";

                case "CS0103":
                    return "Make sure the object which is being accessed has been declared. When accessing predefined Source and Target forms, helpers and constants make sure the correct letter case is used.";

                case "CS0266":
                    return "This error usually occurs when assigning a floating point value to an integer. If that is the case use methods Math.Round(), Math.Floor() or Math.Ceiling() to retrieve an integer value.";

                case "CS0428":
                    return "The most likely cause of this error is that parentheses are missing after the method name which is being invoked. Parentheses are required even when the method has not parameters.";

                case "CS1061":
                    return "Make sure the method or property is defined for the form, helper or another object on which it is invoked or accessed and the correct letter case is used.";

                case "CS1501":
                    return "Make sure to pass the correct number of arguments to the method.";

                case "CS1502":
                    return "Make sure the arguments passed to the method are the correct type.";

                case "CS1503":
                    return "The method was expecing an argument of different type.";

                case "CS1955":
                    return "The most likely cause of this error is that parentheses are used after the property name which is being accessed. Properties are accessed without parentheses.";

                default:
                    // No hint is offered
                    return string.Empty;
            }
        }

        public RuleCompiler(RuleEngine engine, string pluginFileName)
        {
            this.engine = engine;
            this.pluginFileName = pluginFileName;
            cachePath = Path.Combine(Program.ProgramFolder, Program.ProgramCacheFolder, engine.RulesFolder, pluginFileName);
            generatedAssemblyPath = Path.Combine(cachePath, NamespaceName + ".dll");
        }

        public void Add(RuleEntry entry, RuleMetadata metadata, bool debug)
        {
            string className = "Rule_" + ruleClassCounter++;

            var unit = new RuleCompilationUnit()
            {
                ClassName = className,
                ClassFullName = string.Format("{0}.{1}", NamespaceName, className),
                IsDebugModeEnabled = debug,
                Rule = new CompiledRule(engine, metadata)
                {
                    From = entry.From == null ? FormKind.Any : (FormKind)entry.From.FormKind
                },
            };

            string comment = string.Format("//\n// Source file for rule {0}\\{1}@{2} by {3}\n//\n", metadata.PluginFileName, metadata.RuleFileName, entry.Name, Program.GetProgramFullVersionInfo());
            CodeBuilder builder = new CodeBuilder(NamespaceName, className, comment);

            builder.Usings.Add("Patcher.Rules.Compiled.Helpers");
            builder.Usings.Add("Patcher.Rules.Compiled.Helpers." + engine.Context.GameTitle);
            builder.Usings.Add("Patcher.Rules.Compiled.Extensions");
            builder.Usings.Add("Patcher.Rules.Compiled.Extensions." + engine.Context.GameTitle);
            builder.Usings.Add("Patcher.Rules.Compiled.Constants");
            builder.Usings.Add("Patcher.Rules.Compiled.Constants." + engine.Context.GameTitle);

            Type sourceProxyType = entry.From != null ? engine.ProxyProvider.GetInterface((FormKind)entry.From.FormKind) : typeof(object);

            if (entry.Where != null)
            {
                string code = PreprocessCode(entry.Where.Code);
                if (StripComments(code).Length > 0)
                {
                    // TODO: optimize HasTag() with index similarly

                    var match = Regex.Match(code, @"^\s*Source\.EditorId\s*==\s*""([^""]*)""\s*$");
                    if (match.Success)
                    {
                        unit.Rule.WhereEditorId = match.Groups[1].Value;

                        Log.Fine("Where criteria EditorID == '" + match.Groups[1].Value + "' will use an index.");
                    }
                    else
                    {
                        builder.BeginMethod("Where", sourceProxyType);
                        builder.WriteCodeAsReturn(code);
                        builder.EndMethod();

                        unit.Rule.Where = new WhereMethod();
                    }
                }
            }

            if (entry.Select != null)
            {
                string code = PreprocessCode(entry.Select.Code);
                if (StripComments(code).Length > 0)
                {
                    builder.BeginMethod("Select", sourceProxyType);
                    builder.WriteCode(code);
                    builder.ReturnTrue();
                    builder.EndMethod();

                    unit.Rule.Select = new SelectMethod();
                }
            }

            if (entry.Update != null)
            {
                string code = PreprocessCode(entry.Update.Code);
                if (StripComments(code).Length > 0)
                {
                    builder.BeginMethod("Update", sourceProxyType, sourceProxyType);
                    builder.WriteCode(code);
                    builder.ReturnTrue();
                    builder.EndMethod();

                    unit.Rule.Update = new UpdateMethod();
                }
            }

            if (entry.Inserts != null)
            {
                List<InsertMethod> methods = new List<InsertMethod>();
                foreach (var insert in entry.Inserts)
                {
                    string code = PreprocessCode(insert.Code);
                    if (StripComments(code).Length > 0)
                    {
                        Type targetProxyType = engine.ProxyProvider.GetInterface((FormKind)insert.InsertedFormKind);
                        builder.BeginMethod("Insert_" + methods.Count, sourceProxyType, targetProxyType);
                        builder.WriteCode(code);
                        builder.ReturnTrue();
                        builder.EndMethod();
                    }

                    methods.Add(new InsertMethod()
                    {
                        InsertedFormId = insert.InsertedFormId,
                        InsertedFormKind = (FormKind)insert.InsertedFormKind,
                        Copy = insert.Copy
                    });
                }
                unit.Rule.Inserts = methods.ToArray();
            }

            // Declare static helper fields
            foreach (var helper in engine.HelperProvider.Helpers)
            {
                // Skip declaration of helpers that are not used in debug mode if debug mode is disabled
                if (helper.DebugModeOnly && !debug)
                    continue;

                builder.WriteCode(string.Format("static readonly {0} {1} = null;", helper.InterfaceType.FullName, helper.Name));
            }

            string finalCode = builder.ToString();
            if (!debug)
            {
                finalCode = StripDebug(finalCode);
            }

            // Write to memory stream
            using (var memoryStream = new MemoryStream(finalCode.Length))
            {
                var writer = new StreamWriter(memoryStream);
                writer.Write(finalCode);
                writer.Flush();
                memoryStream.Position = 0;

                // Find available file name for the new source file.
                int ruleNumber = 0;
                string sourceFilePath;
                do
                {
                    sourceFilePath = string.Format("{0}@{1:00}.cs", Path.Combine(cachePath, unit.Rule.Metadata.RuleFileName), ruleNumber++);
                } while (units.Any(u => u.Source == sourceFilePath));

                // Update source if necessary
                var sourceFile = engine.Context.DataFileProvider.GetDataFile(FileMode.Create, sourceFilePath);
                bool updated = sourceFile.CopyFrom(memoryStream, true);

                unit.Source = sourceFilePath;
                unit.Updated = updated;
            }

            units.Add(unit);
        }

        public bool CompileAll()
        {
            if (units.Count == 0)
            {
                Log.Warning("No rules form plugin {0}. Nothing to compile.", pluginFileName);
                return false;
            }

            // Collect sources into a list, checking whether any has been updated
            bool anyUpdated = false;
            List<string> sources = new List<string>();
            foreach (var unit in units)
            {
                if (unit.Updated)
                    anyUpdated = true;

                // Get full path to source fine
                sources.Add(engine.Context.DataFileProvider.GetDataFile(FileMode.Open, unit.Source).FullPath);
            }

            if (!anyUpdated)
            {
                // None of the source files have been updated
                // so the cached rules assembly can be used, if it still exists
                // and appears to be valid
                var cachedAssemblyFile = engine.Context.DataFileProvider.GetDataFile(FileMode.Open, generatedAssemblyPath);
                if (cachedAssemblyFile.Exists())
                {
                    //if (ValidateCachedAssemblyVersion(cachedAssemblyFile.FullPath))
                    {
                        try
                        {
                            // Load cached assembly into the domain
                            Assembly assembly = Assembly.LoadFrom(cachedAssemblyFile.FullPath);

                            LoadMethodsFromAssembly(assembly);
                            Log.Info("Using cached assembly containing compiled rules for plugin {0}.", pluginFileName);
                            return true;
                         }
                        catch (Exception ex)
                        {
                            Log.Error("Something is wrong with cached compiled rules.");
                            Log.Error("If this problem persists please delete this file: {0}", cachedAssemblyFile.FullPath);
                            Log.Error("Then run this program again.");
                            throw new InvalidProgramException("Cached assembly containing compiled rules for plugin " + pluginFileName + " could not be loaded: " + ex.Message);
                        }
                    }
                    //else
                    //{
                    //    Log.Fine("Cached assembly containing compiled rules for plugin {0} is no longer valid and needs to be recompiled.", pluginFileName);
                    //}
                }
                else
                {
                    Log.Fine("Cached assembly containing compiled rules for plugin {0} could not be found.", pluginFileName);
                }
            }

            //// Create version.cs
            //CodeBuilder builder = new CodeBuilder(NamespaceName, VersionClassName, "//\n// Source file for version tracking.\n//\n\n");
            //builder.WriteCode(string.Format("public static readonly string {0} = \"{1}\";\n", VersionFieldName, Program.GetProgramVersionInfo()));
            //string versionCode = builder.ToString();
            //string versionFilePath = Path.Combine(cachePath, "version.cs");
            //var versionFile = engine.Context.DataFileProvider.GetDataFile(FileMode.Create, versionFilePath);
            //using (var stream = versionFile.Open())
            //{
            //    using (var writer = new StreamWriter(stream))
            //    {
            //        writer.Write(versionCode);
            //    }
            //}
            //sources.Add(versionFile.FullPath);

            // Compile all sources
            Log.Info("Compiling rules for plugin {0}.", pluginFileName);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            // Following 2 needed for dynamic type
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll"); 
            parameters.ReferencedAssemblies.Add("System.Core.dll"); 
            // Extracted embedded resource with proxies and helpers
            parameters.ReferencedAssemblies.Add(engine.Context.DataFileProvider.GetDataFile(FileMode.Open, RuleEngine.CompiledRulesAssemblyPath).FullPath); 
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = true;
            parameters.OutputAssembly = engine.Context.DataFileProvider.GetDataFile(FileMode.Create, generatedAssemblyPath).FullPath;
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, sources.ToArray());

            // Version file is not needed to be cached
            // new one will always be created when needed
            //versionFile.Delete();

            if (results.Errors.HasErrors)
            {
                SortedSet<string> sourcesWithErrors = new SortedSet<string>();

                foreach (CompilerError error in results.Errors)
                {
                    //Log.Error(string.Format("Compiler {0} in file `{1}` at line {2:000}: {3}", error.IsWarning ? "warning" : "error", error.FileName, error.Line, error.ErrorText));
                    if (!error.IsWarning)
                        Log.Error(error.ToString());

                    // Collect filenames of sources with errors
                    if (!sourcesWithErrors.Contains(error.FileName))
                        sourcesWithErrors.Add(error.FileName);
                }

                // Alter the sources that failed to ensure recompilation
                // even if the source does not change 
                foreach (var path in sourcesWithErrors)
                {
                    File.AppendAllText(path, " ");
                }

                throw new CompilerException("Error(s) occured during rule compilation", results.Errors);
            }

            LoadMethodsFromAssembly(results.CompiledAssembly);

            return true;
        }

        private bool ValidateCachedAssemblyVersion(string path)
        {
            AppDomain domain = AppDomain.CreateDomain("VersionChecker", 
                new Evidence(AppDomain.CurrentDomain.Evidence), 
                AppDomain.CurrentDomain.SetupInformation);

            try
            {
                Type checkerType = typeof(VersionChecker);
                var checker = (VersionChecker)domain.
                            CreateInstanceFrom(
                            checkerType.Assembly.Location,
                            checkerType.FullName).Unwrap();

                checker.LoadAssembly(path);
                string version = checker.GetVersion();
                if (version != Program.GetProgramVersionInfo())
                    return false;

                return EnsureMethodsExist(checker.GetMethods());
            }
            finally
            {
                AppDomain.Unload(domain);
            }

        }

        class VersionChecker : MarshalByRefObject
        {
            Assembly assembly;

            internal string GetVersion()
            {
                Type versionType = assembly.GetType(string.Format("{0}.{1}", NamespaceName, VersionClassName));
                if (versionType == null)
                    return null;

                FieldInfo versionField = versionType.GetField(VersionFieldName, BindingFlags.Static | BindingFlags.Public);
                if (versionField == null)
                    return null;

                object versionValue = versionField.GetValue(null);
                if (versionValue == null)
                    return null;

                return versionValue.ToString();
            }

            internal string[] GetMethods()
            {
                List<string> methods = new List<string>();
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                    {
                        methods.Add(string.Format("{0}.{1}", type.Name, method.Name));
                    }
                }
                return methods.ToArray();
            }

            internal void LoadAssembly(string assemblyPath)
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
        }

        private bool EnsureMethodsExist(string[] methods)
        {
            foreach (var unit in units)
            {
                if (unit.Rule.Where != null)
                {
                    var method = string.Format("{0}.Where", unit.ClassName);
                    if (!methods.Contains(method))
                        return false;
                }

                if (unit.Rule.Select != null)
                {
                    var method = string.Format("{0}.Select", unit.ClassName);
                    if (!methods.Contains(method))
                        return false;
                }

                if (unit.Rule.Update != null)
                {
                    var method = string.Format("{0}.Update", unit.ClassName);
                    if (!methods.Contains(method))
                        return false;
                }

                if (unit.Rule.Inserts != null)
                {
                    for (int i = 0; i < unit.Rule.Inserts.Length; i++)
                    {
                        var method = string.Format("{0}.Insert_{1}", unit.ClassName, i);
                        if (!methods.Contains(method))
                            return false;
                    }
                }
            }

            return true;
        }

        private void LoadMethodsFromAssembly(Assembly assembly)
        {
            foreach (var unit in units)
            {
                Type compiledRule = assembly.GetType(unit.ClassFullName);

                // Assign static helper fields
                CompiledRuleContext context = new CompiledRuleContext(unit.Rule);
                foreach (var helper in engine.HelperProvider.Helpers)
                {
                    // Skip initialization of helpers that are not used in debug mode if debug mode is disabled
                    if (helper.DebugModeOnly && !unit.IsDebugModeEnabled)
                        continue;

                    var field = compiledRule.GetField(helper.Name, BindingFlags.Static | BindingFlags.NonPublic);
                    field.SetValue(null, helper.CreateInstance(context));
                }

                if (unit.Rule.Where != null)
                {
                    MethodInfo method = GetMethod(compiledRule, "Where");
                    unit.Rule.Where.Method = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), method);
                }

                if (unit.Rule.Select != null)
                {
                    MethodInfo method = GetMethod(compiledRule, "Select");
                    unit.Rule.Select.Method = (Func<object, bool>)Delegate.CreateDelegate(typeof(Func<object, bool>), method);
                }

                if (unit.Rule.Update != null)
                {
                    MethodInfo method = GetMethod(compiledRule, "Update");
                    unit.Rule.Update.Method = (Func<object, object, bool>)Delegate.CreateDelegate(typeof(Func<object, object, bool>), method);
                }

                if (unit.Rule.Inserts != null)
                {
                    for (int i = 0; i < unit.Rule.Inserts.Length; i++)
                    {
                        MethodInfo method = GetMethod(compiledRule, "Insert_" + i);
                        unit.Rule.Inserts[i].Method = (Func<object, object, bool>)Delegate.CreateDelegate(typeof(Func<object, object, bool>), method);
                    }
                }
            }
        }

        private MethodInfo GetMethod(Type type, string name)
        {
            MethodInfo method = type.GetMethod(name);
            if (method == null)
                throw new InvalidProgramException("Method " + name + " not found in compiled rule " + type.Name + ".");

            return method;
        }

        private string PreprocessCode(string code)
        {
            // Trim lines
            code = TrimLines(code);

            CheckIllegalTokens(code);

            return code;
        }

        private string StripDebug(string code)
        {
            // Comment out all calls to Debug class
            code = Regex.Replace(code, @"(\s*)(Debug\s*\.\s*(Message|Assert|Dump|Break|Pause)\s*\([^;]*;)", @"$1/* $2 */", RegexOptions.Multiline);
            return code;
        }

        private string StripComments(string text)
        {
            return Regex.Replace(text, @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/", "$1");
        }

        private string StripBlanks(string text)
        {
            return Regex.Replace(text, @"\s", string.Empty);
        }

        private string StripStrings(string text)
        {
            return Regex.Replace(text, @"""[^""\\]*(?:\\.[^""\\]*)*""", @"""""", RegexOptions.Singleline);
        }

        private string TrimLines(string text)
        {
            return Regex.Replace(text, @"(\n)\s*", "$1").Trim();
        }

        private void CheckIllegalTokens(string code)
        {
            // Strip and compact code
            string stripped = StripBlanks(StripStrings(StripComments(code)));

            foreach (var token in IllegalCodeTokens)
            {
                if (stripped.Contains(token))
                {
                    throw new IllegalTokenException(string.Format("Illegal token '{0}' found in rule code" , token));
                }
            }
        }

        class RuleCompilationUnit
        {
            public CompiledRule Rule { get; set; }
            public string ClassName { get; set; }
            public string ClassFullName { get; set; }
            public bool IsDebugModeEnabled { get; set; }
            public string Source { get; set; }
            public bool Updated { get; set; }
        }
    }
}
