using System.Globalization;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.MVVM.Abstractions.Localization;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IAnalyticsLogger _logger;

        public LocalizationService(IAnalyticsLogger<LocalizationService> logger)
        {
            _logger = logger;
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

            _logger.LogInformation("Set device culture from {OldCulture} to {NewCulture}", originalCulture.DisplayName, newCulture.DisplayName);
        }

        public CultureInfo GetCulture()
        {
            return CultureInfo.CurrentUICulture;
        }
    }
}