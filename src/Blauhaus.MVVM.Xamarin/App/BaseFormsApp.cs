using System;
using System.IO;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.App
{
    public abstract class BaseFormsApp<TStartupPage> : Application 
        where TStartupPage : Page, IView
    {
        protected IBuildConfig CurrentBuildConfig = null!;

        protected BaseFormsApp(IServiceCollection? platformServices, Action<Page>? mainPageHandler = null)
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
                    
                    //do this last to give platform services a chance to override defaults
                    foreach (var platformService in platformServices)
                    {
                        services.Add(platformService);
                    }

                }).Build().Services;
             

            AppServiceLocator.Initialize(serviceProvider.GetRequiredService<IServiceLocator>());

            if (mainPageHandler == null)
            {
                MainPage = AppServiceLocator.Resolve<TStartupPage>();
            }
            else
            {
                mainPageHandler.Invoke(MainPage);
            }

        }

        protected override void OnStart()
        {
        }

        protected abstract IBuildConfig GetBuildConfig();

        protected abstract void ConfigureServices(IServiceCollection services);

    }
}