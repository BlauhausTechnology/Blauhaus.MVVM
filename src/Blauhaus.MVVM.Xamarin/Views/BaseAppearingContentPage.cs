using System;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Xamarin.Views
{
    public abstract class BaseAppearingContentPage<TViewModel> : BaseContentPage<TViewModel> 
        where TViewModel : IAppearing
    {
        protected BaseAppearingContentPage(TViewModel viewModel) : base(viewModel)
        {
        }

        protected override void OnAppearing()
        {
            if (ViewModel == null)
            {
                throw new Exception($"Cannot call AppearingCommand because {typeof(TViewModel).Name} is null");
            }

            if (ViewModel.AppearingCommand == null)
            {
                throw new Exception($"Cannot call AppearingCommand on {typeof(TViewModel).Name} because the command is null");
            }

            ViewModel.AppearingCommand.Execute();
        }

    }
}