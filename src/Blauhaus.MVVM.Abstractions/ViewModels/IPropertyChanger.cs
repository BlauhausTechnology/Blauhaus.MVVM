namespace Blauhaus.MVVM.Abstractions.ViewModels;

public interface IPropertyChanger
{
    void Notify(string propertyName);
}