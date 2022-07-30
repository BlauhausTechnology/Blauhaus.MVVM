namespace Blauhaus.MVVM.Maui.MauiApplication;

public interface IMauiApplication
{
    void SetMainPage(Page page);
    Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept);
    Task DisplayAlertAsync(string title, string message, string cancel);
}