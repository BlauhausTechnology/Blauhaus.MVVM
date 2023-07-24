using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Application;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.MVVM.Abstractions.Application;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.AppLifecycle;
using Blauhaus.MVVM.Blazor.DummyServices;
using Blauhaus.MVVM.Blazor.Services;
using Blauhaus.MVVM.Blazor.ViewNavigator;
using Blauhaus.MVVM.Ioc;
using Blauhaus.Push.Abstractions.Client;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.Blazor.Ioc;

public static class ServiceCollectionExtensions
{
    
    private static IServiceCollection AddBlazorViewNavigator(this IServiceCollection services)
    {
        services
            .AddSingleton<IViewNavigator, BlazorViewNavigator>()
            .AddViewRegister();

        return services;
    }

    public static IServiceCollection AddBlazorServerDeviceServices<TKeyValueStore>(this IServiceCollection services) where TKeyValueStore : class, IKeyValueStore
    {
        services
            .AddScoped<IDeviceInfoService, BlazorDeviceInfoService>()
            .AddScoped<IKeyValueStore, TKeyValueStore>();

        return services;
    }


    public static IServiceCollection AddBlazorServerMvvm(this IServiceCollection services)
    {
        //Infrastructure
        services
            .AddScoped<IServiceLocator>(sp => new DotNetCoreServiceLocator(sp));

        //MVVM services
        services
            .AddScoped<IAppLifecycleService, AppLifecycleService>()
            .AddScoped<IViewNavigator, BlazorViewNavigator>()
            .AddExecutingCommands()
            .AddScoped<IErrorHandler, BlazorErrorHandler>();

        
        //Device services
        services           
            .AddScoped<IApplicationInfoService, BlazorApplicationInfoService>()
            .AddScoped<IConnectivityService, BlazorConnectivityService>()
            .AddScoped<IThreadService, BlazorThreadService>()
            .AddScoped<IPushNotificationsClientService, DummyPushNotificationsClientService>();

        return services;
    }

    public static IServiceCollection AddBlazorWebAssemblyDeviceServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IDeviceInfoService, BlazorDeviceInfoService>()
            .AddSingleton<IKeyValueStore, BlazorWebAssemblyKeyValueStore>();

        return services;
    }

public static IServiceCollection AddBlazorWebAssemblyMvvm(this IServiceCollection services)
    {
        
        //Infrastructure
        services
            .AddSingleton<IServiceLocator>(sp => new DotNetCoreServiceLocator(sp));

        //External dependencies
        services
            .AddBlazoredLocalStorageAsSingleton();
        
        //MVVM services
        services
            .AddSingleton<IAppLifecycleService, AppLifecycleService>()
            .AddExecutingCommands()
            .AddScoped<IErrorHandler, BlazorErrorHandler>();
        
        //Device services
        services           
            .AddSingleton<IApplicationInfoService, BlazorApplicationInfoService>()
            .AddSingleton<IConnectivityService, BlazorConnectivityService>()
            .AddSingleton<IThreadService, BlazorThreadService>()
            .AddSingleton<IPushNotificationsClientService, DummyPushNotificationsClientService>();

        return services;
    }
     
}