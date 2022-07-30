using Blauhaus.Analytics.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Maui.Services.Application;
using Application = Microsoft.Maui.Controls.Application;

public class MauiApplicationProxy : IMauiApplicationProxy
{
    private readonly IAnalyticsLogger<MauiApplicationProxy> _logger;

    public MauiApplicationProxy(
        IAnalyticsLogger<MauiApplicationProxy> logger)
    {
        _logger = logger;
    }

    public Page? MainPage
    {
        get => Application.Current is null ? null : Application.Current.MainPage;
        set
        {
            if (Application.Current is not null)
            {
                if (value is null)
                {
                    _logger.LogTrace("Setting maui main page to null");
                }
                else if (Application.Current.MainPage is not null)
                {
                    _logger.LogTrace("Replacing maui main page {OldMainPage} with new {NewMainPage}", Application.Current.MainPage.GetType().Name, value.GetType().Name);
                }
                else
                {
                    _logger.LogTrace("Setting maui main page to {NewMainPage}", value.GetType().Name);
                }

                Application.Current.MainPage = value;
            }
        }
    }
}