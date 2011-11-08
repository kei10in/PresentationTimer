using System;
using System.Windows.Data;
using System.Globalization;

namespace PresentationTimer
{
    class SimpleTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture) {
            TimeSpan time = (TimeSpan) value;
            return time.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
}
