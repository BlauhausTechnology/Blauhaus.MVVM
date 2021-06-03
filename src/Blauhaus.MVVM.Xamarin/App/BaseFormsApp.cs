using System.IO;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.App
{

    public abstract class BaseFormsApp : BaseFormsApp<DotNetCoreServiceLocator> 
    {
        protected BaseFormsApp(IServiceCollection? platformServices) : base(platformServices)
        {
        }
    }

    //todo try using just the ServiceProvider without the HostBuilder to improve startup time
    //https://github.com/dotnet/maui/issues/880

    public abstract class BaseFormsApp<TServiceLocator> : Application where TServiceLocator : class, IServiceLocator
    {
        protected IBuildConfig CurrentBuildConfig = null!;
        private readonly AppLifecycleService _appLifeCycleService;

        protected BaseFormsApp(IServiceCollection? platformServices)
        {
            platformServices ??= new ServiceCollection();

            var serviceProvider = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(x => x.ClearProviders())
                .ConfigureServices((services) =>
                {
                    CurrentBuildConfig = GetBuildConfig();
                    services.AddSingleton(CurrentBuildConfig);

                    ConfigureServices(services);

                    services.AddSingleton<IServiceLocator, TServiceLocator>();
                    services.AddSingleton<IAppLifecycleService, AppLifecycleService>();
                    
                    //do this last to give platform services a chance to override defaults
                    foreach (var platformService in platformServices)
                    {
                        services.Add(platformService);
                    }

                }).Build().Services;

            AppServiceLocator.Initialize(serviceProvider.GetRequiredService<IServiceLocator>());

            _appLifeCycleService = (AppLifecycleService) AppServiceLocator.Resolve<IAppLifecycleService>();

        }

        protected sealed override void OnStart()
        {
            _appLifeCycleService.NotifyAppStarting();
            HandleAppStarting();
        }
        protected virtual void HandleAppStarting()
        {
        }

        protected sealed override void OnSleep()
        {
            _appLifeCycleService.NotifyAppGoingToSleep();
            HandleAppGoingToSleep();
        }
        protected virtual void HandleAppGoingToSleep()
        {
        }
        
        protected sealed override void OnResume()
        {
            _appLifeCycleService.NotifyAppWakingUp();
            HandleAppWakingUp();
        }
        
        protected virtual void HandleAppWakingUp()
        {
        }

        protected abstract IBuildConfig GetBuildConfig();

        protected abstract void ConfigureServices(IServiceCollection services);

    }
}