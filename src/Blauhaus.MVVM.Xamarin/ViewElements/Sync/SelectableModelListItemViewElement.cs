using System;
using System.Windows.Input;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public class SelectableModelListItemViewElement : ModelListItemViewElement
    { 

        public ICommand ItemSelectedCommand { get; protected set; }
    }
}