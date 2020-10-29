using System;
using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.Xamarin.Views.Content
{
    public abstract class BaseAppearingContentPage<TViewModel> : BaseContentPage<TViewModel> 
        where TViewModel : IAppear
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

            if (ViewModel.AppearCommand == null)
            {
                throw new Exception($"Cannot call AppearingCommand on {typeof(TViewModel).Name} because the command is null");
            }

            ViewModel.AppearCommand.Execute(null);
        }

    }
}