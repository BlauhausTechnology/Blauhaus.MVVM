using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class MainContainerView : BaseMauiShell<MainContainerViewModel>
{
    public MainContainerView(MainContainerViewModel viewModel, IServiceLocator serviceLocator) : base(viewModel)
    {
        Items.Add(new FlyoutItem
        {
            Title = "random page",
            Route = "page",
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,

            Items =
            {
                new ShellContent
                {
                    Title = "Random",
                    ContentTemplate = new DataTemplate(()=> new ContentPage()),
                },

            }
        });
    }
}