using System.Collections.ObjectModel;
using System.ComponentModel;
using Blauhaus.Domain.Client.Sync;
using Blauhaus.Domain.Common.CommandHandlers.Sync;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public interface ISyncCollectionViewElement<TListItem, TSyncCommand> : INotifyPropertyChanged, IAppearing
        where TListItem : ModelListItemViewElement, new()
        where TSyncCommand : SyncCommand, new()
    {
        public ClientSyncRequirement SyncRequirement { get; set; }
        public ObservableCollection<TListItem> ListItems { get; }
        public TSyncCommand SyncCommand { get; }
        public SyncStats SyncStats { get; }
    }
}