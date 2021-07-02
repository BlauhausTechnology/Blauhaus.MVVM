using System;
using System.IO;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.MonoGame.Screens;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.MonoGame.Games
{
    public abstract class BaseGame<TGameScreen> : BaseScreenGame where TGameScreen : class, IGameScreen
    {
        protected IBuildConfig CurrentBuildConfig = null!;
        private AppLifecycleService? _appLifeCycleService;

        protected AppLifecycleService AppLifecycleService => _appLifeCycleService ??= (AppLifecycleService) AppServiceLocator.Resolve<IAppLifecycleService>();

        private bool _hasStarted;
        
        protected BaseGame(IServiceCollection? platformServices)
        {
            
            platformServices ??= new ServiceCollection();
            
            var serviceProvider = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(x => x.ClearProviders())
                .ConfigureServices((services) =>
                {
                    ConfigureServices(services);

                    CurrentBuildConfig = GetBuildConfig();
                    services.AddSingleton(CurrentBuildConfig);
                    services.AddSingleton<IServiceLocator, DotNetCoreServiceLocator>();
                    services.AddSingleton<IAppLifecycleService, AppLifecycleService>();
                    services.AddSingleton<IScreenGame>(this);
                    
                    //do this last to give platform services a chance to override defaults
                    foreach (var platformService in platformServices)
                    {
                        services.Add(platformService);
                    }

                }).Build().Services;
            
            AppServiceLocator.Initialize(()=> serviceProvider.GetRequiredService<IServiceLocator>());
        }

        protected override void Initialize()
        {
            base.Initialize();

            _hasStarted = true;
            AppLifecycleService.NotifyAppStarting();

            ChangeScene(AppServiceLocator.Resolve<TGameScreen>());
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            
            if (_hasStarted)
            {
                AppLifecycleService.NotifyAppWakingUp();
            }
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            AppLifecycleService.NotifyAppGoingToSleep();
        }
         
        protected abstract IBuildConfig GetBuildConfig();

        protected abstract void ConfigureServices(IServiceCollection services);

    }
}