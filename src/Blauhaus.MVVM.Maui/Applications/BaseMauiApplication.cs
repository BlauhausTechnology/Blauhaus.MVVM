using Blauhaus.Analytics.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.AppLifecycle;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.Applications;

public abstract class BaseMauiApplication : Application
{
    private IDisposable? _analyticsSession;
    private bool _isSleeping;
    
    private readonly AppLifecycleService _appLifecycleService;

    protected readonly IServiceLocator ServiceLocator;
    protected readonly IAnalyticsLogger Logger;
    protected readonly INavigator Navigator;

    protected BaseMauiApplication(
        IServiceLocator serviceLocator,
        IAnalyticsLogger logger, 
        INavigator navigator)
    {
        ServiceLocator = serviceLocator;
        Logger = logger;
        Navigator = navigator;
        _appLifecycleService = ServiceLocator.ResolveAs<AppLifecycleService>(typeof(IAppLifecycleService));
        MainPage = new ContentPage();
    }

    protected sealed override async void OnStart()
    {
        base.OnStart();
        
        Logger.SetValue("AppSessionId", Guid.NewGuid());
        _analyticsSession = Logger.BeginTimedScope(LogLevel.Information, "App Session");
        Logger.LogTrace("Application starting");

        _appLifecycleService.NotifyAppStarting();
        
        await HandleStartingAsync();
    }

    protected sealed override async void OnSleep()
    {
        base.OnSleep();

        Logger.LogTrace("Application going to sleep");
        
        _isSleeping = true;
        _appLifecycleService.NotifyAppGoingToSleep();
        await HandleGoingToSleepAsync();

        _analyticsSession?.Dispose();
    }

    protected sealed override async void OnResume()
    {
        base.OnResume();
        
        if (_isSleeping)
        {
            Logger.SetValue("AppSessionId", Guid.NewGuid());
            _analyticsSession = Logger.BeginTimedScope(LogLevel.Information, "App Session");
            Logger.LogTrace("Application waking up");
        
            await HandleWakingUpAsync();
            _appLifecycleService.NotifyAppWakingUp();
            _isSleeping = false;
        }
    }

    protected abstract Task HandleStartingAsync();
    protected virtual Task HandleGoingToSleepAsync() => Task.CompletedTask;
    protected virtual Task HandleWakingUpAsync() => Task.CompletedTask;

}