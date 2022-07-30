using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;
using Blauhaus.MVVM.Maui.Views;

namespace Blauhaus.MVVM.Maui.TestApp.Views.Base;

public abstract class BaseTestAppContentPage<TViewModel> : BaseMauiContentPage<TViewModel> where TViewModel : class, IViewModel
{
    protected BaseTestAppContentPage(TViewModel viewModel) : base(viewModel)
    {
        SetBinding(TitleProperty, new Binding(nameof(BaseTestAppViewModel.Title)));
    }
}