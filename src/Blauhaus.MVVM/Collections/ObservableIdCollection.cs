using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly SemaphoreSlim _semaphore = new(1);

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

        public async Task UpdateAsync(IReadOnlyList<IHasId<TId>> idSources)
        {
            await UpdateAsync(idSources.Select(x => x.Id).ToArray());
        }

        public async Task UpdateAsync(IReadOnlyList<TId> sourceIds)
        {
            await _threadService.InvokeOnMainThreadAsync(async () =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    var tasks = new List<Task>();

                    var itemsToRemove = new List<T>();
                    foreach (var existingItem in this)
                    {
                        var sourceId = sourceIds.FirstOrDefault(x => Equals(x, existingItem.Id));

                        if (sourceId == null || sourceId.Equals(default(TId)))
                        {
                            itemsToRemove.Add(existingItem);
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
                            var newItem = _serviceLocator.Resolve<T>();
                            tasks.Add(newItem.InitializeAsync(sourceIds[i]));
                            InsertItem(i, newItem);
                        }

                        else
                        {

                            if (existingItem is IAsyncReloadable reloadable)
                            {
                                tasks.Add(reloadable.ReloadAsync());
                            }

                            if (IndexOf(existingItem) != i)
                            {
                                Move(IndexOf(existingItem), i);
                            }
                        }
                    }

                    if (tasks.Any())
                    {
                        await Task.WhenAll(tasks);
                    }
                }
                catch (Exception e)
                {
                    await _errorHandler.HandleExceptionAsync(this, e);
                }
                finally
                {
                    _semaphore.Release();
                }
            });
        }
    }
}