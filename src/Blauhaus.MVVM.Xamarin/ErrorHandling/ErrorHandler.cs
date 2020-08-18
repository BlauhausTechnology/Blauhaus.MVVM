using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;

namespace Blauhaus.MVVM.Xamarin.ErrorHandling
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IDialogService _dialogService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IBuildConfig _buildConfig;

        public ErrorHandler(
            IDialogService dialogService, 
            IAnalyticsService analyticsService,
            IBuildConfig buildConfig)
        {
            _dialogService = dialogService;
            _analyticsService = analyticsService;
            _buildConfig = buildConfig;
        }


        public Task HandleExceptionAsync(object sender, Exception exception)
        {

            _analyticsService.LogException(sender, exception);

            var errorMessage = "An unexpected error has occured";
            if (exception is ErrorException errorException)
            {
                errorMessage = errorException.Error.Description;
            }
            else if ((BuildConfig) _buildConfig == BuildConfig.Debug)
            {
                errorMessage = exception.Message;
            }

            return _dialogService.DisplayAlertAsync("Error", errorMessage);
        }

        public async Task HandleErrorAsync(string errorMessage)
        {
            if (errorMessage.IsError(out var error))
            {
                errorMessage = error.Description;
            }
            await _dialogService.DisplayAlertAsync("Error", errorMessage);
        }
    }
}