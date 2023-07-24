using System.Threading.Tasks;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy;

namespace Blauhaus.MVVM.Xamarin.Dialogs
{
    public class FormsDialogService : IDialogService
    {
        private readonly IFormsApplicationProxy _application;
        private readonly IThreadService _threadService;

        public FormsDialogService(
            IFormsApplicationProxy application, 
            IThreadService threadService)
        {
            _application = application;
            _threadService = threadService;
        }

        public Task DisplayAlertAsync(string title, string message, string cancelButtonText = "OK")
        {
            return _threadService.InvokeOnMainThreadAsync(async () => await _application
                .DisplayAlertAsync(title, message, cancelButtonText));
        }

        public Task<bool> DisplayConfirmationAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
        {
            return _threadService.InvokeOnMainThreadAsync(async () => await _application
                .DisplayAlertAsync(title, message, cancelButtonText, acceptButtonText));
        }

        public Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return _threadService.InvokeOnMainThreadAsync(async () => await _application
                .DisplayActionSheetAsync(title, cancel, destruction, buttons));
        }
    }
}