namespace Blauhaus.MVVM.Maui.Extensions.SetterExtensions;

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

    public static T WithVerticalAlignment<T>(this T control, LayoutOptions layoutOptions) where T : View
    {
        control.VerticalOptions = layoutOptions;
        return control;
    }

    public static T WithHorizontalAlignment<T>(this T control, LayoutOptions layoutOptions) where T : View
    {
        control.HorizontalOptions = layoutOptions;
        return control;
    }
}