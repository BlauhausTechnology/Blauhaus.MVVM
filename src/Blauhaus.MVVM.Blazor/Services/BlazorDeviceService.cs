using System.Globalization;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.DeviceType;
using Blauhaus.Common.ValueObjects.RuntimePlatforms;
using Blauhaus.DeviceServices.Abstractions.DeviceInfo;

namespace Blauhaus.MVVM.Blazor.Services;

public class BlazorDeviceInfoService : IDeviceInfoService
{
    private IKeyValueStore _keyValueStore;
    private string? _deviceId;
    private const string DeviceKey = "DeviceUniqueId";

    public BlazorDeviceInfoService(IKeyValueStore keyValueStore)
    {
        _keyValueStore = keyValueStore;
    }

    public IDeviceType Type { get; } = DeviceType.Unknown;
    public IRuntimePlatform Platform { get; } = RuntimePlatform.Unknown;
    public string OperatingSystemVersion { get; } = "Unknown";
    public string Manufacturer { get; }= "Unknown";
    public string Model { get; } = "Unknown";
    public string AppDataFolder { get; }= "Unknown";
    public CultureInfo CurrentCulture => CultureInfo.CurrentUICulture;

    public string DeviceUniqueIdentifier
    {
        get { return _deviceId ??= Task.Run(async () => await GetDeviceIdentifierAsync()).GetAwaiter().GetResult(); }
    }

    public async ValueTask<string> GetDeviceIdentifierAsync()
    {
        if (_deviceId == null)
        {
            _deviceId = await _keyValueStore.GetAsync(DeviceKey);
            if (string.IsNullOrEmpty(_deviceId))
            {
                _deviceId = Guid.NewGuid().ToString();
                await _keyValueStore.SetAsync(DeviceKey, _deviceId);
            }
        }
        return _deviceId;
    }
}