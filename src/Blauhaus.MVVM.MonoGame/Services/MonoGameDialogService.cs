using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Dialogs;

namespace Blauhaus.MVVM.MonoGame.Services
{
    public class MonoGameDialogService : IDialogService
    {
        public Task DisplayAlertAsync(string title, string message, string cancelButtonText = "OK")
        {
            return Task.CompletedTask;
        }

        public Task<bool> DisplayConfirmationAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
        {
            return Task.FromResult(true);
        }

        public Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return Task.FromResult("");
        }

        public Task<string?> DisplayPromptAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
        {
            throw new System.NotImplementedException();
        }
    }
}