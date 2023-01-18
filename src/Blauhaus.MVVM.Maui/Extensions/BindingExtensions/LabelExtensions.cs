namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class LabelExtensions
{
    public static T BindText<T>(this T control, string propertyName) where T : Label
    {
        control.SetBinding(Label.TextProperty, $"{propertyName}", BindingMode.OneWay);
        return control;
    }
}