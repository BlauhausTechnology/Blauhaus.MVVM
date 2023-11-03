using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigation.NavigationService
{
    public interface INavigationService
    {
        Task SetMainViewAsNavigationRootAsync<TViewModel>(string navigationStackName = "") where TViewModel : class, IViewModel;
        Task SetAndInitializeMainViewAsNavigationRootAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : class, IViewModel, IAsyncInitializable<T>;

        Task ShowMainViewAsync<TViewModel>() where TViewModel : class, IViewModel;
        Task ShowMainViewAsync(Type viewModelType);


        Task ShowViewAsync<TViewModel>(string navigationStackName = "") where TViewModel : IViewModel;
        Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : IViewModel, IAsyncInitializable<T>;
        Task ShowAndInitializeMainViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IAsyncInitializable<T>;

        Task ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel;

        void SetCurrentNavigationView(INavigationView navigationView);
        void SetCurrentNavigationStack(string navigationStackName);
        void SetCurrentFlyoutView(IFlyoutView flyoutView);
        
        /// <summary>
        /// Navigates back to the last page for TViewModel or resets the navigation stack and navigates to the page for TViewModel.
        /// </summary>
        Task GoBackOrShowAsync<TViewModel>();
        Task GoBackAsync();
        Task GoBackToRootAsync();
        Task GoBackToAsync<TViewModel>();

        /// <summary>
        /// Navigates to the next page for TViewModel and removes the current page from the navigation stack.
        /// </summary>
        Task ShowViewAndRemoveCurrent<TViewModel>();
    }
}