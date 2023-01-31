namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class CheckBoxExtensions
{
    public static T BindIsChecked<T>(this T control, string propertyName) where T : CheckBox
    {
        control.SetBinding(CheckBox.IsCheckedProperty, propertyName);
        return control;
    }
}