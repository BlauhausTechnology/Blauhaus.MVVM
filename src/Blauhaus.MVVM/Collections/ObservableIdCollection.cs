using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Contracts;

namespace Blauhaus.MVVM.Collections
{

    public class ObservableIdCollection<T, TId> : ObservableCollection<T> where T : class, IHasId<TId>, IAsyncInitializable<TId>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public ObservableIdCollection(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public async Task UpdateAsync(TId[] sourceIds)
        {
            await _semaphore.WaitAsync();
            try
            {
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
                    Remove(itemToRemove);
                }

                for (var i = 0; i < sourceIds.Length; i++)
                {

                    var existingItem = this.FirstOrDefault(x => x.Id != null && x.Id.Equals(sourceIds[i]));

                    if (existingItem == null)
                    {
                        var newItem = _serviceLocator.Resolve<T>();
                        await newItem.InitializeAsync(sourceIds[i]);
                        InsertItem(i, newItem);
                    }

                    else if (IndexOf(existingItem) != i)
                    {
                        Move(IndexOf(existingItem), i);
                    }

                }
            }
            finally
            {
                _semaphore.Release();
            }

        }
    }
}