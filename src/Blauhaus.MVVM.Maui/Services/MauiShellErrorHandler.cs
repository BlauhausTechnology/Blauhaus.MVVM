using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiShellErrorHandler : IErrorHandler
{
    private readonly IAnalyticsLogger<MauiShellErrorHandler> _logger;

    public MauiShellErrorHandler(
        IAnalyticsLogger<MauiShellErrorHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleExceptionAsync(object sender, Exception exception)
    {
        if (exception is ErrorException errorException)
        {
            await HandleErrorAsync(errorException.Error);
        }
        else
        {
            _logger.LogError(Error.Unexpected(sender.GetType().Name));
            await Shell.Current.DisplayAlert("Error", "An unexpected error occured", "OK");
        }
    }

    public async Task HandleErrorAsync(string errorMessage)
    {
        await Shell.Current.DisplayAlert("Error", "errorMessage", "OK");
    }

    public async Task HandleErrorAsync(Error errorMessage)
    {
        await Shell.Current.DisplayAlert(errorMessage.Code, errorMessage.Description, "OK");
    }
}