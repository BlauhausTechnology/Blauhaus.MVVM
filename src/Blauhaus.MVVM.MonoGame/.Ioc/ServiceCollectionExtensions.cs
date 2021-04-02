using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.MonoGame.Screens;
using Blauhaus.MVVM.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.MVVM.MonoGame._Ioc
{
    public static class ServiceCollectionExtensions
    {
        private static readonly NavigationLookup NavigationLookup = new NavigationLookup();
        
        public static IServiceCollection AddMvvmServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, MonoGameNavigationService>();
            
            services
                .AddSingleton<IDialogService, MonoGameDialogService>()
                .AddSingleton<INavigationService, MonoGameNavigationService>()
                .AddSingleton<INavigationLookup>(x => NavigationLookup)
                .AddSingleton<IErrorHandler, MonoGameErrorHandler>();
            
            return services;
        }
        
        public static IServiceCollection AddPage<TPage, TViewModel>(this IServiceCollection services) 
            where TPage : BaseScreen<TViewModel>, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationLookup>(NavigationLookup);
            
            services.AddTransient<TPage>();
            services.AddTransient<TViewModel>();

            NavigationLookup.Register<TPage, TViewModel>();
            return services;
        }
    }
}