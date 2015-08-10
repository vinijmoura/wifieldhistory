using System;
using System.Windows;

namespace Lambda3.WorkItemFieldHistory.Extensions
{
    static class ExceptionExtensions
    {
        public static void Show(this Exception exception, string title = "Work Item Field History")
        {
            MessageBox.Show(exception.Message,
                            title,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.OK,
                            MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
