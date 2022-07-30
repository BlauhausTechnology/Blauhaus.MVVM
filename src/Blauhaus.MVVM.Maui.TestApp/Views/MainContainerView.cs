using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class MainContainerView : BaseMauiShell<MainContainerViewModel>
{
    public MainContainerView(MainContainerViewModel viewModel) : base(viewModel)
    {
    }
}