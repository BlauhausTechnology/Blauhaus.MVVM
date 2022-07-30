using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views.Base;
using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class LoadingView : BaseTestAppView<LoadingViewModel>
{
    public LoadingView(LoadingViewModel viewModel) : base(viewModel)
    {
        BackgroundColor = Color.FromRgb(100, 10, 0);

        Content = new StackLayout
        {
            Children =
            {
                new Button{ Text = "Go to container" }
                    .Bind(nameof(ViewModel.NavigateToContainerCommand))
            }
        }.Center();
    }
}