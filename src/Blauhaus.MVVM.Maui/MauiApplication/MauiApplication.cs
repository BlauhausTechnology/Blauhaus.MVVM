using Blauhaus.Analytics.Abstractions;

namespace Blauhaus.MVVM.Maui.MauiApplication;

public class MauiApplication : IMauiApplication
{

    public void SetMainPage(Page page)
    {
        GetMainPage("navigate");
        Application.Current!.MainPage = page;
    }

    public void AddShellContent(Page page)
    {
        var shellContent = new ShellContent
        {
            Title = page.GetType().Name,
            Route = page.GetType().Name,
            Content = page
        };

        var mainPage = GetMainPage("navigate");
        if (mainPage is not Shell shell)
        {
            shell = new Shell
            {
                Title = "Shell",
                Items = { shellContent }
            };
            Application.Current!.MainPage = shell;
        }
        else
        {
            shell.Items.Add(shellContent);
        }
        
        
    }

    public Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept)
    {
     return GetMainPage("display alert")
         .DisplayAlert(title, message, accept, cancel);
    }

    public Task DisplayAlertAsync(string title, string message, string cancel)
    {
        return GetMainPage("display alert")
            .DisplayAlert(title, message, cancel);
    }

    public Task GoToAsync(string route, bool animate)
    {
        return Shell.Current.GoToAsync(route, animate);
    }

    
    private static Page GetMainPage(string reason)
    {
        if (Application.Current is null)
        {
            throw new InvalidOperationException($"Cannot {reason} because current application is null");
        }
        
        if (Application.Current.MainPage is null)
        {
            throw new InvalidOperationException($"Cannot {reason} because current main page is null");
        }

        return Application.Current.MainPage;
    }

}