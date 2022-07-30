using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views.Base;

public abstract class BaseTestAppView<TViewModel> : BaseMauiContentPage<TViewModel> where TViewModel : IViewModel
{
    protected BaseTestAppView(TViewModel viewModel) : base(viewModel)
    {
    }
}