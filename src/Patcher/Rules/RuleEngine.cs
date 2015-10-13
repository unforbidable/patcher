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

using Patcher.Data;
using Patcher.Data.Plugins;
using Patcher.Rules.Proxies;
using Patcher.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patcher.Rules
{
    public sealed class RuleEngine : IDisposable
    {
        readonly DataContext context;
        public DataContext Context { get { return context; } }

        readonly TagManager tagManager;
        public TagManager Tags { get { return tagManager; } }

        readonly ObjectDumper dupmper;
        internal ObjectDumper Dumper { get { return dupmper; } }

        public string RulesFolder { get; set; }

        IDictionary<string, List<IRule>> rules = new Dictionary<string, List<IRule>>();

        readonly ProxyProvider proxyProvider;
        public ProxyProvider ProxyProvider { get { return proxyProvider; } }

        public Plugin ActivePlugin { get; set; }

        public bool DebugAll { get; set; }

        public string DebugPluginFileName { get; set; }
        public string DebugRuleFileName { get; set; }

        public readonly static string CompiledRulesAssemblyPath = Path.Combine(Program.ProgramFolder, Program.ProgramCacheFolder, "Patcher.Rules.Compiled.dll");

        public RuleEngine(DataContext context)
        {
            this.context = context;

            proxyProvider = new ProxyProvider(this);
            tagManager = new TagManager(this);
            dupmper = new ObjectDumper(this);

            ExtractCompiledAssemblyFile();
        }

        private void ExtractCompiledAssemblyFile()
        {
            string ressourceName = @"costura.patcher.rules.compiled.dll.zip";
            using (var unzip = new DeflateStream(GetType().Assembly.GetManifestResourceStream(ressourceName), CompressionMode.Decompress))
            {
                var outputFile = context.DataFileProvider.GetDataFile(FileMode.Create, CompiledRulesAssemblyPath);
                outputFile.CopyFrom(unzip, true);
            }
        }

        public void Load()
        {
            Log.Info("Loading rules");
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();

            foreach (var pluginFileName in context.Plugins.Select(p => p.FileName))
            {
                var compiler = new RuleCompiler(this, pluginFileName);

                string path = Path.Combine(Program.ProgramFolder, Program.ProgramRulesFolder, RulesFolder, pluginFileName);
                foreach (var file in Context.DataFileProvider.FindDataFiles(path, "*.rules"))
                {
                    using (var stream = file.Open())
                    {
                        bool isDebugModeEnabled = DebugAll ||
                            pluginFileName.Equals(DebugPluginFileName, StringComparison.OrdinalIgnoreCase) &&
                            (DebugRuleFileName == null || Path.GetFileName(file.Name).Equals(DebugRuleFileName, StringComparison.OrdinalIgnoreCase));

                        int count = 0;
                        using (RuleReader reader = new RuleReader(stream))
                        {
                            foreach (var entry in reader.ReadRules())
                            {
                                if (entry.Select == null && entry.Update == null && entry.Inserts.Count() == 0)
                                {
                                    Log.Warning("Rule {0} in file {1} ignored because it lacks any operation", entry.Name, pluginFileName);
                                    continue;
                                }

                                var metadata = new RuleMetadata()
                                {
                                    PluginFileName = pluginFileName,
                                    RuleFileName = Path.GetFileName(file.Name),
                                    Name = entry.Name,
                                    Description = entry.Description,
                                };

                                Log.Fine("Loading rule {0}\\{1}@{2}", metadata.PluginFileName, metadata.RuleFileName, metadata.Name);

                                compiler.Add(entry, metadata, isDebugModeEnabled);

                                count++;
                            }
                        }
                        Log.Fine("Loaded {0} rule(s) from file {1}", count, stream.Name);
                    }
                }

                if (compiler.HasRules)
                {
                    compiler.CompileAll();
                    rules.Add(pluginFileName, new List<IRule>(compiler.CompiledRules));
                }
            }
            sw1.Stop();
            Log.Fine("Task finished in {0}ms", sw1.ElapsedMilliseconds);
        }

        public void Run()
        {
            int totalRulesToRun = rules.SelectMany(p => p.Value).Count();
            if (totalRulesToRun == 0)
            {
                Log.Warning("No rules loaded, nothing to run.");
                return;
            }

            using (var progress = Program.Status.StartProgress("Running rules"))
            {
                int run = 0;
                foreach (var pluginFileName in rules.Keys)
                {
                    foreach (var rule in rules[pluginFileName])
                    {
                        Log.Info("Running rule {0}", rule);
                        progress.Update(run++, totalRulesToRun, "{0}", rule);

                        // Run rule
                        var runner = new RuleRunner(this, rule);
                        try
                        {
                            runner.Run();
                        }
#if !DEBUG
                        // Catch any unhandled exception in a Release build only
                        catch (Exception ex)
                        {
                            if (ex is CompiledRuleAssertException)
                            {
                                Log.Error("Assertion failed while executing rule {0} with message: {1}", rule, ex.Message);
                            }
                            else
                            {
                                Log.Error("Error occured while executing rule {0} with message: {1}", rule, ex.Message);
                            }
                            Log.Fine(ex.ToString());

                            var choice = Program.Prompt.Choice("Continue executing rules?", ChoiceOption.Ok, ChoiceOption.Cancel);
                            if (choice == ChoiceOption.Cancel)
                            {
                                Log.Warning("Rule execution has been aborted.");
                                throw new UserAbortException("Rule execution has been aborted by the user.");
                            }
                            else
                            {
                                Log.Warning("Any changes made by the faulty rule were discarded.");
                                continue;
                            }
                        }
#endif
                        finally
                        {
                        }

                        // Create/Override/Update forms when the rule is completed
                        ActivePlugin.AddForms(runner.Result);

                        Log.Info("Completed rule with {0} updates and {1} inserts", runner.Updated, runner.Created);
                    }

                    // After all rules of a plugin were run
                    // Clear tags
                    Tags.Clear();
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
