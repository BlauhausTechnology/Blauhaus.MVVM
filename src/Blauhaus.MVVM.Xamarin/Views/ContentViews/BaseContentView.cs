using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Xamarin.Converters;
using Blauhaus.MVVM.Xamarin.Views.Content;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.ContentViews
{

    public abstract class BaseContentView<TViewModel> : BaseContentView
    {
        protected readonly TViewModel ViewModel;

        protected BaseContentView(TViewModel viewModel) 
            : base(viewModel is INotifyUpdates)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }
    }


    public abstract class BaseContentView : ContentView
    {
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

        protected BaseContentView(bool isUpdatable = true)
        {
            if (isUpdatable)
            {
                SetBinding(UpdateProperty, new Binding(nameof(INotifyUpdates.Update), BindingMode.OneWay, new ActionConverter<object>(OnUpdated)));
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

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is IAppearingViewModel appear)
            {
                appear.AppearCommand.Execute();
            }

            if (BindingContext is INotifyUpdates update)
            {
                OnUpdated(update.Update);
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