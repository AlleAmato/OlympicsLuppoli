using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OlympicsStatistics.Helpers
{
    public class MedalsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int -> string
            //if (targetType != typeof(string)) throw new InvalidOperationException();
            int n = (int)value;
            return n <= 0 ? "" : n.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
