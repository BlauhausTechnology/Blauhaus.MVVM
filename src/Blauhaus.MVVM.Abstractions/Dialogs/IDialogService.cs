using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Dialogs
{
    public interface IDialogService
    {
        Task DisplayAlertAsync(string title, string message, string cancelButtonText = "OK");
        Task<bool> DisplayConfirmationAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK");
        Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
        Task<string?> DisplayPromptAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK");

    }
}