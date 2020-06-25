using System;
using System.Linq.Expressions;
using Blauhaus.MVVM.Xamarin.Converters;
using Blauhaus.MVVM.Xamarin.ViewElements.Sync;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class RefreshViewExtensions
    {
        public static RefreshView BindSyncCollection(this RefreshView refreshView, string nameOfSyncCollection)
        {
            refreshView.SetBinding(RefreshView.CommandProperty, new Binding(nameOfSyncCollection + ".LoadNewFromServerCommand")); 
            refreshView.SetBinding(RefreshView.IsRefreshingProperty, new Binding(nameOfSyncCollection + ".SyncStats.State", BindingMode.OneWay, new SyncCollectionIsRunningConverter())); 

            return refreshView;
        }

    }
}