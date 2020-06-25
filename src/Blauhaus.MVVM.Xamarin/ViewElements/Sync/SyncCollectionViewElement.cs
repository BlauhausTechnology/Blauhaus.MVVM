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
using Blauhaus.MVVM.Abstractions.ErrorHandling;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public class SyncCollectionViewElement<TModel, TViewElement, TSyncCommand> : ObservableCollection<TViewElement> where TModel : class, IClientEntity
        where TViewElement : ModelListItemViewElement, new()
        where TSyncCommand : SyncCommand, new()
    {
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IConnectivityService _connectivityService;
        private readonly ISyncClient<TModel, TSyncCommand> _syncClient;
        private readonly IModelViewElementUpdater<TModel, TViewElement> _listItemUpdater;

        public SyncCollectionViewElement(
            IErrorHandlingService errorHandlingService, 
            IAnalyticsService analyticsService,
            IConnectivityService connectivityService,
            ISyncClient<TModel, TSyncCommand> syncClient,
            IModelViewElementUpdater<TModel, TViewElement> listItemUpdater)
        {
            _errorHandlingService = errorHandlingService;
            _analyticsService = analyticsService;
            _connectivityService = connectivityService;
            _syncClient = syncClient;
            _listItemUpdater = listItemUpdater;

            SyncCommand = new TSyncCommand();
            SyncStats = new SyncStats();
            SyncRequirement = ClientSyncRequirement.Batch;

            BindingBase.EnableCollectionSynchronization(this, null, ObservableCollectionCallback);
            InitializeCommand = new Command(Initialize);
        }
        

        public TSyncCommand SyncCommand { get; }
        public ICommand InitializeCommand { get; }
        public SyncStats SyncStats { get; }
        public ClientSyncRequirement SyncRequirement { get; set; }


        private void Initialize()
        {
            _syncClient.Connect(SyncCommand, SyncRequirement, SyncStats)
                .Subscribe(OnNext, OnError);
        }

        private void OnNext(TModel nextModel)
        {
            try
            {
                var existingElement = this.FirstOrDefault(x => x.Id == nextModel.Id);
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

        private void UpdateExistingElement(TViewElement existingElement, TModel model)
        {
            existingElement = _listItemUpdater.Update(model, existingElement);
            existingElement.ModifiedAtTicks = model.ModifiedAtTicks;
                
            var currentIndex = IndexOf(existingElement);
            var newIndex = 0;
            var numberOfItems = Count;
                
            for (var i = 0; i < numberOfItems; i++)
            {
                if (existingElement.ModifiedAtTicks > this[i].ModifiedAtTicks)
                {
                    newIndex = i;
                    break;
                }
            }

            Move(currentIndex, newIndex);
        }

        private void AddNewElement(TModel model)
        {
            var newListItem = _listItemUpdater.Update(model, new TViewElement
            {
                Id = model.Id,
                ModifiedAtTicks = model.ModifiedAtTicks
            });

            var isAdded = false;
            var numberOfItems = Count;
            for (var i = 0; i < numberOfItems; i++)
            {
                if (newListItem.ModifiedAtTicks > this[i].ModifiedAtTicks)
                {
                    Insert(i, newListItem);
                    isAdded = true;
                    break;
                }
            }
            if (!isAdded)
            {
                Add(newListItem);
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