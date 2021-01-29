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
            var originalCulture = GetCulture();
            var newCultureName = cultureString.Replace("_", "-");
            var newCulture = new CultureInfo(newCultureName);

            CultureInfo.CurrentUICulture = newCulture;
            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;

            _analyticsService.Trace(this, $"Set device culture from {originalCulture.DisplayName} to {newCulture.DisplayName}");
        }

        public CultureInfo GetCulture()
        {
            return CultureInfo.CurrentUICulture;
        }
    }
}