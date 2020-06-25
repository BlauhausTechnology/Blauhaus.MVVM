﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using CSharpFunctionalExtensions;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public interface INavigationService
    {
        Task ShowMainViewAsync<TViewModel>() where TViewModel : IViewModel;
        Task ShowViewAsync<TViewModel>() where TViewModel : IViewModel;
        Task ShowAndInitializeViewAsync<TViewModel, T>(T parameter) where TViewModel : IViewModel, IInitializing<T>;
        Task GoBackAsync();
        void SetCurrentNavigationView(INavigationView navigationView);

    }
}