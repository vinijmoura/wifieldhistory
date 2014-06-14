﻿using System;
using System.Windows;

namespace Lambda3.WorkItemFieldHistory.Extensions
{
    static class ExceptionExtensions
    {
        public static void Show(this Exception exception, string title)
        {
            MessageBox.Show(exception.Message,
                            title,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.OK,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}