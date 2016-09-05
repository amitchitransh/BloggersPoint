using BloggersPoint.Services;
using System.Windows;
using System.Windows.Threading;
using BloggersPoint.Core.Services;

namespace BloggersPoint.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            IMesaageService messageBoxService = new MessageService();
            messageBoxService.ShowErrorMessage("An unhandled exception occurred: " + e.Exception.Message);
            Current.Shutdown();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            CachingService.Current.FlushCachedData();
        }
    }
}
