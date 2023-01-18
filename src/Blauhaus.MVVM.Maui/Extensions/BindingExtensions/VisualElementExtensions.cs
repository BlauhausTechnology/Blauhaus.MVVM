namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class VisualElementExtensions
{
    public static T BindIsVisible<T>(this T control, string propertyName) where T : VisualElement
    {
        control.SetBinding(VisualElement.IsVisibleProperty, propertyName);
        return control;
    }
}