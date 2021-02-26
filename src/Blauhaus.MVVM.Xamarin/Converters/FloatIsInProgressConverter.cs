using System;
using System.Globalization;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Converters
{
    
    public class FloatIsInProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float progress)
            {
                var isInprogress = progress > 0 && progress < 1;
                return isInprogress;
            }
            if (value is double dprogress)
            {
                return dprogress > 0 || dprogress < 1;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}