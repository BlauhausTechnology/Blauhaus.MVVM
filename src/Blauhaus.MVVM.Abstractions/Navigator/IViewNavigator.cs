using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Navigation;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigator;

public interface IViewNavigator
{
    Task NavigateAsync(IViewTarget viewTarget);
    Task GoBackAsync();
}

