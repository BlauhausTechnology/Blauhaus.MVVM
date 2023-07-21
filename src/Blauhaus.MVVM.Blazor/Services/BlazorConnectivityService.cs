using Blauhaus.Common.Utils.Disposables;
using Blauhaus.DeviceServices.Abstractions.Connectivity;

namespace Blauhaus.MVVM.Blazor.Services;

public class BlazorConnectivityService : BasePublisher, IConnectivityService
{
    public Task<IDisposable> SubscribeAsync(Func<ConnectionState, Task> handler, Func<ConnectionState, bool>? filter = null)
    {
        return Task.FromResult(AddSubscriber(handler, filter));
    }

    public async ValueTask<ConnectionState> GetConnectionStateAsync()
    {
        return ConnectionState.Wifi;
    }

    public bool IsConnectedToInternet => true;
    public bool IsConnectionUsingCellularData => false;
    public ConnectionState CurrentConnection => ConnectionState.Wifi;
}