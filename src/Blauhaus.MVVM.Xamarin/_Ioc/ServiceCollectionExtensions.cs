using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Dialogs;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Navigation;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin._Ioc
{
    public static class ServiceCollectionExtensions
    {

        private static readonly NavigationLookup NavigationLookup = new NavigationLookup();

        public static IServiceCollection AddMvvmServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IDialogService, FormsDialogService>()
                .AddSingleton<INavigationService, FormsNavigationService>()
                .AddSingleton<IFormsApplicationProxy, FormsApplicationProxy>()
                .AddSingleton<INavigationLookup>(x => NavigationLookup);

            services
                .AddTransient<IErrorHandler, ErrorHandler>();
            
            return services;
        }
         
        public static IServiceCollection AddViewSingleton<TView, TViewModel>(this IServiceCollection services) 
            where TView : Element, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationLookup>(NavigationLookup);

            services.AddSingleton<TView>();
            services.AddSingleton<TViewModel>();

            NavigationLookup.Register<TView, TViewModel>();
            return services;
        }
         

        public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services) 
            where TView : Element, IView 
            where TViewModel : class, IViewModel
        {
            services.TryAddSingleton<INavigationLookup>(NavigationLookup);
            
            services.AddTransient<TView>();
            services.AddTransient<TViewModel>();

            NavigationLookup.Register<TView, TViewModel>();
            return services;
        }
         
         
    }
}