using Blauhaus.Errors;
using Blauhaus.Errors.Handler;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Blauhaus.MVVM.Blazor.Services;

//todo remove MudBlazor dependency
public class BlazorErrorHandler : IErrorHandler
{
    private readonly ILogger<BlazorErrorHandler> _logger;
    private readonly ISnackbar _snackbar;

    public BlazorErrorHandler(ILogger<BlazorErrorHandler> logger, ISnackbar snackbar)
    {
        _logger = logger;
        _snackbar = snackbar;
    }

    public Task HandleExceptionAsync(object sender, Exception exception)
    {
        _logger.LogError(exception, $"{sender.GetType().Name} failed");
        _snackbar.Add($"Exception: {exception.Message}", Severity.Error);
        return Task.CompletedTask;
    }

    public Task HandleErrorAsync(string errorMessage)
    {
        return HandleErrorAsync(Error.Unexpected(errorMessage));
    }

    public Task HandleErrorAsync(Error error)
    {
        _logger.LogError(error.ToString());
        _snackbar.Add($"Error: {error.Description}", Severity.Error);
        return Task.CompletedTask;
    }
}