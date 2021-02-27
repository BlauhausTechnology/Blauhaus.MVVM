using System.Threading.Tasks;

namespace Blauhaus.MVVM.Abstractions.Application
{
    public interface IAppLifecycleHandler
    {
        Task HandleAppLifecycleState(AppLifecycleState state);
    }
}