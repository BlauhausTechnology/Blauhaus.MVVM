using Blauhaus.MVVM.Maui.Controls.ButtonControls;
using static Blauhaus.MVVM.Maui.Styling.AppVisuals;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class ButtonExtensions
{
    public static T WithText<T>(this T control, string text) where T : Button
    {
        control.Text = text;
        return control;
    }

    public static T AsPrimary<T>(this T control) where T : Button
    {
        control.BackgroundColor = Styling.AppVisuals.Colours.Primary;
        control.TextColor = Colours.OnPrimary;

        if (control is DisablingButton buttonControl)
        {
            buttonControl.EnabledBackgroundColour = Colours.Primary;
            buttonControl.DisabledBackgroundColour = Colours.PrimaryFaded;
            buttonControl.EnabledTextColour = Colours.OnPrimary;
            buttonControl.DisabledTextColour = Colours.OnPrimaryFaded;
        }

        control.FontFamily = Fonts.DefaultFontFamily;
        control.CharacterSpacing = Fonts.DefaultCharacterSpacing;

        return control;
    }
    public static T AsOnBackground<T>(this T control) where T : Button
    {
        control.BackgroundColor = Colours.OnBackground;
        control.TextColor = Colours.Background;

        if (control is DisablingButton buttonControl)
        {
            buttonControl.EnabledBackgroundColour = Colours.OnBackground;
            buttonControl.EnabledTextColour = Colours.Background;
        } 

        control.FontFamily = Fonts.DefaultFontFamily;
        control.CharacterSpacing = Fonts.DefaultCharacterSpacing;

        return control;
    }

}