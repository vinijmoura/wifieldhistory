using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace Lambda3.WorkItemFieldHistory.Views.Converters
{
    class HtmlConverter : IValueConverter
    {
        private const char newLine = '\n';
        private const char space = ' ';

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return value.ToString().Replace(newLine, space);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
