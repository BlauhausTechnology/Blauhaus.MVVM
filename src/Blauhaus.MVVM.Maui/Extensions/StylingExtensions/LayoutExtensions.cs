namespace Blauhaus.MVVM.Maui.Extensions.StylingExtensions;

public static class LayoutExtensions
{
    public static T WithChildren<T>(this T control, params IView[] views) where T : Layout
    {
        foreach (var view in views)
        {
            control.Add(view);
        }
        return control;
    }

    public static T WithPadding<T>(this T control, double thickness) where T : Layout
    {
        control.Padding = thickness;
        return control;
    }
    
    public static T WithPadding<T>(this T control, double left, double top, double right, double borrom) where T : Layout
    {
        control.Padding = new Thickness(left, top, right, borrom);
        return control;
    }
}