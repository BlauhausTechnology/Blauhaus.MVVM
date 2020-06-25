using System;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Xamarin.Views
{
    public abstract class BaseDisappearingContentPage<TViewModel> : BaseAppearingContentPage<TViewModel> 
        where TViewModel : IAppearing, IDisappearing
    {
        protected BaseDisappearingContentPage(TViewModel viewModel) : base(viewModel)
        {
        }

        protected override void OnDisappearing()
        {
            if (ViewModel == null)
            {
                throw new Exception($"Cannot call DisappearingCommand because {typeof(TViewModel).Name} is null");
            }

            if (ViewModel.DisappearingCommand == null)
            {
                throw new Exception($"Cannot call DisappearingCommand on {typeof(TViewModel).Name} because the command is null");
            }

            ViewModel.DisappearingCommand.Execute(null);
        }

    }
}