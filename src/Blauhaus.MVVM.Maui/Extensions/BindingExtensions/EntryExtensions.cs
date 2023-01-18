namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class EntryExtensions
{
    public static T BindText<T>(this T control, string name) where T : Entry
    {
        control.SetBinding(Entry.TextProperty, name);
        return control;
    }
}