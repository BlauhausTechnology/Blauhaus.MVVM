namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class ItemsViewExtensions
{
    public static T WithDataTemplate<T>(this T control, Type type) where T : ItemsView
    {
        control.ItemTemplate = new DataTemplate(type);
        return control;
    }
    
    public static T WithDataTemplate<T>(this T control, Func<IView> func) where T : ItemsView
    {
        control.ItemTemplate = new DataTemplate(func);
        return control;
    }
    
    public static T BindItemsSource<T>(this T control, string name) where T : ItemsView
    {
        control.SetBinding(ItemsView.ItemsSourceProperty, new Binding(name));
        return control;
    }
      

}