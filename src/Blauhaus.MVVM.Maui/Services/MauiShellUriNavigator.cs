using Blauhaus.Analytics.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.MVVM.Abstractions.Navigation;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.Services;

public class MauiShellUriNavigator : IUriNavigator
{
    private readonly IAnalyticsLogger<MauiShellUriNavigator> _logger;
    private readonly IErrorHandler _errorHandler;
    private readonly IThreadService _threadService;
    private readonly SemaphoreSlim _semaphore = new(1);

    public MauiShellUriNavigator(
        IAnalyticsLogger<MauiShellUriNavigator> logger,
        IErrorHandler errorHandler,
        IThreadService threadService)
    {
        _logger = logger;
        _errorHandler = errorHandler;
        _threadService = threadService;
    }

    public async Task NavigateAsync(string uri)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _threadService.InvokeOnMainThreadAsync(async () =>
            {
                using (var _ = _logger.BeginTimedScope(LogLevel.Trace, "Navigated to uri {Uri}", uri)) ;
                {
                    await Shell.Current.GoToAsync(uri);
                }
            });
        }
        catch (Exception e)
        {
            await _errorHandler.HandleExceptionAsync(this, e);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}