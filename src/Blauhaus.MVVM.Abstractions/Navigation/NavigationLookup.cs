using System;
using System.Collections.Generic;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.Views;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public class NavigationLookup : INavigationLookup
    {

        private readonly Dictionary<string, Type> _viewTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> _navigationViewTypes = new Dictionary<string, Type>();

        public void Register<TView, TViewModel>()
            where TView : IView
            where TViewModel : IViewModel
        {
            _viewTypes[GetViewModelIdentifier<TViewModel>()] = typeof(TView);
        }
        
        public Type? GetViewType<TViewModel>() where TViewModel : IViewModel
        {
            return _viewTypes.TryGetValue(GetViewModelIdentifier<TViewModel>(), out var viewType) ? viewType : null;
        }

        public Type? GetViewType(Type viewModelType)
        {
            return _viewTypes.TryGetValue(viewModelType.FullName, out var viewType) ? viewType : null;
        }

        private static string GetViewModelIdentifier<TViewModel>() where TViewModel : IViewModel
        {
            return typeof(TViewModel).FullName;
        }

    }
}