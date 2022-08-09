using Blauhaus.Analytics.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Maui.Applications;
using Blauhaus.MVVM.Maui.TestApp.Navigation;

namespace Blauhaus.MVVM.Maui.TestApp;

public partial class TestApp
{ 
    public TestApp(
        IServiceLocator serviceLocator, 
        IAnalyticsLogger<TestApp> logger, 
        INavigator navigator) 
        : base(serviceLocator, logger, navigator)
    {
    }

    protected override async Task HandleStartingAsync()
    {
        await Navigator.NavigateAsync(NavigationTarget.CreateView(AppViews.LoadingView));
    }
}
