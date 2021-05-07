using System;
using System.Collections.Generic;
using System.Linq;
using Blauhaus.DeviceServices.Abstractions.Thread;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.MonoGame.Games;
using Blauhaus.MVVM.Services;

namespace Blauhaus.MVVM.MonoGame.Screens
{
    public class BaseUpdateScreen<TViewModel> : BaseScreen<TViewModel>
    {
        
        private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

        private readonly IThreadService _threadService;

        
        public BaseUpdateScreen(IScreenGame game, TViewModel viewModel) : base(game, viewModel)
        {
            _threadService = AppServiceLocator.Resolve<IThreadService>();
        }
        
        
        public override void LoadContent()
        {
            if (ViewModel is INotifyUpdates publisher)
            {
                publisher.PropertyChanged += Publisher_PropertyChanged;
            }
            base.LoadContent();
        }
        public override void UnloadContent()
        {
            if (ViewModel is INotifyUpdates publisher)
            {
                publisher.PropertyChanged -= Publisher_PropertyChanged;
            }
            base.UnloadContent();
        }

        protected void Subscribe<TUpdate>(Action<TUpdate> handler)
        {
            _handlers[typeof(TUpdate)] = obj => handler.Invoke((TUpdate) obj);
        }

        private void Publisher_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel is INotifyUpdates publisher && e.PropertyName == nameof(INotifyUpdates.Update))
            {
                var update = publisher.Update;
                if (update != null)
                {
                    _threadService.InvokeOnMainThread(() =>
                    {
                        foreach (var handler in _handlers.Where(handler => update.GetType() == handler.Key || handler.Key.IsInstanceOfType(update)))
                        {
                            handler.Value.Invoke(update);
                        }
                    });
                }
            }
        }
    }
}