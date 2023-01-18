using System.Globalization;

namespace Blauhaus.MVVM.Maui.Converters
{
    public class FuncConverter<TParameter> : IValueConverter
    {
        private readonly Func<TParameter, object> _func;

        public FuncConverter(Func<TParameter, object> func)
        {
            _func = func;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actionParameter = (TParameter)value;
            return _func?.Invoke(actionParameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}