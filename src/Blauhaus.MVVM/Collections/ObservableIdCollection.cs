using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{

    public class ObservableIdCollection<T, TId> : ObservableCollection<T> 
        where T : class, IHasId<TId>, IAsyncInitializable<TId>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public ObservableIdCollection(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
         
        public async Task UpdateAsync(IReadOnlyList<TId> sourceIds)
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
            finally
            {
                _semaphore.Release();
            }

        }
    }
}