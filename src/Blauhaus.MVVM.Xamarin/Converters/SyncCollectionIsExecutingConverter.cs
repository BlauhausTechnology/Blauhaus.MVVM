//using System;
//using System.Globalization;
//using Blauhaus.Domain.Abstractions.Sync;
//using Blauhaus.MVVM.Xamarin.Extensions;
//using Xamarin.Forms;

//namespace Blauhaus.MVVM.Xamarin.Converters
//{
//    public class SyncCollectionIsRunningConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var state = (SyncClientState) value;
//            return state.IsExecuting();
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value;
//        }
//    }
//}