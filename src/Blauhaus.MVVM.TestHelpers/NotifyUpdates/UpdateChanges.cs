using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.TestHelpers.Extensions;

namespace Blauhaus.MVVM.TestHelpers.NotifyUpdates
{
   
    public class UpdateChanges<TUpdate> : List<TUpdate>, IDisposable 
    {
        private readonly INotifyPropertyChanged _bindableObject;

        public UpdateChanges(INotifyUpdates bindableObject)
        {
            _bindableObject = bindableObject; 
            _bindableObject.PropertyChanged += BindableObject_PropertyChanged;
        }

        private void BindableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(INotifyUpdates.Update))
            {
                Add((TUpdate) ((INotifyUpdates)sender).Update!);
            }
        }
        
        public void Dispose()
        {
            _bindableObject.PropertyChanged -= BindableObject_PropertyChanged;
        }

        public static UpdateChanges<TUpdate> Subscribe(INotifyUpdates bindableObject)
        {
            return new UpdateChanges<TUpdate>(bindableObject);
        } 

        public UpdateChanges<TUpdate>  WaitForChangeCount(int requiredCount, int timeoutMs = 1000)
        {
            WaitForChanges(x => x.Count >= requiredCount, timeoutMs);
            return this;
        }
         
        public UpdateChanges<TUpdate>  WaitForChanges(Expression<Func<UpdateChanges<TUpdate>, bool>> predicate, int timeoutMs = 1000)
        {
            return this.WaitFor(predicate, timeoutMs); 
        }
    }
}