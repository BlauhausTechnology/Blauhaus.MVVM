using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Extensions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.Errors.Handler;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableIdCollection<T, TId> : ObservableCollection<T> 
        where T : class, IHasId<TId>, IAsyncInitializable<TId>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IThreadService _threadService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IErrorHandler _errorHandler;
        private bool _isUpdating;

        public ObservableIdCollection(
            IServiceLocator serviceLocator,
            IThreadService threadService,
            IAnalyticsService analyticsService,
            IErrorHandler errorHandler)
        {
            _serviceLocator = serviceLocator;
            _threadService = threadService;
            _analyticsService = analyticsService;
            _errorHandler = errorHandler;
        }

        public bool LogDebugMessages { get; set; }

        public async Task UpdateAsync(IReadOnlyList<IHasId<TId>> idSources)
        {
            await UpdateAsync(idSources.Select(x => x.Id).ToArray());
        }

        public async Task UpdateAsync(IReadOnlyList<TId> sourceIds)
        {

            if (_isUpdating)
            {
                _analyticsService.TraceVerbose(this, $"{typeof(TId).Name} collection will not be updated with {sourceIds.Count} ids because it is already busy");
                return;
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
                            itemsToRemove.Add(existingItem);
                            
                            if(LogDebugMessages) 
                                _analyticsService.Debug($"Removing {typeof(T).Name} with id {existingItem.Id} from collection");
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
                            
                            if(LogDebugMessages) 
                                _analyticsService.Debug($"Adding {typeof(T).Name} with id {sourceIds[i]} to collection");

                            var newItem = _serviceLocator.Resolve<T>();
                            await newItem.InitializeAsync(sourceIds[i]);
                            InsertItem(i, newItem);
                        }

                        else
                        {
                            if (existingItem is IAsyncReloadable reloadable)
                            {
                                if(LogDebugMessages) 
                                    _analyticsService.Debug($"Reloading {typeof(T).Name} with id {sourceIds[i]}");

                                await reloadable.ReloadAsync();
                            }

                            if (IndexOf(existingItem) != i)
                            {
                                if(LogDebugMessages) 
                                    _analyticsService.Debug($"Moving {typeof(T).Name} with id {sourceIds[i]} from {IndexOf(existingItem)} to {i}");

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
        }
         
    }
}