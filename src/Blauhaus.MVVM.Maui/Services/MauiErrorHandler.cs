using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Errors;
using Blauhaus.Errors.Handler;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiErrorHandler : IErrorHandler
{
    private readonly IAnalyticsLogger<MauiErrorHandler> _logger;

    public MauiErrorHandler(
        IAnalyticsLogger<MauiErrorHandler> logger)
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
            if (Application.Current != null && Application.Current.MainPage!=null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occured", "OK");
            }
        }
    }

    public async Task HandleErrorAsync(string errorMessage)
    {
        if (Application.Current != null && Application.Current.MainPage!=null)
        {
            await Shell.Current.DisplayAlert("Error", "errorMessage", "OK");
        }
    }

    public async Task HandleErrorAsync(Error errorMessage)
    {
        if (Application.Current != null && Application.Current.MainPage!=null)
        {
            await Shell.Current.DisplayAlert(errorMessage.Code, errorMessage.Description, "OK");
        }
    }
}