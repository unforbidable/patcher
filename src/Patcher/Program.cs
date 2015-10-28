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
using Patcher.Logging;
using Patcher.Rules;
using Patcher.UI;
using Patcher.UI.Terminal;
using Patcher.UI.Windows;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Patcher
{
    class Program
    {
        public readonly static string ProgramFolder = "Patcher";
        public readonly static string ProgramRulesFolder = "rules";
        public readonly static string ProgramCacheFolder = "cache";
        public readonly static string ProgramLogsFolder = "logs";

        public static IDisplay Display = new WindowDisplay();
        public static Status Status = Display.GetStatus();
        public static Prompt Prompt = Display.GetPrompt();

        [STAThread]
        static void Main(string[] args)
        {
            // Parse arguments, display help screen when appropriate
            var options = new ProgramOptions();
            if (!options.TryLoad(args))
            {
                return;
            }

            // Adjust console properties
             if (options.WindowWidth > 0)
            {
                Display.SetWindowWidth(options.WindowWidth);
            }
            if (options.WindowHeight > 0)
            {
                Display.SetWindowHeight(options.WindowHeight);
            }

            // Validate rules folder
            if (string.IsNullOrWhiteSpace(options.RulesFolder) || options.RulesFolder.IndexOfAny(new char[] { '"', '\'', '\\', '/', ':' }) >= 0)
            {
                Display.ShowPreRunMessage("Specified rule folder name does not seems to be valid: \n\n" + options.RulesFolder + 
                    "\n\nEnsure the value is a single folder name (not a full path) without special characters: \", ', \\, / and :.", true);
                return;
            }

            // Initialize data file provider
            IDataFileProvider fileProvider;
            try
            {
                // Use data folder path from options which will default the parent direcotry unless another path is specified
                // Pass custom plugins.txt file path if provided via options
                fileProvider = new DefaultDataFileProvider(options.DataFolder, options.PluginListFile);
            }
            catch (Exception ex)
            {
                // Program will exit on error
                // Appropriate hint is displayed
                Display.ShowPreRunMessage("Data folder path does not seems to be valid: " + ex.Message + "\n\n" + options.DataFolder + 
                    "\n\nUse option -d or --data to specify correct path to the data folder or use option -h or --help for more help.", true);
                return;
            }

            // Mod Ogranizer
            if (!string.IsNullOrEmpty(options.ModOrganizerProfile))
            {
                try
                {
                    // After verifying data folder with DefaultDataFolderProvider
                    // replace it with MO data file provider
                    fileProvider = new ModOrganizerDataFileProvider(options.DataFolder, options.ModOrganizerProfile, options.ModOrganizerModsPath);
                }
                catch (Exception ex)
                {
                    // Program will exit on error
                    // Appropriate hint is displayed
                    Display.ShowPreRunMessage("Incorrect Mod Organizer configuration: " + ex.Message + 
                        "\n\nUse option -h or --help for more help.", true);
                    return;
                }
            }

            // Setup terminal logger
            // Set terminal log level, 0-4
            Log.AddLogger(Display.GetLogger((LogLevel)Math.Min(4, Math.Max(0, options.ConsoleLogLevel))));

            // Determine output plugin file name, use filename from options if provided
            string targetPluginFileName = options.OutputFilename ?? string.Format("Patcher-{0}.esp", options.RulesFolder);

            // File log will be created in the data folder, named as the output plugin plus .log extension
            string logFileName = Path.Combine(ProgramFolder, ProgramLogsFolder, targetPluginFileName + ".log");
            using (var logger = new StreamLogger(fileProvider.GetDataFile(FileMode.Create, logFileName).Open()))
            {
                Log.AddLogger(logger);

                // Put some handy info at the beginning of the log file
                Log.Info(GetProgramVersionInfo());
                Log.Fine("Options: " + options);

                var task = new Task(() =>
                {
                    try
                    {
                        // Create suitable data context according to the data folder content
                        using (DataContext context = DataContext.CreateContext(fileProvider))
                        {
                            Log.Fine("Initialized data context: " + context.GetType().FullName);

                            // Context tweaks
                            context.AsyncFormIndexing = true;
                            context.AsyncFormLoading = true;
                            context.AsyncFormLoading = true;
                            context.AsyncFormLoadingWorkerThreshold = 100;
                            context.AsyncFromLoadingMaxWorkers = Math.Max(1, options.MaxLoadingThreads);

                            // Inform context to ignore output plugin in case it is active
                            context.IgnorePlugins.Add(targetPluginFileName);

                            // Index all forms except hidden (such as cells and worlds)
                            context.Load();

                            using (RuleEngine engine = new RuleEngine(context))
                            {
                                engine.RulesFolder = options.RulesFolder;

                                // Apply debug scope to rule engine
                                if (!string.IsNullOrEmpty(options.DebugScope))
                                {
                                    if (options.DebugScope == "*")
                                    {
                                        engine.DebugAll = true;
                                    }
                                    else
                                    {
                                        var parts = options.DebugScope.Split('/', '\\');
                                        if (parts.Length > 0)
                                            engine.DebugPluginFileName = parts[0];
                                        if (parts.Length > 1)
                                            engine.DebugRuleFileName = parts[1];
                                    }
                                }

                                // Load rules
                                engine.Load();

#if DEBUG
                                // Load supported forms in debug mode
                                context.LoadForms(f => context.IsSupportedFormKind(f.FormKind));
#else
                                // Load all indexed forms in release mode
                                context.LoadForms();
#endif

                                // Create and set up target plugin
                                engine.ActivePlugin = context.CreatePlugin(targetPluginFileName);
                                engine.ActivePlugin.Author = options.Author ?? GetProgramVersionInfo();
                                engine.ActivePlugin.Description = options.Description ?? string.Format("Generated by {0}", GetProgramVersionInfo());

                                // See if target plugin exists 
                                var targetPluginFile = fileProvider.GetDataFile(FileMode.Open, targetPluginFileName);
                                if (targetPluginFile.Exists())
                                {
                                    Log.Info("Target plugin {0} already exists and will be overwriten, however previously used FormIDs will be preserved if possible.", targetPluginFileName);

                                    try
                                    {
                                        var inspector = new PluginInspector(context, targetPluginFile);
                                        foreach (var record in inspector.NewRecords)
                                        {
                                            engine.ActivePlugin.ReserveFormId(record.FormId, record.EditorId);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Warning("Previously used Form IDs cannot be preserved because target plugin {0} could not be read: {1}", targetPluginFileName, ex.ToString());
                                    }
                                    finally
                                    {
                                    }
                                }

                                engine.Run();

                                if (!options.KeepDirtyEdits)
                                    engine.ActivePlugin.PurgeDirtyEdits();

                                // Save target plugin
                                engine.ActivePlugin.Save();
                            }
                        }
                    }
                    catch (UserAbortException ex)
                    {
                        Log.Error("Program aborted: " + ex.Message);
                    }
#if !DEBUG
                    // Catch any unhandled exception in a Release build only
                    catch (Exception ex)
                    {
                        Log.Error("Program error: " + ex.Message);
                        Log.Fine(ex.ToString());
                    }
#endif
                    finally
                    {
                    }
                });

                Display.Run(task);
            }
        }

        public static string GetProgramVersionInfo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = new Version(fvi.FileVersion);
            return string.Format("{0} {1}.{2}.{3}", assembly.GetName().Name, version.Major, version.Minor, version.Build);
        }

        public static string GetProgramFullVersionInfo()
        {
            return string.Format("{0} [{1}]", GetProgramVersionInfo(), Assembly.GetEntryAssembly().GetName().Version.ToString());
        }
    }
}
