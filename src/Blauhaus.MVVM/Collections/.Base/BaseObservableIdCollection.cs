using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blauhaus.MVVM.Collections.Base
{

    public class ObservableIdCollection<T, TId> : BaseObservableIdCollection<T, TId>
        where T : class, IHasId<TId>, IAsyncInitializable<TId>
    {
        public ObservableIdCollection(IAnalyticsLogger<ObservableIdCollection<T, TId>> logger, IServiceLocator serviceLocator, IThreadService threadService, IErrorHandler errorHandler) : base(logger, serviceLocator, threadService, errorHandler)
        {
        }
    }

    public abstract class BaseObservableIdCollection<T, TId> : ObservableCollection<T> 
        where T : class, IHasId<TId>, IAsyncInitializable<TId>
    {
        private readonly IAnalyticsLogger _logger;
        private readonly IServiceLocator _serviceLocator;
        private readonly IThreadService _threadService;
        private readonly IErrorHandler _errorHandler;
        private bool _isUpdating;

        protected BaseObservableIdCollection(
            IAnalyticsLogger logger,
            IServiceLocator serviceLocator,
            IThreadService threadService,
            IErrorHandler errorHandler)
        {
            _logger = logger;
            _serviceLocator = serviceLocator;
            _threadService = threadService;
            _errorHandler = errorHandler;
        }


        public async Task UpdateAsync(IReadOnlyList<IHasId<TId>> idSources)
        {
            await UpdateAsync(idSources.Select(x => x.Id).ToArray());
        }

        public async Task OrberByAsync<TProp>(Expression<Func<T, TProp>> property, bool isAscending = true)
        {
            if (_isUpdating)
            {
                return;
            }
            _isUpdating = true;


            await _threadService.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    var orderedCollection = isAscending 
                        ? Items.OrderBy(property.Compile()).ToArray() 
                        : Items.OrderByDescending(property.Compile()).ToArray();

                    for (var i = 0; i < orderedCollection.Count(); i++)
                    {
                        
                        var existingItem = this.FirstOrDefault(x => x.Id != null && x.Id.Equals(orderedCollection[i].Id));
                        if (IndexOf(existingItem) != i)
                        {
                            _logger.LogTrace("Moving {ListItemType} with id {ListItemId} from {OldListPosition} to {ListPosition}", typeof(T).Name, orderedCollection[i].Id, IndexOf(existingItem), i);
                            Move(IndexOf(existingItem), i);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _errorHandler.HandleExceptionAsync(this, e);
                }
                finally
                {
                    _isUpdating = false;
                }
            });
        }

        public async Task<bool> UpdateAsync(IReadOnlyList<TId> sourceIds)
        {
            var somethingHasChanged = false;
            if (_isUpdating)
            {
                _logger.LogTrace("Collection of {ListItemType} will not be updated with {ListItemCount} ids because it is already busy updating", typeof(T).Name, sourceIds.Count);
                return somethingHasChanged;
            }
            _isUpdating = true;

            await _threadService.InvokeOnMainThreadAsync(async () =>
            {
                try
                {

                    var itemsToRemove = new List<T>();
                    foreach (var existingItem in this)
                    {
                        var sourceId = sourceIds.FirstOrDefault(x => Equals(x, existingItem.Id));

                        if (sourceId == null || sourceId.Equals(default(TId)))
                        {
                            somethingHasChanged = true;
                            itemsToRemove.Add(existingItem);
                            _logger.LogTrace("Removing {ListItemType} with id {ListItemId} from collection", typeof(T).Name, existingItem.Id);
                        }
                    }

                    foreach (var itemToRemove in itemsToRemove)
                    {
                        if (itemToRemove is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }

                        if (itemToRemove is IAsyncDisposable asyncDisposable)
                        {
                            await asyncDisposable.DisposeAsync();
                        }

                        Remove(itemToRemove);
                    }

                    for (var i = 0; i < sourceIds.Count; i++)
                    {

                        var existingItem = this.FirstOrDefault(x => x.Id != null && x.Id.Equals(sourceIds[i]));

                        if (existingItem == null)
                        {
                            
                            somethingHasChanged = true;

                            var newItem = _serviceLocator.Resolve<T>();
                            await newItem.InitializeAsync(sourceIds[i]);
                            InsertItem(i, newItem);
                        }

                        else
                        {
                            if (existingItem is IAsyncReloadable reloadable)
                            {
                                _logger.LogTrace("Reloading {ListItemType} with id {ListItemId}", typeof(T).Name, sourceIds[i]);
                                await reloadable.ReloadAsync();
                            }

                            if (IndexOf(existingItem) != i)
                            {
                                somethingHasChanged = true;
                                _logger.LogTrace("Moving {ListItemType} with id {ListItemId} from {OldListPosition} to {ListPosition}", typeof(T).Name, sourceIds[i], IndexOf(existingItem), i);
                                Move(IndexOf(existingItem), i);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    await _errorHandler.HandleExceptionAsync(this, e);
                }
                finally
                {
                    _isUpdating = false;
                }

            });

            return somethingHasChanged;
        }
         
    }
}