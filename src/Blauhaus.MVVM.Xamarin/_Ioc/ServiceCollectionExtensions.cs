using System;
using Blauhaus.Domain.Common.CommandHandlers.Sync;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Dialogs;
using Blauhaus.MVVM.Xamarin.ErrorHandling;
using Blauhaus.MVVM.Xamarin.Navigation;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;
using Blauhaus.MVVM.Xamarin.ViewElements.Sync;
using Blauhaus.MVVM.Xamarin.Views;
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
                .AddTransient<IErrorHandlingService, ErrorHandlingService>();
            
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

        public static IServiceCollection AddSyncCollectionViewElement<TModel, TViewElement, TSyncCommand, TUpdater>(this IServiceCollection services) 
            where TModel : class, IClientEntity 
            where TViewElement : ModelListItemViewElement, new() 
            where TSyncCommand : SyncCommand, new()
            where TUpdater : class, IModelViewElementUpdater<TModel, TViewElement>
        {
            services.AddTransient<SyncCollectionViewElement<TModel, TViewElement, TSyncCommand>>();
            services.AddTransient<IModelViewElementUpdater<TModel, TViewElement>, TUpdater>();
            return services;
        }

        public static IServiceCollection AddSyncCollectionViewElement<TICollection, TCollection, TModel, TViewElement, TSyncCommand, TUpdater>(this IServiceCollection services) 
            where TICollection : class, ISyncCollectionViewElement<TViewElement, TSyncCommand>
            where TCollection : SyncCollectionViewElement<TModel, TViewElement, TSyncCommand>, TICollection
            where TModel : class, IClientEntity 
            where TViewElement : ModelListItemViewElement, new() 
            where TSyncCommand : SyncCommand, new()
            where TUpdater : class, IModelViewElementUpdater<TModel, TViewElement>
        {
            services.AddTransient<TICollection, TCollection>();
            services.AddTransient<SyncCollectionViewElement<TModel, TViewElement, TSyncCommand>>();
            services.AddTransient<IModelViewElementUpdater<TModel, TViewElement>, TUpdater>();
            return services;
        }
    }
}