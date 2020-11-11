namespace Blauhaus.MVVM.Abstractions.Views
{
    public interface IFlyoutView : IView
    {
        void ShowDetail<TView>(TView view) where TView : IView;
    }
}