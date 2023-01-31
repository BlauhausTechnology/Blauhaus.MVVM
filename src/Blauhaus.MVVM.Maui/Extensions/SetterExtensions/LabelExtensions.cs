using static Blauhaus.MVVM.Maui.Styling.AppStyling;

namespace Blauhaus.MVVM.Maui.Extensions.SetterExtensions;
public static class LabelExtensions
{
     
    public static T WithText<T>(this T control, string text) where T : Label
    {
        control.Text = text;
        return control;
    }

    public static T WithFontBold<T>(this T control) where T : Label
    {
        control.FontAttributes = FontAttributes.Bold;
        return control;
    }

    public static T WithVerticalTextAlignment<T>(this T control, TextAlignment alignment) where T : Label
    {
        control.VerticalTextAlignment = alignment;
        return control;
    }
    public static T WithHorizontalTextAlignment<T>(this T control, TextAlignment alignment) where T : Label
    {
        control.HorizontalTextAlignment = alignment;
        return control;
    }
}