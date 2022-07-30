using System;
using System.Collections.Generic;
using Blauhaus.MVVM.Abstractions.Navigator;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Navigator;

public class NavigationRegister : INavigationRegister
{
    private readonly Dictionary<string, Type> _viewTypes = new();
    private readonly Dictionary<string, Type> _viewModelTypes = new();

    public void Register<TView, TViewModel>(string name)
        where TView : IView
        where TViewModel : IViewModel
    {
        _viewTypes[name] = typeof(TView);
        _viewModelTypes[name] = typeof(TViewModel);
    }
     
    public Type? GetViewType(ViewIdentifier viewIdentifier)
    {
        return _viewTypes.TryGetValue(viewIdentifier.Name, out var viewType) ? viewType : null;
    }

    public Type? GetViewModelType(ViewIdentifier viewIdentifier)
    {
        return _viewModelTypes.TryGetValue(viewIdentifier.Name, out var viewModelType) ? viewModelType : null;
    }
}