using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Lambda3.WorkItemFieldHistory.Views.Controls
{
    public class Fallback : DependencyObject
    {
        public static int GetFallbackItemIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(FallbackItemIndexProperty);
        }

        public static void SetFallbackItemIndex(DependencyObject obj, int value)
        {
            obj.SetValue(FallbackItemIndexProperty, value);
        }

        public static readonly DependencyProperty FallbackItemIndexProperty =
            DependencyProperty.RegisterAttached("FallbackItemIndex",
                typeof(int),
                typeof(Fallback),
                new PropertyMetadata(-1,
                                     new PropertyChangedCallback(FallbackItemIndex_PropertyChangedCallback)));

        private static void FallbackItemIndex_PropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is Selector && (int)args.NewValue >= 0)
            {
                (sender as Selector).SelectionChanged += (s, e) =>
                {
                    if (e.AddedItems.Count == 0)
                        (sender as Selector).SelectedIndex = 0;
                };
            }
        }
    }
}
