namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IInitialize<T>
    {
        void Initialize(T initialValue);
    }
}