using Blauhaus.Analytics.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Maui.MauiApplication;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.Navigator;

public class MauiNavigator : INavigator
{
    private readonly IAnalyticsLogger<MauiNavigator> _logger;
    private readonly IServiceLocator _serviceLocator;
    private readonly INavigationRegister _navigationRegister;
    private readonly IMauiApplication _mauiApplication;

    public MauiNavigator(
        IAnalyticsLogger<MauiNavigator> logger,
        IServiceLocator serviceLocator,
        INavigationRegister navigationRegister,
        IMauiApplication mauiApplication)
    {
        _logger = logger;
        _serviceLocator = serviceLocator;
        _navigationRegister = navigationRegister;
        _mauiApplication = mauiApplication;
    }

    public async Task NavigateAsync(NavigationTarget target)
    {
        if (target.Path.Length == 0)
        {
            throw new InvalidOperationException("Navigation path cannot be empty");
        }
        if (target.Path.Length == 1)
        {
            var page = ConstructPage<Page>(target.Path[0]);
            _mauiApplication.AddShellContent(page);
            //_mauiApplication.SetMainPage(page);
        }

    }

    public async Task NavigateBackAsync()
    {
        throw new NotImplementedException();
    }

    private TPage ConstructPage<TPage>(ViewIdentifier viewIdentifier) where TPage : Page
    {
        var viewType = _navigationRegister.GetViewType(viewIdentifier);
        if (viewType is null)
        {
            _logger.LogWarning("View Identifier {ViewIdentifier} does not have a registered view type", viewIdentifier.ToString());
            throw new InvalidOperationException("Invalid page configuration");
        }
        var view = _serviceLocator.Resolve(viewType);
        if (view is not TPage page)
        {
            _logger.LogWarning("View Type {ViewType} is not the required type {RequiredViewType}", view.GetType().Name, typeof(TPage).Name);
            throw new InvalidOperationException("Invalid page configuration");
        }

        return page;
    }
}