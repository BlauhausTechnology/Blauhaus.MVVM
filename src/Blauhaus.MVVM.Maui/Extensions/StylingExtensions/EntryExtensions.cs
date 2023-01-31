using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class EntryExtensions
{

    public static T AsOnBackground<T>(this T control) where T : Entry
    {
        control.FontFamily = Fonts.DefaultFontFamily;
        control.BackgroundColor = Colours.BackgroundOffset;
        control.TextColor = Colours.OnBackground;
        control.PlaceholderColor = Colours.OnBackgroundFaded;
        return control;
    }


    public static T WithPlaceholder<T>(this T control, string placeHolder) where T : Entry
    {
        control.Placeholder = placeHolder;
        return control;
    }
    public static T WithKeyboard<T>(this T control, KeyboardFlags keyboards) where T : Entry
    {
        control.Keyboard = Keyboard.Create(keyboards);
        return control;
    }
    public static T WithKeyboard<T>(this T control, Keyboard keyboard) where T : Entry
    {
        control.Keyboard = keyboard;
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