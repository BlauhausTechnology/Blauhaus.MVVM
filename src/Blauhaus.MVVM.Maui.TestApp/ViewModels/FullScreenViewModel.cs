using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.ExecutingCommands.ExecutingNoParameterCommands;
using Blauhaus.MVVM.Maui.TestApp.Navigation;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class FullScreenViewModel : BaseTestAppViewModel
{
    public FullScreenViewModel(IServiceLocator serviceLocator, INavigator navigator) : base(serviceLocator, navigator)
    {
        NavigateLoadingCommand = Navigate(AppNavigation.Loading);
    }

    public IExecutingCommand NavigateLoadingCommand { get; }

}