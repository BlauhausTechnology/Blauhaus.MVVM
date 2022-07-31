using Blauhaus.MVVM.Abstractions.TargetNavigation;
using IView = Blauhaus.MVVM.Abstractions.Views.IView;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiNavigator : IPlatformNavigator
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
        if(view is not Page mauiPage) throw new InvalidOperationException($"View {view.GetType().Name} is not a valid Maui page type");
        Application.Current.MainPage = mauiPage;
    }
}