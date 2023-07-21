using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.Abstractions;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Blazor.Services
{
    public class BlazorWebAssemblyKeyValueStore : IKeyValueStore
    {
        private readonly IAnalyticsLogger<BlazorWebAssemblyKeyValueStore> _logger;
        private readonly ILocalStorageService _localStorageService;

        public BlazorWebAssemblyKeyValueStore(
            IAnalyticsLogger<BlazorWebAssemblyKeyValueStore> logger,
            ILocalStorageService localStorageService)
        {
            _logger = logger;
            _localStorageService = localStorageService;
        }

        public async Task<string> GetAsync(string key)
        {
            _logger.LogTrace("Retrieved value for {ValueName} from secure storage", key);
            return await _localStorageService.GetItemAsStringAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            _logger.LogTrace("Saved value for {ValueName} to secure storage", key);
            await _localStorageService.SetItemAsStringAsync(key, value);
        }

        public bool Remove(string key)
        {
            _logger.LogTrace("Removed value for {ValueName} from secure storage", key);
            _localStorageService.RemoveItemAsync(key);
            return true;
        }
    }
}