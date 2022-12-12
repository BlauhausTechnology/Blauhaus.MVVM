namespace Blauhaus.MVVM.Maui.Applications;

public class MauiApplicationProxy : IMauiApplicationProxy
{
    
    public void SetMainPage<TPage>(TPage page) where TPage : Page
    {
        if (Application.Current == null)
        {
            throw new InvalidOperationException("No MAUI application exists");
        }
        
        Application.Current.MainPage = page;
            
    }

    public Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept)
    {
        if (Application.Current == null)
        {
            throw new InvalidOperationException("No MAUI application exists");
        }
        if (Application.Current.MainPage == null)
        {
            throw new InvalidOperationException("Current MAUI application has no main page");
        }

        return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
    }

    public Task DisplayAlertAsync(string title, string message, string cancel)
    {
        if (Application.Current == null)
        {
            throw new InvalidOperationException("No MAUI application exists");
        }
        if (Application.Current.MainPage == null)
        {
            throw new InvalidOperationException("Current MAUI application has no main page");
        }

        return Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }
}