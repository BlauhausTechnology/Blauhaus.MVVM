using Blauhaus.MVVM.Abstractions.TargetNavigation;
using System.Runtime.CompilerServices;

namespace Blauhaus.MVVM.Maui.TestApp.Navigation;

public static class AppContainers
{
    public static NavigationContainerIdentifier MainAppContainer => NavigationContainerIdentifier.Create();


}


public class MainContainerTarget : NavigationTarget
{
    public MainContainerTarget(string name, string? payload) : base(AppContainers.MainAppContainer, name, payload)
    {
    }

    public static NavigationTarget Create(string? payload = null, [CallerMemberName] string name = "") 
        => new MainContainerTarget(name, payload);
}