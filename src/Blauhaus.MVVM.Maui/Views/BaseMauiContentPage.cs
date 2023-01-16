using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Maui.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Blauhaus.MVVM.Maui.Views;

public abstract class BaseMauiContentPage<TViewModel> : ContentPage, IView<TViewModel>, IAsyncInitializable<NavigationTarget>
    where TViewModel : IViewModel
{
    public TViewModel ViewModel { get; }
    
    private bool _isSubscribedToHotReload;
    protected abstract bool IsHotReloadEnabled { get; }

    protected BaseMauiContentPage(TViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = ViewModel;
        
        SubscribeToHotReload();
        On<iOS>().SetUseSafeArea(true);
    }
    
    public virtual Task InitializeAsync(NavigationTarget value)
    {
        return Task.CompletedTask;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel is IAppearingViewModel appearViewModel)
        {
            if (appearViewModel.AppearCommand.CanExecute(null))
            {
                appearViewModel.AppearCommand.Execute();
            }
        }
        
        SubscribeToHotReload();

    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (ViewModel is IDisappearingViewModel disappearViewModel)
        {
            if (disappearViewModel.DisappearCommand.CanExecute(null))
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }

        UnsubscribeFromHotReload();
    }

     
    #region HotReload
    private void SubscribeToHotReload()
    {
        if (IsHotReloadEnabled)
        {
            if (!_isSubscribedToHotReload)
            {
                HotReloadService.UpdateApplicationEvent += ReloadUi;
                _isSubscribedToHotReload = true;
            }
        }
       
        Content = Build();
    }

    private void UnsubscribeFromHotReload()
    {
        if (IsHotReloadEnabled)
        {
            HotReloadService.UpdateApplicationEvent -= ReloadUi;
            _isSubscribedToHotReload = false;
        }
    }

    private void ReloadUi(Type[]? obj)
    {
        MainThread.BeginInvokeOnMainThread(() => Content = Build());
    }

    protected abstract View Build();

    #endregion

}