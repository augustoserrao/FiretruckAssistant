using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FiretruckAssistant.ValueConverters
{
    class StatusIDtoForegroundVC : IValueConverter
    {
        string[] statusForeground = new string[]
        {
            null,
            "#e60000",
            "#ffff1a",
            "#33cc33"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return statusForeground[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
