﻿using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using IView = Blauhaus.MVVM.Abstractions.Views.IView;

namespace Blauhaus.MVVM.Maui.Views;

public abstract class BaseMauiShell<TViewModel> : Shell, IView<TViewModel>
    where TViewModel : IViewModel
{
    protected BaseMauiShell(TViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = ViewModel;
    }
    
    public TViewModel ViewModel { get; }
    public ViewIdentifier ContainerViewIdentifier { get; private set; } = null!;
  

    public void Initialize(ViewIdentifier viewIdentifier)
    {
        ContainerViewIdentifier = viewIdentifier;
    }

    protected override void OnAppearing()
    {
        if (ViewModel is IAppearingViewModel appearViewModel)
        {
            if (appearViewModel.AppearCommand.CanExecute(null))
            {
                appearViewModel.AppearCommand.Execute();
            }
        }
    }

    protected override void OnDisappearing()
    {
        if (ViewModel is IDisappearingViewModel disappearViewModel)
        {
            if (disappearViewModel.DisappearCommand.CanExecute(null))
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }
    }
     
    
}