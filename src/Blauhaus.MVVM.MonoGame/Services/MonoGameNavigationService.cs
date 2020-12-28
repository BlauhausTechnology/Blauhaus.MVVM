using System;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.MonoGame.Games;
using Blauhaus.MVVM.MonoGame.Scenes;

namespace Blauhaus.MVVM.MonoGame.Services
{
    public class MonoGameNavigationService : INavigationService
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly INavigationLookup _navigationLookup;
        private readonly IThreadService _threadService;
        private readonly ISceneGame _sceneGame;

        public MonoGameNavigationService(
            IServiceLocator serviceLocator,
            INavigationLookup navigationLookup,
            IThreadService threadService,
            ISceneGame sceneGame)
        {
            _serviceLocator = serviceLocator;
            _navigationLookup = navigationLookup;
            _threadService = threadService;
            _sceneGame = sceneGame;
        }
        
        public Task ShowMainViewAsync<TViewModel>() where TViewModel : class, IViewModel
        {
            var scene = GetSceneForViewModel<TViewModel>();
            _sceneGame.ChangeScene(scene);
            return Task.CompletedTask;
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
        
        
        private IScene GetSceneForViewModel<TViewModel>() 
            where TViewModel : class
        {
            var viewType = _navigationLookup.GetViewType(typeof(TViewModel));
            if (viewType == null)
            {
                throw new NavigationException($"No view is registered for {typeof(TViewModel).Name}");
            }

            var view = _serviceLocator.Resolve<TViewModel>();
            if (view == null)
            {
                throw new NavigationException($"No View of type {viewType.Name} has been registered with the Ioc container");
            }
            
            if (!(view is IScene page))
            {
                throw new NavigationException($"View type {viewType.Name} is not an IScene");
            }

            return page;
        }
    }
}