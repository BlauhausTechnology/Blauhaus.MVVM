using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views.Base;
using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class LoadingView : BaseTestAppContentPage<LoadingViewModel>
{
    public LoadingView(LoadingViewModel viewModel) : base(viewModel)
    {
        BackgroundColor = Color.FromRgb(100, 100, 100);

        Content = new VerticalStackLayout
        {            
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,

            Children =
            {
                new BoxView { HeightRequest = 100 },
                
                new Label()
                    .Bind(nameof(ViewModel.Status)),
                
                new Button{Text = "Go to Full Screen screen"}
                    .Bind(nameof(ViewModel.NavigateFullScreenCommand)),

                new Button{Text = "Go to Main Container screen"}
                    .Bind(nameof(ViewModel.NavigateContainerCommand)),
            }
        };
    }
}