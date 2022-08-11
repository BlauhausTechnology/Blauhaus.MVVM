using System;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Navigation;

public class FormsNavigator : IPlatformNavigator
{
    public IView? GetCurrentMainView()
    {
        if (Application.Current is null) return null;
        var mainPage = Application.Current.MainPage;
        if (mainPage is IView view) return view;
        return null;
    }

    public void SetCurrentMainView(IView view)
    {
        if (Application.Current is null) throw new InvalidOperationException("Cannot set application main page because current application is null");
        if (view is not Page page) throw new InvalidOperationException($"View {view.GetType().Name} is not a valid Xamarin Forms Page type");
        Application.Current.MainPage = page;
    }
}