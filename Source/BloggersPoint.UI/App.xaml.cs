using BloggersPoint.Services;
using System.Windows;
using System.Windows.Threading;

namespace BloggersPoint.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            IMesaageService messageBoxService = new MessageService();
            messageBoxService.ShowErrorMessage("An unhandled exception occurred: " + e.Exception.Message);
            Current.Shutdown();
        }
    }
}
