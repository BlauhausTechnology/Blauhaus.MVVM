namespace Blauhaus.MVVM.Maui.Extensions.SetterExtensions;

public static class StackExtensions
{
    public static T WithSpacing<T>(this T control, double spacing) where T : StackBase
    {
        control.Spacing = spacing;
        return control;
    }
}