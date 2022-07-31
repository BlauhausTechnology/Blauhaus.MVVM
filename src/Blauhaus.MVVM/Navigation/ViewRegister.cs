using System;
using System.Collections.Generic;
using Blauhaus.MVVM.Abstractions.TargetNavigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Navigation;

public class ViewRegister : IViewRegister
{

    private readonly Dictionary<string, Type> _viewTypes = new();

    public void RegisterView<TView>(ViewIdentifier identifier)
        where TView : IView
    {
        _viewTypes[identifier.Name] = typeof(TView);
    }
     
    public Type GetViewType(ViewIdentifier viewIdentifier)
    {
        if (!_viewTypes.TryGetValue(viewIdentifier.Name, out var viewType))
        {
            throw new InvalidOperationException("No view type registered for " + viewIdentifier.Name);
        }
        return viewType;
    }
}