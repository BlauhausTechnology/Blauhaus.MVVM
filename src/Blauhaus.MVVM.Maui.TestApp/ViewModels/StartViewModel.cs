using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Maui.TestApp.ViewModels.Base;

namespace Blauhaus.MVVM.Maui.TestApp.ViewModels;

public class StartViewModel : BaseTestAppViewModel
{
    public StartViewModel(IServiceLocator serviceLocator) 
        : base(serviceLocator)
    {
        Title = "Start!";
    }
}