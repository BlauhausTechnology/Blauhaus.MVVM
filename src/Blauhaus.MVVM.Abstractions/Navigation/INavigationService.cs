using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigation
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

        Task GoBackAsync();
        Task GoBackToRootAsync();
    }
}