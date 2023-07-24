using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Maui.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Blauhaus.MVVM.Maui.Views;

public abstract class BaseMauiContentPage<TViewModel> : ContentPage, IView<TViewModel>, INavigableView
    where TViewModel : IViewModel
{
    protected BaseMauiContentPage(TViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = ViewModel;
        
        SubscribeToHotReload();
        On<iOS>().SetUseSafeArea(true);
    }
    
    public TViewModel ViewModel { get; }
    public IViewTarget ViewTarget { get; private set; } = null!;

    public Task InitializeAsync(IViewTarget viewTarget)
    {
        ViewTarget = viewTarget;
        return OnInitializedAsync(viewTarget);
    }

    protected virtual Task OnInitializedAsync(IViewTarget viewIdentifier)
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
    
    private bool _isSubscribedToHotReload;
    protected abstract bool IsHotReloadEnabled { get; }

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