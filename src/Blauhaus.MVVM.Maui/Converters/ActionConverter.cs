using System.Globalization;

namespace Blauhaus.MVVM.Maui.Converters
{
    public class ActionConverter : IValueConverter
    {
        private readonly Action _action;

        public ActionConverter(Action action)
        {
            _action = action;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _action?.Invoke();
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    
    public class ActionConverter<TParameter> : IValueConverter
    {
        private readonly Action<TParameter> _action;

        public ActionConverter(Action<TParameter> action)
        {
            _action = action;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actionParameter = (TParameter)value;
            _action?.Invoke(actionParameter);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}