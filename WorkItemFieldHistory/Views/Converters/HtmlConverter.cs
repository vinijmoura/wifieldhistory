using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace Lambda3.WorkItemFieldHistory.Views.Converters
{
    class HtmlConverter: IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return string.Empty;
            }
            string s = value.ToString();
            return s.Replace(Environment.NewLine, " ");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
