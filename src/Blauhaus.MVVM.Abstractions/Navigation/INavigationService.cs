﻿using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public interface INavigationService
    {
        Task ShowMainViewAsync<TViewModel>() where TViewModel : IViewModel;
        Task ShowViewAsync<TViewModel>() where TViewModel : IViewModel;
        Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IInitialize<T>;
        
        Task ShowDetailViewAsync<TViewModel>() where TViewModel : IViewModel;

        void SetCurrentNavigationView(INavigationView navigationView);
        void SetCurrentFlyoutView(IFlyoutView flyoutView);

        Task GoBackAsync();
    }
}