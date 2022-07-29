using Blauhaus.MVVM.Maui.TestApp.ViewModels;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views;

public class StartShell : BaseMauiShell<StartViewModel>
{
    public StartShell(StartViewModel viewModel) : base(viewModel)
    {
    }
}