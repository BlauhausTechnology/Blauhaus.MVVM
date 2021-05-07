using System.Collections.Generic;

namespace Blauhaus.MVVM.Abstractions.ViewModels.Tabs
{
    public interface ITabbedViewModel
    {
        IReadOnlyList<TabDefinition> TabDefinitions { get; }
    }
}