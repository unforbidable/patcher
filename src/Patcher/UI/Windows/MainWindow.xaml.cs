using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Patcher.Logging;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Threading;

namespace Patcher.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<LogItem> logItems = new ObservableCollection<LogItem>();

        public bool TerminateOnEscape { get; set; }

        bool autoScrollEnabled = true;

        ChoiceOption chosenOption = ChoiceOption.Cancel;
        ChoiceOption[] offeredOptions = new ChoiceOption[] { };
        AutoResetEvent waitFormChoseOption = new AutoResetEvent(false);

        public MainWindow()
        {
            InitializeComponent();

            DataContext = logItems;

            AppLabel.Content = Program.GetProgramVersionInfo();

            logItems.CollectionChanged += LogItems_CollectionChanged;
        }

        internal ChoiceOption OfferChoice(string message, ChoiceOption[] options)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                PromptLabel.Content = message;
                PromptYesButton.Visibility = options.Contains(ChoiceOption.Yes) ? Visibility.Visible : Visibility.Collapsed;
                PromptNoButton.Visibility = options.Contains(ChoiceOption.No) ? Visibility.Visible : Visibility.Collapsed;
                PromptOkButton.Visibility = options.Contains(ChoiceOption.Ok) ? Visibility.Visible : Visibility.Collapsed;
                PromptCancelButton.Visibility = options.Contains(ChoiceOption.Cancel) ? Visibility.Visible : Visibility.Collapsed;
                PromptControl.Visibility = Visibility.Visible;
            }));

            offeredOptions = options;

            // Wait until an option is chosen
            waitFormChoseOption.Reset();
            waitFormChoseOption.WaitOne();

            return chosenOption;
        }

        private void OptionChosen(ChoiceOption option)
        {
            // Ignore if not one of the offered options
            if (offeredOptions.Contains(option))
            {
                PromptControl.Visibility = Visibility.Collapsed;
                chosenOption = option;

                waitFormChoseOption.Set();
            }
        }

        //LogLevel currentLogLevel = LogLevel.None;
        //List<string> logMessageBuffer = new List<string>();

        internal void WriteLogEntry(LogEntry entry)
        {
            DoWriteLogMessage(entry.Level, entry.Text);

            //if (currentLogLevel != entry.Level || logMessageBuffer.Count > 2)
            //{
            //    // Write accumulated log messages of the same kind
            //    DoWriteLogMessage(currentLogLevel, string.Join("\n", logMessageBuffer));
            //    logMessageBuffer.Clear();

            //    currentLogLevel = entry.Level;
            //}

            //logMessageBuffer.Add(entry.Text);
        }

        private void DoWriteLogMessage(LogLevel level, string text)
        {
            switch (level)
            {
                case LogLevel.Error:
                    WriteMessage(Brushes.OrangeRed, text);
                    break;

                case LogLevel.Warning:
                    WriteMessage(Brushes.Orange, text);
                    break;

                case LogLevel.Info:
                    WriteMessage(Brushes.White, text);
                    break;

                case LogLevel.Fine:
                    WriteMessage(Brushes.DarkGray, text);
                    break;
            }
        }

        internal void ShowStatusMessage(string text)
        {
            SetStatusText(Brushes.LimeGreen, text);
        }

        private void SetStatusText(SolidColorBrush brush, string text)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                StatusLabel.Content = text.TrimEnd('.');
                StatusLabel.Foreground = brush;

                if (string.IsNullOrEmpty(text))
                    StatusPanel.Visibility = Visibility.Collapsed;
                else
                    StatusPanel.Visibility = Visibility.Visible;
            }));
        }

        internal void UpdateProgress(double current, double total)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                StatusProgressBar.Visibility = current == 1 ? Visibility.Hidden : Visibility.Visible;
                StatusProgressBar.Value = current;
                StatusProgressBar.Maximum = total;
            }));
        }

        internal void WriteMessage(Brush brush, string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => 
            {
                logItems.Add(new LogItem()
                {
                    Brush = brush,
                    Text = message
                });
            }));
        }

        private void LogItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (LoggerItemsControl.IsLoaded)
            {
                var scrollViewer =
                    VisualTreeHelper.GetChild(LoggerItemsControl, 0) as ScrollViewer;

                if (autoScrollEnabled)
                    scrollViewer.ScrollToEnd();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    OptionChosen(ChoiceOption.Cancel);
                    if (TerminateOnEscape)
                    {
                        Close();
                    }
                    e.Handled = true;
                    break;

                case Key.Y:
                    OptionChosen(ChoiceOption.Yes);
                    e.Handled = true;
                    break;

                case Key.N:
                    OptionChosen(ChoiceOption.No);
                    e.Handled = true;
                    break;

                case Key.Enter:
                    OptionChosen(ChoiceOption.Ok);
                    e.Handled = true;
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        private void LoggerItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer =
                 VisualTreeHelper.GetChild(LoggerItemsControl, 0) as ScrollViewer;

            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            scrollViewer.Focus();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange < 0 && e.ViewportHeightChange == 0)
            {
                // Stop auto scroll on manual offset change
                autoScrollEnabled = false;
            }
            else if (!autoScrollEnabled && e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
            {
                // Resume auto scroll on manual offset change at the end of the extent
                autoScrollEnabled = true;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
            }
        }

        private void PromptYesButton_Click(object sender, RoutedEventArgs e)
        {
            OptionChosen(ChoiceOption.Yes);
        }

        private void PromptNoButton_Click(object sender, RoutedEventArgs e)
        {
            OptionChosen(ChoiceOption.No);
        }

        private void PromptOkButton_Click(object sender, RoutedEventArgs e)
        {
            OptionChosen(ChoiceOption.Ok);
        }

        private void PromptCancelButton_Click(object sender, RoutedEventArgs e)
        {
            OptionChosen(ChoiceOption.Cancel);
        }

        private void LoggerItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (LoggerItemsControl.IsLoaded)
            {
                var scrollViewer =
                    VisualTreeHelper.GetChild(LoggerItemsControl, 0) as ScrollViewer;

                if (autoScrollEnabled)
                    scrollViewer.ScrollToEnd();
            }
        }
    }
}
