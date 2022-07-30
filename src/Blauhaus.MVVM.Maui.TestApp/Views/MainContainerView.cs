using Blauhaus.MVVM.Maui.TestApp.Styling;
using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class MainContainerView : BaseMauiShell<MainContainerViewModel>
{
    public MainContainerView(MainContainerViewModel viewModel) : base(viewModel)
    {
        BackgroundColor = AppColours.PrussianBlue;

        Items.Add(new ShellContent
        {
            Route = "test",
            Title = "Shell content",
            Content = new ContentPage
            {
                BackgroundColor = AppColours.Accent
            }
        });
    }
}