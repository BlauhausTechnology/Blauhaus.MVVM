using Blauhaus.MVVM.Abstractions.ViewModels;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class ActivityIndicatorExtensions
    {
        public static ActivityIndicator BindIsRunningToIsExecuting(this ActivityIndicator activityIndicator, string propertyName)  
        {
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding($"{propertyName}.{nameof(IExecuting.IsExecuting)}"));
            return activityIndicator;
        }
    }
}