using System;
using System.Windows;

namespace BloggersPoint.UI.Services
{
    public class MessageService: IMessageService
    {
        private const string InformationCaption = "Information";
        private const string WarningCaption = "Warning";
        private const string ErrorCaption = "Error";

        public void ShowInfoMessage(string text)
        {
            MessageBox.Show(text, InformationCaption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarningMessage(string text)
        {
            MessageBox.Show(text, WarningCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public void ShowErrorMessage(string text)
        {
            MessageBox.Show(text, ErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
    }
}
