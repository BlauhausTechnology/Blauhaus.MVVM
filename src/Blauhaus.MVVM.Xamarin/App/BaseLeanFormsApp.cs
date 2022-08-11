using System;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.App
{
    public abstract class BaseLeanFormsApp : Application  
    {
        protected IBuildConfig CurrentBuildConfig = null!;
        private AppLifecycleService _appLifeCycleService = null!;
        protected IAnalyticsLogger<BaseLeanFormsApp> Logger = null!;
        private IDisposable? _analyticsSession;
        private bool _isSleeping;

        protected BaseLeanFormsApp(IServiceCollection? platformServices)
        {
             Initialize(platformServices);
        }

        private void Initialize(IServiceCollection? platformServices)
        {
            var services = new ServiceCollection();
            CurrentBuildConfig = GetBuildConfig();
            services.AddSingleton(CurrentBuildConfig);

            ConfigureServices(services);

            services.AddSingleton<IServiceLocator, DotNetCoreServiceLocator>();
            services.AddSingleton<IAppLifecycleService, AppLifecycleService>();
            
            //do this last to give platform services a chance to override defaults
            if(platformServices!= null )
            {
                foreach (var platformService in platformServices)
                {
                    services.Add(platformService);
                }
            }

            var serviceProvider = services.BuildServiceProvider();
            
            AppServiceLocator.Initialize(()=> serviceProvider.GetRequiredService<IServiceLocator>());
            _appLifeCycleService = (AppLifecycleService) serviceProvider.GetRequiredService<IAppLifecycleService>();
            Logger = serviceProvider.GetRequiredService<IAnalyticsLogger<BaseLeanFormsApp>>();
        }

        protected sealed override void OnStart()
        {
            Logger.SetValue("AppSessionId", Guid.NewGuid());
            _analyticsSession = Logger.BeginTimedScope(LogLevel.Information, "App Session");
            Logger.LogTrace("Application starting");

            _appLifeCycleService.NotifyAppStarting();

            HandleAppStarting();
        }
        protected virtual void HandleAppStarting()
        {
        }

        protected sealed override void OnSleep()
        {
            base.OnSleep();
            
            Logger.LogTrace("Application going to sleep");
        
            HandleAppGoingToSleep();

            _isSleeping = true;
            _appLifeCycleService.NotifyAppGoingToSleep();
            _analyticsSession?.Dispose();

        }
        protected virtual void HandleAppGoingToSleep()
        {
        }
        
        protected sealed override void OnResume()
        {
            if (_isSleeping)
            {
                Logger.SetValue("AppSessionId", Guid.NewGuid());
                _analyticsSession = Logger.BeginTimedScope(LogLevel.Information, "App Session");
                Logger.LogTrace("Application waking up");
        
                _appLifeCycleService.NotifyAppWakingUp();
                HandleAppWakingUp();

                _isSleeping = false;
            }

        }
        
        protected virtual void HandleAppWakingUp()
        {
        }

        protected abstract IBuildConfig GetBuildConfig();

        protected abstract void ConfigureServices(IServiceCollection services);

    }
}