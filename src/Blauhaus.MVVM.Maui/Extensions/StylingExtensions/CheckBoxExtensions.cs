using Blauhaus.MVVM.Maui.Styling;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class CheckBoxExtensions
{
    public static T AsOnBackground<T>(this T control) where T : CheckBox
    {
        control.BackgroundColor = AppStyling.Colours.Background;
        control.Color = AppStyling.Colours.OnBackground;
        return control;
    }
}