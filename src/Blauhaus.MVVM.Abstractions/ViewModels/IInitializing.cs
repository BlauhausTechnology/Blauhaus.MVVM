namespace Blauhaus.MVVM.Abstractions.ViewModels
{
    public interface IInitializing<T>
    {
        void Initialize(T initialValue);
    }
}