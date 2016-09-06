using System;
using System.Windows;
using System.Windows.Controls;

namespace BloggersPoint.UI.Services
{
    public class WebBrowserService
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(WebBrowserService), new UIPropertyMetadata(null, SourcePropertyChanged));

        public static string GetSource(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(SourceProperty, value);
        }

        public static void SourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = dependencyObject as WebBrowser;

            if (browser == null)
                return;
            
                string uri = e.NewValue as string;
                browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            
        }

    }
}
