using Blauhaus.MVVM.Abstractions.Dialogs;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiShellDialogService : IDialogService
{
    public async Task DisplayAlertAsync(string title, string message, string cancelButtonText = "OK")
    {
        await Shell.Current.DisplayAlert(title, message, cancelButtonText);
    }

    public async Task<bool> DisplayConfirmationAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
    {
        await Shell.Current.DisplayPromptAsync(title, message, acceptButtonText, cancelButtonText);
        return true;
        
    }
}