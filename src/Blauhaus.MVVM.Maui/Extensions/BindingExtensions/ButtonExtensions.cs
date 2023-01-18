namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class ButtonExtensions
{
    public static T BindCommand<T>(this T control, string name) where T : Button
    {
        control.SetBinding(Button.CommandProperty, new Binding(name));
        return control;
    }
}