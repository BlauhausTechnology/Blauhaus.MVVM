using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Xamarin.Converters;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Content
{
    public abstract class BaseUpdateContentPage : ContentPage
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

        protected BaseUpdateContentPage(bool isUpdatable = false)
        {
            if (isUpdatable)
            {
                SetBinding(UpdateProperty, new Binding(nameof(IUpdate.Update), BindingMode.OneWay, new ActionConverter<object>(OnUpdated)));
            }
        }

        private void OnUpdated(object? update)
        {
            if (update != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                { 
                    foreach (var handler in _handlers.Where(handler => update.GetType() == handler.Key || handler.Key.IsInstanceOfType(update)))
                    {
                        handler.Value.Invoke(update);
                    }
                });
            }
        }
         
        protected void Subscribe<TUpdate>(Action<TUpdate> handler)
        {
            _handlers[typeof(TUpdate)] = obj => handler.Invoke((TUpdate) obj);
        }

        public static readonly BindableProperty UpdateProperty = BindableProperty.Create(
            propertyName: nameof(Update),
            returnType: typeof(object),
            declaringType: typeof(BaseUpdateContentPage),
            defaultValue: string.Empty);

        public object Update
        {
            get => GetValue(UpdateProperty);
            set => SetValue(UpdateProperty, value);
        }
    }
}