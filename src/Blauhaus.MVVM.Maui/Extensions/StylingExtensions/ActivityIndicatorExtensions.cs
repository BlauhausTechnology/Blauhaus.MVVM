using static Blauhaus.MVVM.Maui.Styling.AppVisuals;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class ActivityIndicatorExtensions
{
    public static T AsPrimary<T>(this T control) where T : ActivityIndicator
    {
        control.Color = Colours.Primary;
        return control;
    }
}