namespace Blauhaus.MVVM.Maui.Styling.Extensions;

public static class ActivityIndicatorExtensions
{
    public static T AsPrimary<T>(this T control) where T : ActivityIndicator
    {
        control.Color = AppTheme.Colours.Primary;
        return control;
    }
}