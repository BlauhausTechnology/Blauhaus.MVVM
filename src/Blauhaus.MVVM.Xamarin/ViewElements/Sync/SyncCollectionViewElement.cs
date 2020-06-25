using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.DeviceServices.Abstractions.Connectivity;
using Blauhaus.Domain.Client.Sync;
using Blauhaus.Domain.Common.CommandHandlers.Sync;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.MVVM.Abstractions.Bindable;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Xamarin.Commands.ExecutingCommands.ExecutingNoValueCommands;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public class SyncCollectionViewElement<TModel, TListItem, TSyncCommand> : BaseBindableObject, ISyncCollectionViewElement<TListItem, TSyncCommand>
        where TModel : class, IClientEntity
        where TListItem : ModelListItemViewElement, new()
        where TSyncCommand : SyncCommand, new()
    {
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IConnectivityService _connectivityService;
        private readonly ISyncClient<TModel, TSyncCommand> _syncClient;
        private readonly IModelViewElementUpdater<TModel, TListItem> _listItemUpdater;
        private IDisposable? _syncClientConnection;

        public SyncCollectionViewElement(
            IErrorHandlingService errorHandlingService, 
            IAnalyticsService analyticsService,
            IConnectivityService connectivityService,
            ISyncClient<TModel, TSyncCommand> syncClient,
            IModelViewElementUpdater<TModel, TListItem> listItemUpdater)
        {
            _errorHandlingService = errorHandlingService;
            _analyticsService = analyticsService;
            _connectivityService = connectivityService;
            _syncClient = syncClient;
            _listItemUpdater = listItemUpdater;

            SyncCommand = new TSyncCommand();
            SyncStats = new SyncStats();
            SyncRequirement = ClientSyncRequirement.Batch;
            ListItems = new ObservableCollection<TListItem>();

            BindingBase.EnableCollectionSynchronization(ListItems, null, ObservableCollectionCallback);
            AppearingCommand = new ExecutingCommand(errorHandlingService, Initialize);
        }
        

        public ClientSyncRequirement SyncRequirement { get; set; }
        public ObservableCollection<TListItem> ListItems { get; }
        public TSyncCommand SyncCommand { get; }
        public IExecutingCommand AppearingCommand { get; }
        public SyncStats SyncStats { get; }


        private void Initialize()
        {
            if (_syncClientConnection == null)
            {
                _syncClientConnection = _syncClient.Connect(SyncCommand, SyncRequirement, SyncStats)
                    .Subscribe(OnNext, OnError);
            }
            else
            {
                _syncClient.LoadNewFromClient();
            }
        }

        private void OnNext(TModel nextModel)
        {
            try
            {
                var existingElement = ListItems.FirstOrDefault(x => x.Id == nextModel.Id);
                if (existingElement == null)
                {
                    AddNewElement(nextModel);
                }
                else
                {
                    UpdateExistingElement(existingElement, nextModel);
                }
            }
            catch (Exception e)
            {
                _errorHandlingService.HandleExceptionAsync(this, e);
            }
        }

        private void UpdateExistingElement(TListItem existingElement, TModel model)
        {
            existingElement = _listItemUpdater.Update(model, existingElement);
            existingElement.ModifiedAtTicks = model.ModifiedAtTicks;
                
            var currentIndex = ListItems.IndexOf(existingElement);
            var newIndex = 0;
            var numberOfItems = ListItems.Count;
                
            for (var i = 0; i < numberOfItems; i++)
            {
                if (existingElement.ModifiedAtTicks > ListItems[i].ModifiedAtTicks)
                {
                    newIndex = i;
                    break;
                }
            }

            ListItems.Move(currentIndex, newIndex);
        }

        //todo add IsVisible property and remove / do not add if false;
        private void AddNewElement(TModel model)
        {
            var newListItem = _listItemUpdater.Update(model, new TListItem
            {
                Id = model.Id,
                ModifiedAtTicks = model.ModifiedAtTicks
            });

            var isAdded = false;
            var numberOfItems = ListItems.Count;
            for (var i = 0; i < numberOfItems; i++)
            {
                if (newListItem.ModifiedAtTicks > ListItems[i].ModifiedAtTicks)
                {
                    ListItems.Insert(i, newListItem);
                    isAdded = true;
                    break;
                }
            }
            if (!isAdded)
            {
                ListItems.Add(newListItem);
            }
        }

        private void OnError(Exception exception)
        {
            _errorHandlingService.HandleExceptionAsync(this, exception);
        }


        private static void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeaccess)
        {
            if (writeaccess)
            {
                lock (collection)
                {
                    accessMethod?.Invoke();
                }
            }
        }

    }
}