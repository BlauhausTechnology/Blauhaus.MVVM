using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public interface INavigationService
    {
        Task ShowMainViewAsync<TViewModel>() where TViewModel : IViewModel;


        Task ShowViewAsync<TViewModel>(string navigationStackName = "") where TViewModel : IViewModel;
        Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : IViewModel, IAsyncInitializable<T>;
        
        Task ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel;

        void SetCurrentNavigationView(INavigationView navigationView);
        void SetCurrentNavigationView(string navigationStackName);
        void SetCurrentFlyoutView(IFlyoutView flyoutView);

        Task GoBackAsync();
        Task GoBackToRootAsync();
    }
}