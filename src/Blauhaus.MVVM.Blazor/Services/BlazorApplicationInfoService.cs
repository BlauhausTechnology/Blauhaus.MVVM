using Blauhaus.DeviceServices.Abstractions.Application;

namespace Blauhaus.MVVM.Blazor.Services;

public class BlazorApplicationInfoService : IApplicationInfoService
{
    public string CurrentVersion { get; } = "Unknown"; 
}