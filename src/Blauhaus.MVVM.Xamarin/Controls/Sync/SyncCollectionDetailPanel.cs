using System;
using Blauhaus.MVVM.Xamarin.Extensions;
using Blauhaus.MVVM.Xamarin.ViewElements.Sync;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace Blauhaus.MVVM.Xamarin.Controls.Sync
{
    public class SyncCollectionDetailPanel : StackLayout
    {
        private readonly Label _stateLabel;
        private readonly Label _messageLabel;

        public SyncCollectionDetailPanel()
        {
            _stateLabel = new Label();
            _messageLabel = new Label();

            Children.Add(_stateLabel);
            Children.Add(_messageLabel);
        }

        public SyncCollectionDetailPanel Bind(string collectionName)
        {
            _stateLabel.Bind($"{collectionName}.SyncStats.{nameof(SyncStats.State)}");
            _messageLabel.Bind($"{collectionName}.SyncStats.{nameof(SyncStats.StatusMessage)}");
            
            return this;
        }
         
        public View LabelStyle<T>(T styles, Func<T, Style<Label>> labelStyle)
        {
            _stateLabel.Style(labelStyle.Invoke(styles));
            _messageLabel.Style(labelStyle.Invoke(styles));
            return this;
        }
    }
}