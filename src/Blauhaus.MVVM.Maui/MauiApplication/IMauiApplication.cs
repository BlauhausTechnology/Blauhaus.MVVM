namespace Blauhaus.MVVM.Maui.MauiApplication;

public interface IMauiApplication
{
    Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept);
    Task DisplayAlertAsync(string title, string message, string cancel);

    void SetMainPage(Page page);
    void AddShellContent(Page page);
}