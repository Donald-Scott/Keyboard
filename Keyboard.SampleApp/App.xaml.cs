using System.Diagnostics;
using System.Windows;

namespace Keyboard.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            PresentationTraceSources.Refresh();

            PresentationTraceSources.RoutedEventSource.Listeners.Add(new DefaultTraceListener());

            PresentationTraceSources.RoutedEventSource.Switch.Level = SourceLevels.Off;

            MainWindow app = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            app.Show();
        }
    }
}
