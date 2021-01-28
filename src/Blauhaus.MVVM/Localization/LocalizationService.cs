using System.Globalization;
using System.Threading;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.MVVM.Abstractions.Localization;

namespace Blauhaus.MVVM.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IAnalyticsService _analyticsService;

        public LocalizationService(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public void SetCulture(string cultureString)
        {
            var originalCulture = GetCulture().DisplayName;
            var currentCulture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;

            _analyticsService.Trace(this, $"Set device culture from {originalCulture} to {currentCulture.DisplayName}");
        }

        public CultureInfo GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}