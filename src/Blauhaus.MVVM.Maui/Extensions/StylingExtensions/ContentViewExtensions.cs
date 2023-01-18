namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class ContentViewExtensions
{
    public static T WithContent<T>(this T control, View view) where T : ContentView
    {
        control.Content = view;
        return control;
    }


}