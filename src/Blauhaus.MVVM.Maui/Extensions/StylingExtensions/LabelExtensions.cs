using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;
public static class LabelExtensions
{
    
    public static T AsDefault<T>(this T control) where T : Label
    {
        control.FontSize = Fonts.DefaultFontSize;
        control.FontFamily = Fonts.DefaultFontFamily;
        control.CharacterSpacing = Fonts.DefaultCharacterSpacing;
        return control;
    }
    public static T AsOnBackground<T>(this T control) where T : Label
    {
        control.AsDefault();
        control.TextColor = Colours.OnBackground;
        return control;
    }
    
    public static T AsPrimary<T>(this T control) where T : Label
    {
        control.AsDefault();
        control.TextColor = Colours.Primary;
        return control;
    }
     
}