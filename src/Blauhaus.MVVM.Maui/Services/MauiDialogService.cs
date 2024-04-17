using Blauhaus.MVVM.Abstractions.Dialogs;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiDialogService : IDialogService
{
    public async Task DisplayAlertAsync(string title, string message, string cancelButtonText = "OK")
    {
        if (Application.Current is null) throw new InvalidOperationException("Current application is null");
        if (Application.Current.MainPage is null) throw new InvalidOperationException("Current main application page is null");

        await Application.Current.MainPage.DisplayAlert(title, message, cancelButtonText);
    }

    public async Task<bool> DisplayConfirmationAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
    {
        if (Application.Current is null) throw new InvalidOperationException("Current application is null");
        if (Application.Current.MainPage is null) throw new InvalidOperationException("Current main application page is null");

        await Application.Current.MainPage.DisplayAlert(title, message, acceptButtonText, cancelButtonText);
        return true;
        
    }

    public async Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
    {
        if (Application.Current is null) throw new InvalidOperationException("Current application is null");
        if (Application.Current.MainPage is null) throw new InvalidOperationException("Current main application page is null");

        return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, FlowDirection.MatchParent, buttons);
    }

    public async Task<string?> DisplayPromptAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
    {
        if (Application.Current is null) throw new InvalidOperationException("Current application is null");
        if (Application.Current.MainPage is null) throw new InvalidOperationException("Current main application page is null");

        return await Application.Current.MainPage.DisplayPromptAsync(title, message, acceptButtonText, cancelButtonText);
    }
}