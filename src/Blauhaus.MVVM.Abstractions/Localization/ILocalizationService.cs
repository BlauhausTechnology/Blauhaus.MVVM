using System.Globalization;

namespace Blauhaus.MVVM.Abstractions.Localization
{
    public interface ILocalizationService
    {
        void SetCulture(string cultureString);
        CultureInfo GetCulture();
    }
}