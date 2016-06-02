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
using Patcher.UI.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Patcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();

            Start((MainWindow)MainWindow, e.Args);

            MainWindow.Show();
        }

        private void Start(MainWindow window, string[] args)
        {
            window.MaxLogLevel = LogLevel.Info;

            Display.SetDisplay(window);
            Log.AddLogger(window);

            // Parse arguments, display help screen when appropriate
            var options = new ProgramOptions();
            try
            {
                options.Load(args);
            }
            catch (Exception ex)
            {
                Display.WriteText("Bad options: {0}\n\n{1}", ex.Message, options.GetOptions());
                window.Terminate(true);
                return;
            }

            if (options.ShowHelp)
            {
                Display.WriteText("{0}", options.GetOptions());
                window.Terminate(true);
                return;
            }

            // Set log level from options
            window.MaxLogLevel = options.ConsoleLogLevel >= 0 && options.ConsoleLogLevel <= 4 ? (LogLevel)options.ConsoleLogLevel : LogLevel.Info;

            // Set initial window state
            if (options.StartWindowMaximized)
                window.WindowState = WindowState.Maximized;
            else if (options.StartWindowMinimized)
                window.WindowState = WindowState.Minimized;

            // Run in the background
            new Task(() =>
            {
                Main(options);
                window.Terminate(!options.ExitWhenDone);
            }).Start();
        }

        private void Main(ProgramOptions options)
        {
            // Validate rules folder
            if (string.IsNullOrWhiteSpace(options.RulesFolder) || options.RulesFolder.IndexOfAny(new char[] { '"', '\'', '\\', '/', ':' }) >= 0)
            {
                Display.WriteText("Specified rule folder name does not seems to be valid: \n\n" + options.RulesFolder +
                    "\n\nEnsure the value is a single folder name (not a full path) without special characters: \", ', \\, / and :.");
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
                Display.WriteText("Data folder path does not seems to be valid: " + ex.Message + "\n\n" + options.DataFolder +
                    "\n\nUse option -d or --data to specify correct path to the data folder or use option -h or --help for more help.");
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
                    Display.WriteText("Incorrect Mod Organizer configuration: " + ex.Message +
                        "\n\nUse option -h or --help for more help.");
                    return;
                }
            }

            // Determine output plugin file name, use filename from options if provided
            string targetPluginFileName = options.OutputFilename ?? string.Format("Patcher-{0}.esp", options.RulesFolder);

            // File log will be created in the data folder, named as the output plugin plus .log extension
            string logFileName = Path.Combine(Program.ProgramFolder, Program.ProgramLogsFolder, targetPluginFileName + ".log");
            using (var logger = new StreamLogger(fileProvider.GetDataFile(FileMode.Create, logFileName).Open()))
            {
                Log.AddLogger(logger);

                // Put some handy info at the beginning of the log file
                Log.Info(Program.GetProgramVersionInfo());
                Log.Fine("Options: " + options);

                try
                {
                    // Create suitable data context according to the data folder content
                    using (DataContext context = DataContext.CreateContext(fileProvider))
                    {
                        Log.Fine("Initialized data context: " + context.GetType().FullName);

                        // Context tweaks
                        context.AsyncFormIndexing = true;
                        context.AsyncFormLoading = options.MaxLoadingThreads > 0;
                        context.AsyncFormLoadingWorkerThreshold = 100;
                        context.AsyncFromLoadingMaxWorkers = Math.Max(1, options.MaxLoadingThreads);

                        if (!options.Append)
                        {
                            // Inform context to ignore output plugin in case it is active
                            context.IgnorePlugins.Add(targetPluginFileName);
                        }

                        // Index all forms except hidden (such as cells and worlds)
                        context.Load();

                        if (options.Append && context.Plugins.Exists(targetPluginFileName) && context.Plugins.Count > 0)
                        {
                            // Make sure output plugin is loaded last when appending
                            var plugin = context.Plugins[(byte)(context.Plugins.Count - 1)];
                            if (!plugin.FileName.Equals(targetPluginFileName))
                                throw new ApplicationException("Output plugin must be loaded last when appending changes.");
                        }

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

                            foreach (var param in options.Parameters)
                            {
                                var split = param.Split('=');
                                if (split.Length != 2 || !split[0].Contains(':'))
                                {
                                    Log.Warning("Ignored malformatted parameter: '{0}' Expected format is 'plugin:param=value'", param);
                                }
                                engine.Params.Add(split[0], split[1]);
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

                            if (options.Append && context.Plugins.Exists(targetPluginFileName))
                            {
                                // Use existing plugin as the target plugin
                                engine.ActivePlugin = context.Plugins[targetPluginFileName];
                            }
                            else
                            {
                                // Create and set up target plugin
                                engine.ActivePlugin = context.CreatePlugin(targetPluginFileName);
                                engine.ActivePlugin.Author = options.Author ?? Program.GetProgramVersionInfo();
                                engine.ActivePlugin.Description = options.Description ?? string.Format("Generated by {0}", Program.GetProgramVersionInfo());
                            }

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
                            }

                            engine.Run();

                            if (!options.KeepDirtyEdits)
                                engine.ActivePlugin.PurgeDirtyEdits();

                            // Prepare list of master to be removed by force
                            IEnumerable<string> removeMasters = options.RemovedMasters != null ? options.RemovedMasters.Split(',') : null;

                            // Save target plugin
                            engine.ActivePlugin.Save(removeMasters);
                        }
                    }
                }
                catch (UserAbortException ex)
                {
                    Log.Error("Program aborted: " + ex.Message);
                }
#if !DEBUG
                catch (Exception ex)
                {
                    Log.Error("Program error: " + ex.Message);
                    Log.Error(ex.ToString());
                }
#endif
            }

            return;
        }
    }
}
