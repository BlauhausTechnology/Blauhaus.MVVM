namespace Blauhaus.MVVM.Maui.Applications;

public interface IMauiApplicationProxy
{
    void SetMainPage<TPage>(TPage page) where TPage : Page;
    Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept);
    Task DisplayAlertAsync(string title, string message, string cancel);
}