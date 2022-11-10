using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Abstractions.Views
{
    public interface IView
    {
    }

    public interface IView<out TViewModel> : IView 
        where TViewModel : IViewModel
    {
        public TViewModel ViewModel { get; }
    }
}