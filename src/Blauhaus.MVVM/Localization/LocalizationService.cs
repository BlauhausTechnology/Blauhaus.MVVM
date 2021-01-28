using System.Globalization;
using System.Threading;
using Blauhaus.MVVM.Abstractions.Localization;

namespace Blauhaus.MVVM.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public void SetCulture(string cultureString)
        {
            var currentCulture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        public CultureInfo GetCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }
    }
}