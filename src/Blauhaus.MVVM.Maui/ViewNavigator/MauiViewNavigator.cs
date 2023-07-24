using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Maui.Applications;
using Blauhaus.MVVM.Maui.ViewNavigator.Containers;
using Blauhaus.Responses;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.ViewNavigator;

public class MauiViewNavigator : IMauiViewNavigator
{
    private readonly IAnalyticsLogger<MauiViewNavigator> _logger;
    private readonly IMauiApplicationProxy _mauiApplicationProxy;
    private readonly IMauiViewFactory _viewFactory;

    private Dictionary<ViewIdentifier, IMauiViewContainer> _currentNavigationPages = new();
    private IMauiViewContainer? _activeContainer;

    public MauiViewNavigator(
        IAnalyticsLogger<MauiViewNavigator> logger,
        IMauiApplicationProxy mauiApplicationProxy,
        IMauiViewFactory viewFactory)
    {
        _logger = logger;
        _mauiApplicationProxy = mauiApplicationProxy;
        _viewFactory = viewFactory;
    }

    public async Task NavigateAsync(IViewTarget viewTarget)
    {
        if (viewTarget.Count == 1)
        {
            var page = await _viewFactory.GetViewAsync(viewTarget[0]);
            if (_activeContainer is null)
            {
                _logger.LogDebug("There is no active navigation container - setting application main page to {PageType} with identifier {ViewIdentifier}", page.GetType().Name, viewTarget[0].Serialize());
                _mauiApplicationProxy.SetMainPage(page);
            }
            else
            {
                _logger.LogDebug("Passing {PageType} with identifier {ViewIdentifier} to container {ViewContainerType}", page.GetType().Name, viewTarget[0].Serialize(), _activeContainer.GetType().Name);
                await _activeContainer.PushAsync(page);
            }
        }

        else
        {
            throw new NotImplementedException("Only 1 view identifier supported so far");
        }
         
    }
    
    public void SetActive(IMauiViewContainer container)
    {
        _activeContainer = container;
    }

    public Task GoBackAsync()
    {
        return _activeContainer is not null ? _activeContainer.PopAsync() : Task.CompletedTask;
    }

}