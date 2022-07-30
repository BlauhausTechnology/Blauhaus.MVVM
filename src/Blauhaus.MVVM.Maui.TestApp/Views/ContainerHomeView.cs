using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.Views.Base;
using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class ContainerHomeView : BaseTestAppView<ContainerHomeViewModel>
{
    public ContainerHomeView(ContainerHomeViewModel viewModel) : base(viewModel)
    {
        Content = new StackLayout
        {
            BackgroundColor = Color.FromRgb(0,100,0)
        }.Fill();
    }
}