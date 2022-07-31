using System.Threading.Tasks;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.TargetNavigation;

public interface IPlatformNavigator
{
    IView? GetCurrentMainView();
    void SetCurrentMainView(IView view);
}