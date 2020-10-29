using System;
using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.Xamarin.Views.Content
{
    public abstract class BaseDisappearingContentPage<TViewModel> : BaseAppearingContentPage<TViewModel> 
        where TViewModel : IAppear, IDisappear
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

            if (ViewModel.DisappearCommand == null)
            {
                throw new Exception($"Cannot call DisappearingCommand on {typeof(TViewModel).Name} because the command is null");
            }

            ViewModel.DisappearCommand.Execute(null);
        }

    }
}