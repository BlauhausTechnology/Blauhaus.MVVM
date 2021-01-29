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
            var newCulture = new CultureInfo(cultureString);

            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture; 
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;

            _analyticsService.Trace(this, $"Set device culture from {originalCulture} to {newCulture.DisplayName}");
        }

        public CultureInfo GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}