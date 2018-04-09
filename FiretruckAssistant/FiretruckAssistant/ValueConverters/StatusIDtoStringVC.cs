using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FiretruckAssistant.ValueConverters
{
    class StatusIDtoStringVC : IValueConverter
    {
        string[] statusMessage = new string[]
        { 
            null,
            "Urgent",
            "Processing",
            "Finished"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return statusMessage[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
