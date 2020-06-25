using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Abstractions.Navigation;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.ErrorHandling
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly IDialogService _dialogService;
        private readonly IAnalyticsService _analyticsService;

        public ErrorHandlingService(
            IDialogService dialogService, 
            IAnalyticsService analyticsService)
        {
            _dialogService = dialogService;
            _analyticsService = analyticsService;
        }


        public Task HandleExceptionAsync(object sender, Exception exception)
        {
            _analyticsService.LogException(sender, exception);
            return _dialogService.DisplayAlertAsync("Error", "An unexpected error has occured");
        }

        public async Task HandleErrorAsync(string errorMessage)
        {
            await _dialogService.DisplayAlertAsync("Error", errorMessage);
        }
    }
}