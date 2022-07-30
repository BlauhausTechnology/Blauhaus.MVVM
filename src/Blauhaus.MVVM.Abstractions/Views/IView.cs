using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Views
{
    public interface IView
    {
    }

    public interface IView<out TViewModel> where TViewModel : IViewModel
    {
        public TViewModel ViewModel { get; }
    }
}