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

namespace Patcher.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<LogItem> logItems = new ObservableCollection<LogItem>();

        public bool TerminateOnKey { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = logItems;

            AppLabel.Content = Program.GetProgramVersionInfo();

            logItems.CollectionChanged += LogItems_CollectionChanged;
        }

        private void LogItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (LoggerItemsControl.IsLoaded)
            {
                var scrollViewer =
                    VisualTreeHelper.GetChild(LoggerItemsControl, 0) as ScrollViewer;
                
                scrollViewer.ScrollToEnd();
            }
        }

        internal void WriteMessage(LogEntry entry)
        {
            string text = entry.Text;
            switch (entry.Level)
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
                StatusLabel.Content = text;
                StatusLabel.Foreground = brush;
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (TerminateOnKey)
            {
                Close();
            }
        }
    }
}
