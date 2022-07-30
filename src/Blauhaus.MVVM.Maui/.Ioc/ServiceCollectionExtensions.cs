using System.ComponentModel;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation.UriNavigation;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Maui.Services;
using Blauhaus.MVVM.Maui.Services.Application;
using Blauhaus.MVVM.Maui.Services.Navigation;
using Blauhaus.MVVM.Maui.Services.TargetNavigation;
using Blauhaus.MVVM.Maui.Services.TargetNavigation.Register;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.Ioc;

public static class ServiceCollectionExtensions
{
    private static readonly NavigationRegister NavigationRegister = new();

    public static IServiceCollection AddMauiServices(this IServiceCollection services)
    {
        services
            .AddTransient<IErrorHandler, MauiShellErrorHandler>()
            .AddSingleton<IUriNavigator, MauiShellUriNavigator>()
            .AddTransient<IDialogService, MauiShellDialogService>()
            .AddSingleton<IMauiApplicationProxy, MauiApplicationProxy>();

        return services;
    }

    public static IServiceCollection AddMauiTargetNavigator(this IServiceCollection services)
    {
        services
            .AddSingleton<ITargetNavigator, MauiTargetNavigator>()
            .AddSingleton<INavigationRegister>(_ => NavigationRegister);

        return services;
    }

    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, string route) 
        where TView : BaseMauiContentPage<TViewModel>
        where TViewModel : class, IViewModel
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        Routing.RegisterRoute(route, typeof(TView));

        return services;
    }
    
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, NavigationTarget target) 
        where TView : BaseMauiContentPage<TViewModel>
        where TViewModel : class, IViewModel
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();
        Routing.RegisterRoute(target.Name, typeof(TView));
        
        NavigationRegister.RegisterView<TView, TViewModel>(target);

        return services;
    }

    public static IServiceCollection AddContainer<TContainer, TViewModel>(this IServiceCollection services, NavigationContainerIdentifier containerIdentifier) 
        where TContainer: BaseMauiShell<TViewModel>
        where TViewModel : class, IViewModel
    {
        services.AddTransient<TContainer>();
        services.AddTransient<TViewModel>();

        NavigationRegister.RegisterContainer<TContainer, TViewModel>(containerIdentifier);

        return services;
    }
}