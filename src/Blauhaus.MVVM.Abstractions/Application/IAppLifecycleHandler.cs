using System.Threading.Tasks;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Abstractions.Application
{
    public interface IAppLifecycleHandler
    {
        Task<Response> HandleAppStateChangeAsync(AppLifecycleState state);
    }
}