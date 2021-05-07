using System;
using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class NavigationServiceMockBuilder : BaseMockBuilder<NavigationServiceMockBuilder, INavigationService>
    {

        public NavigationServiceMockBuilder Where_NavigateMain_throws<TViewModel>(Exception e) where TViewModel : class, IViewModel
        {
            Mock.Setup(x => x.ShowMainViewAsync<TViewModel>())
                .Throws(e);
            return this;
        }
        
        public void Verify_ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowDetailViewAsync<TViewModel>());
        } 
        public void Verify_ShowDetailViewAsync_NOT_called<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowDetailViewAsync<TViewModel>(), Times.Never);
        }


        public void Verify_ShowViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowViewAsync<TViewModel>(It.IsAny<string>()));
        }
        public void Verify_ShowViewAsync<TViewModel>(string navigationStackName) where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowViewAsync<TViewModel>(navigationStackName));
        }
        public void Verify_ShowViewAsync_NOT_called<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowViewAsync<TViewModel>(It.IsAny<string>()), Times.Never);
        }

        public void Verify_NavigateMain_was_called_with_ViewModelType<TViewModel>() where TViewModel : class, IViewModel
        {
            Mock.Verify(x => x.ShowMainViewAsync<TViewModel>());
        }
        public void Verify_NavigateMain_was_NOT_called_with_ViewModelType<TViewModel>() where TViewModel : class, IViewModel
        {
            Mock.Verify(x => x.ShowMainViewAsync<TViewModel>(), Times.Never);
        }

        public void Verify_ShowAndInitializeViewAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IViewModel, IAsyncInitializable<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeViewAsync<TViewModel, TParameter>(parameter, It.IsAny<string>()));
        } 
        public void Verify_ShowAndInitializeViewAsync<TViewModel, TParameter>(TParameter parameter, string navigationStackName) where TViewModel : IViewModel, IAsyncInitializable<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeViewAsync<TViewModel, TParameter>(parameter, navigationStackName));
        } 
        public void Verify_ShowAndInitializeMainViewAsync<TViewModel, TParameter>(TParameter parameter, int times = 1) where TViewModel : IViewModel, IAsyncInitializable<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeMainViewAsync<TViewModel, TParameter>(parameter), Times.Exactly(times));
        } 
        public void Verify_ShowAndInitializeViewAsync_NOT_called<TViewModel, TParameter>() where TViewModel : IViewModel, IAsyncInitializable<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeViewAsync<TViewModel, TParameter>(It.IsAny<TParameter>(), It.IsAny<string>()), Times.Never);
        }

        
        public void Verify_GoBackAsync(Times times)
        {
            Mock.Verify(x => x.GoBackAsync(), times);
        }

        public void Verify_GoBackAsync()
        {
            Mock.Verify(x => x.GoBackAsync());
        }
        
        public void Verify_GoBackToRootAsync()
        {
            Mock.Verify(x => x.GoBackToRootAsync());
        }
    }
}