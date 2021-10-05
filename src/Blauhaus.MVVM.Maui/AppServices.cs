using System;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.Maui
{ 
    public abstract class BaseMauiServices
    {
        protected IBuildConfig CurrentBuildConfig = null!;
        private AppLifecycleService _appLifeCycleService;


        public void Initialize(IServiceCollection platformServices)
        {
            platformServices ??= new ServiceCollection();
            var services = new ServiceCollection();
            
            CurrentBuildConfig = GetBuildConfig();
            services.AddSingleton(CurrentBuildConfig);
            ConfigureServices(services);

            services.AddSingleton<IServiceLocator, DotNetCoreServiceLocator>();
            services.AddSingleton<IAppLifecycleService, AppLifecycleService>();


            //do this last to give platform services a chance to override defaults
            foreach (var platformService in platformServices)
            {
                services.Add(platformService);
            }

            var serviceProvider = services.BuildServiceProvider();
            
            AppServiceLocator.Initialize(()=> serviceProvider.GetRequiredService<IServiceLocator>());
            _appLifeCycleService = (AppLifecycleService) serviceProvider.GetRequiredService<IAppLifecycleService>();
        }

        
        protected abstract IBuildConfig GetBuildConfig();

        protected abstract void ConfigureServices(IServiceCollection services);
    }
}