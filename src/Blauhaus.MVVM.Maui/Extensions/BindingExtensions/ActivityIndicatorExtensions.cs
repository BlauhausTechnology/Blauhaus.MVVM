namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class ActivityIndicatorExtensions
{
    public static T BindIsRunning<T>(this T control, string name) where T : ActivityIndicator
    {
        control.SetBinding(ActivityIndicator.IsRunningProperty, new Binding(name));
        return control;
    }
}