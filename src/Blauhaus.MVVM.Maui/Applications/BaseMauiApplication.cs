using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Errors;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Ioc;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.Applications;

public abstract class BaseMauiApplication<TApplication> : Application 
    where TApplication : BaseMauiApplication<TApplication>
{
    private IDisposable? _analyticsSession;
    private bool _isSleeping;
    
    private readonly AppLifecycleService _appLifecycleService;

    protected readonly IServiceLocator ServiceLocator;
    protected readonly IAnalyticsLogger<TApplication> Logger;
    protected readonly IViewNavigator ViewNavigator;

    protected BaseMauiApplication(
        IServiceLocator serviceLocator,
        IAnalyticsLogger<TApplication> logger, 
        IViewNavigator viewNavigator)
    {
        ServiceLocator = serviceLocator;
        AppServiceLocator.Initialize(serviceLocator.Resolve<IServiceLocator>);
        Logger = logger;
        ViewNavigator = viewNavigator;
        _appLifecycleService = ServiceLocator.ResolveAs<AppLifecycleService>(typeof(IAppLifecycleService));
        MainPage = new ContentPage();
    }

    protected sealed override async void OnStart()
    {
        base.OnStart();
        
        Logger.SetValue("AppSessionId", Guid.NewGuid());
        _analyticsSession = Logger.BeginTimedScope(LogLevel.Information, "App Session");
        Logger.LogTrace("Application starting");

        var startupTasks = ServiceLocator.Resolve<IEnumerable<StartupTasks>>();
        foreach (var startupTask in startupTasks)
        {
            foreach (Func<Task> task in startupTask)
            {
                try
                {
                    await task.Invoke();
                }
                catch (Exception e)
                {
                    Logger.LogError(Error.Unexpected("Startup task threw an exception"), e);
                }
            }
        }

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