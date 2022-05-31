using System;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Dialogs;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IDialogService _dialogService;
        private readonly IAnalyticsLogger _logger;
        private readonly IBuildConfig _buildConfig;

        public ErrorHandler(
            IDialogService dialogService, 
            IAnalyticsLogger<ErrorHandler> logger,
            IBuildConfig buildConfig)
        {
            _dialogService = dialogService;
            _logger = logger;
            _buildConfig = buildConfig;
        }


        public async Task HandleExceptionAsync(object sender, Exception exception)
        {
            _logger.LogError(Error.Unexpected(), exception);

            var errorMessage = "An unexpected error has occured";
            if (exception is ErrorException errorException)
            {
                errorMessage = errorException.Error.Description;
                if (await OnErrorHandledAsync(errorException.Error))
                {
                    return;
                }
            }
            else if ((BuildConfig) _buildConfig == BuildConfig.Debug)
            {
                errorMessage = exception.Message;
            }

            await _dialogService.DisplayAlertAsync("Error", errorMessage);
        }

        public async Task HandleErrorAsync(string errorMessage)
        {
            if (errorMessage.IsError(out var error))
            {
                errorMessage = error.Description;
                if (await OnErrorHandledAsync(error))
                {
                    return;
                }
            }
            await _dialogService.DisplayAlertAsync("Error", errorMessage);
        }

        public async Task HandleErrorAsync(Error error)
        {
            var errorWasHandled = await OnErrorHandledAsync(error);
            if (!errorWasHandled)
            {
                await _dialogService.DisplayAlertAsync("Error", error.Description);
            }
        }

        protected virtual Task<bool> OnErrorHandledAsync(Error error)
        {
            return Task.FromResult(false);
        }
    }
}