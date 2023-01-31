namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;
public static class LabelExtensions
{

    public static T WithText<T>(this T control, string text) where T : Label
    {
        control.Text = text;
        return control;
    }

    public static T AsPrimary<T>(this T control) where T : Label
    {
        control.TextColor = Styling.AppStyling.Colours.Primary;
        return control.AsDefault();
    }
    
    public static T WithFontBold<T>(this T control) where T : Label
    {
        control.FontAttributes = FontAttributes.Bold;
        return control.AsDefault();
    }

    public static T AsDefault<T>(this T control) where T : Label
    {
        control.FontSize = 14;
        control.FontFamily = Styling.AppStyling.Fonts.DefaultFontFamily;
        control.CharacterSpacing = Styling.AppStyling.Fonts.DefaultCharacterSpacing;
        return control;
    }
}