using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Push.Abstractions.Client;
using Blauhaus.Push.Abstractions.Common.Notifications;

namespace Blauhaus.MVVM.Blazor.DummyServices;

public class DummyPushNotificationsClientService : BasePublisher, IPushNotificationsClientService
{
    public Task<IDisposable> SubscribeAsync(Func<IPushNotification, Task> handler, Func<IPushNotification, bool>? filter = null)
    {
        return Task.FromResult(AddSubscriber(handler, filter));
    }

    public ValueTask<string> GetPushNotificationServiceHandleAsync()
    {
        return new ValueTask<string>(string.Empty);
    }

    public event EventHandler<NewNotificationEventArgs>? NewNotificationEvent;
}