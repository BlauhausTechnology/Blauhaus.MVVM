﻿using System;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class NavigationServiceMockBuilder : BaseMockBuilder<NavigationServiceMockBuilder, INavigationService>
    {

        public NavigationServiceMockBuilder Where_NavigateMain_throws<TViewModel>(Exception e) where TViewModel : IViewModel
        {
            Mock.Setup(x => x.ShowMainViewAsync<TViewModel>())
                .Throws(e);
            return this;
        }
        
        public void Verify_ShowViewAsync<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowViewAsync<TViewModel>());
        }
        
        public void Verify_ShowViewAsync_NOT_called<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowViewAsync<TViewModel>(), Times.Never);
        }

        public void Verify_NavigateMain_was_called_with_ViewModelType<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowMainViewAsync<TViewModel>());
        }
        public void Verify_NavigateMain_was_NOT_called_with_ViewModelType<TViewModel>() where TViewModel : IViewModel
        {
            Mock.Verify(x => x.ShowMainViewAsync<TViewModel>(), Times.Never);
        }

        public void Verify_ShowAndInitializeViewAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IViewModel, IInitializing<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeViewAsync<TViewModel, TParameter>(parameter));
        }
        
        public void Verify_ShowAndInitializeViewAsync_NOT_called<TViewModel, TParameter>() where TViewModel : IViewModel, IInitializing<TParameter>
        {
            Mock.Verify(x => x.ShowAndInitializeViewAsync<TViewModel, TParameter>(It.IsAny<TParameter>()), Times.Never);
        }
    }
}