using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.MonoGame.Games;
using Blauhaus.MVVM.MonoGame.Screens;

namespace Blauhaus.MVVM.MonoGame.Services
{
    public class MonoGameNavigationService : INavigationService
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly INavigationLookup _navigationLookup;
        private readonly IThreadService _threadService;
        private readonly IScreenGame _screenGame;

        public MonoGameNavigationService(
            IServiceLocator serviceLocator,
            INavigationLookup navigationLookup,
            IThreadService threadService,
            IScreenGame screenGame)
        {
            _serviceLocator = serviceLocator;
            _navigationLookup = navigationLookup;
            _threadService = threadService;
            _screenGame = screenGame;
        }
        
        public Task ShowMainViewAsync<TViewModel>() where TViewModel : class, IViewModel
        {
            return ShowMainViewAsync(typeof(TViewModel));
        }

        public async Task ShowMainViewAsync(Type viewModelType)
        {
            var scene = GetScreenForViewModel(viewModelType);

            switch (scene.BindingContext)
            {
                case IAsyncInitializable initializable:
                    await initializable.InitializeAsync();
                    break;
                case IInitializingViewModel initializableViewModel:
                    initializableViewModel.InitializeCommand.Execute();
                    break;
            }

            _screenGame.ChangeScene(scene);
        }
        
        public async Task ShowAndInitializeMainViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IAsyncInitializable<T>
        {
            var scene = GetScreenForViewModel(typeof(TViewModel));
            var viewModel = (TViewModel)scene.BindingContext;
            await viewModel.InitializeAsync(parameter);
            _screenGame.ChangeScene(scene);
        }


        public Task ShowViewAsync<TViewModel>(string navigationStackName = "") where TViewModel : IViewModel
        {
            throw new System.NotImplementedException();
        }

        public Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter, string navigationStackName = "") where TViewModel : IViewModel, IAsyncInitializable<T>
        {
            throw new System.NotImplementedException();
        }

        public Task ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentNavigationView(INavigationView navigationView)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentNavigationView(string navigationStackName)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentFlyoutView(IFlyoutView flyoutView)
        {
            throw new System.NotImplementedException();
        }

        public Task GoBackAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task GoBackToRootAsync()
        {
            throw new System.NotImplementedException();
        }
        
        
        private IGameScreen GetScreenForViewModel(Type viewModelType) 
        {
            var viewType = _navigationLookup.GetViewType(viewModelType);
            if (viewType == null)
            {
                throw new NavigationException($"No view is registered for {viewModelType.Name}");
            }

            var view = _serviceLocator.Resolve(viewType);
            if (view == null)
            {
                throw new NavigationException($"No View of type {viewType.Name} has been registered with the Ioc container");
            }
            
            if (!(view is IGameScreen screen))
            {
                throw new NavigationException($"View type {viewType.Name} is not an IScene");
            }

            return screen;
        }
    }
}