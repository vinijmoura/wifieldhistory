using System;
using System.Windows;
using System.Windows.Controls;

namespace Lambda3.WorkItemFieldHistory.Views.Controls
{
    class PageSource : DependencyObject
    {
        const string pageTemplate = "<html><head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head><body>{0}</p></body></html>";


        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html",
                typeof(string),
                typeof(PageSource),
                new PropertyMetadata(String.Empty, (s, a) => RenderHtml(s as WebBrowser, a.NewValue as string)));

        private static void RenderHtml(WebBrowser browser, string html)
        {
            browser.NavigateToString(string.Format(pageTemplate, html));
        }
    }
}
