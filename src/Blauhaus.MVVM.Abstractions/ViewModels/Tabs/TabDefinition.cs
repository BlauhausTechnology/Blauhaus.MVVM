using System;

namespace Blauhaus.MVVM.Abstractions.ViewModels.Tabs
{
    public class TabDefinition
    {
        public TabDefinition(Type viewModelType, string title, string? navigationStackName)
        {
            ViewModelType = viewModelType;
            Title = title;
            NavigationStackName = navigationStackName;
        }

        public Type ViewModelType { get; }
        public string Title { get; }
        public string? NavigationStackName { get; }
    }
}