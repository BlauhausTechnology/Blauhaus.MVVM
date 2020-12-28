namespace Blauhaus.MVVM.Abstractions.Views
{
    public interface INavigationView : IView
    {
        string StackName { get; }   
        bool IsCurrent { get; set; }   
    }
}