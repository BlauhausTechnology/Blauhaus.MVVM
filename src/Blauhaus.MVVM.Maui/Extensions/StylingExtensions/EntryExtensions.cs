using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class EntryExtensions
{

    public static T AsOnBackground<T>(this T control) where T : Entry
    {
        control.FontFamily = Fonts.DefaultFontFamily;
        control.BackgroundColor = Colours.BackgroundFaded;
        control.TextColor = Colours.OnBackground;
        control.PlaceholderColor = Colours.OnBackgroundFaded;
        return control;
    }


    public static T AsNoPredictiveText<T>(this T control) where T : Entry
    {
        control.IsTextPredictionEnabled = false;
        control.IsSpellCheckEnabled = false;
        return control;
    }
    public static T AsPassword<T>(this T control) where T : Entry
    {
        control.IsPassword = true;
        return control;
    }
}