using System.ComponentModel;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Blauhaus.MVVM.Blazor.Components;

public class BaseComponent<TViewModel> : ComponentBase, IDisposable
    where TViewModel : class, IViewModel, INotifyPropertyChanged
{
    private bool _isDisposed;

    [Inject] public TViewModel ViewModel { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    
    public string Path { get; private set; } = null!;
   
    
    protected override void OnInitialized()
    {
        //this happens if the same route is invoked but with different query string params, so we need to reload
        //however, when you try and reload you get the query string params of the current page so if you nav away the previous page reloads with
        //the new params. So we need to subscribe with a filter that says only reload if the NavigationManager.Uri.Contains(path)

        Path = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        NavigationManager.LocationChanged += HandleLocationChanged;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        if (ViewModel is IAppearingViewModel appearingViewModel)
        {
            appearingViewModel.AppearCommand.Execute(Path);
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }

    private async void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if(_isDisposed) return;
        
        var newPath = NavigationManager.ToBaseRelativePath(e.Location);
        if (Path != newPath)
        {
            Path = newPath;
            if (ViewModel is IAppearingViewModel appearingViewModel)
            {
                appearingViewModel.AppearCommand.Execute(Path);
            }
            await HandleDataContextChanged();
        }
    }

    protected virtual Task HandleDataContextChanged()
    {
        return Task.CompletedTask;
    }


    public void Dispose()
    {
        _isDisposed = true;
        ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        NavigationManager.LocationChanged -= HandleLocationChanged;
    }
}