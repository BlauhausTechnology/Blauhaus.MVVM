using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views.Base;
using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class FullScreenView : BaseTestAppContentPage<FullScreenViewModel>
{
    public FullScreenView(FullScreenViewModel viewModel) : base(viewModel)
    {
        BackgroundColor = Color.FromRgb(10, 120, 100);

        Content = new VerticalStackLayout
        {            
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,

            Children =
            {
                new BoxView { HeightRequest = 100 },
                 
                
                new Button{Text = "Go to Loading screen"}
                    .Bind(nameof(ViewModel.NavigateLoadingCommand)),

                //new Button{Text = "Go to Container screen"}
                //    .Bind(nameof(ViewModel.NavigateContainerCommand)),
            }
        }.Fill();
    }
}