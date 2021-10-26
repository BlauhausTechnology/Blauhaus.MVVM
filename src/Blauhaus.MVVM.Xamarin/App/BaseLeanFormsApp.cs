using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.App
{
    public abstract class BaseLeanFormsApp : Application  
    {
        protected IBuildConfig CurrentBuildConfig = null!;
        private AppLifecycleService _appLifeCycleService = null!;

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