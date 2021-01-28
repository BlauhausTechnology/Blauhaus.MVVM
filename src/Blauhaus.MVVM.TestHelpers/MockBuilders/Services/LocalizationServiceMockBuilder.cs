using System.Globalization;
using Blauhaus.MVVM.Abstractions.Localization;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.MVVM.TestHelpers.MockBuilders.Services
{
    public class LocalizationServiceMockBuilder : BaseMockBuilder<LocalizationServiceMockBuilder, ILocalizationService>
    {
        public LocalizationServiceMockBuilder Where_GetCulture_returns(CultureInfo cultureInfo)
        {
            Mock.Setup(x => x.GetCulture()).Returns(cultureInfo);
            return this;
        }
    }
}