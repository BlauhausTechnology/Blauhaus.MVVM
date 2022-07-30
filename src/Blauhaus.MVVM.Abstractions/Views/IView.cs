using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Views
{
    public interface IView
    {
    }

    public interface IView<out TViewModel> : IView where TViewModel : class, IViewModel
    {
        TViewModel ViewModel { get; }
    }
}