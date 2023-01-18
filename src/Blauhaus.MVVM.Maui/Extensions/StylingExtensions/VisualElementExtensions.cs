namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class VisualElementExtensions
{
    
    public static T WithWidth<T>(this T control, double width) where T : VisualElement
    {
        control.WidthRequest = width;
        return control;
    }
    public static T WithHeight<T>(this T control, double height) where T : VisualElement
    {
        control.HeightRequest = height;
        return control;
    }
}