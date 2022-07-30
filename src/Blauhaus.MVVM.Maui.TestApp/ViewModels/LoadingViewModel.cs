﻿using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class LoadingViewModel : BaseTestAppViewModel
{
    public LoadingViewModel(
        IServiceLocator serviceLocator, 
        ITargetNavigator targetNavigator) 
            : base(serviceLocator, targetNavigator)
    {
        NavigateToContainerCommand = serviceLocator.Resolve<AsyncExecutingCommand>()
            .WithExecute(async () => await targetNavigator.NavigateAsync(AppTargets.MainContainerView));
    }

    public IExecutingCommand NavigateToContainerCommand { get; }
}