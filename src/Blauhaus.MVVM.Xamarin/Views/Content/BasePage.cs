using System;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Xamarin.Views.Content
{
    public abstract class BasePage<TViewModel> : BaseUpdateContentPage, IView
    {
        protected readonly TViewModel ViewModel;

        protected BasePage(TViewModel viewModel) 
            : base(viewModel is INotifyUpdates)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            if (ViewModel is IAppearingViewModel appearViewModel)
            {
                appearViewModel.AppearCommand.Execute();
            }
        }

        protected override void OnDisappearing()
        {
            if (ViewModel is IDisappearingViewModel disappearViewModel)
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (ViewModel is IDisposable disposable)
            {
                disposable?.Dispose();
            }

            if (ViewModel is IAsyncDisposable asyncDisposable)
            {
                asyncDisposable?.DisposeAsync();
            }

            return false;
        }

        public void Initialize(ViewIdentifier initialValue)
        {
            throw new NotImplementedException();
        }
    }
}