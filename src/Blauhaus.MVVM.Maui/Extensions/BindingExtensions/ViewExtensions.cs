namespace Blauhaus.MVVM.Maui.Extensions.BindingExtensions;

public static class ViewExtensions
{ 
    public static T BindTapGesture<T>(this T control, string propertyName) where T : View
    {
        var tgr = new TapGestureRecognizer();
        tgr.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(propertyName));
        control.GestureRecognizers.Add(tgr);
        return control;
    }
}