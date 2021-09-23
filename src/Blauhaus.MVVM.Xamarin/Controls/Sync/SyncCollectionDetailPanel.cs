using System;
using Blauhaus.Domain.Client.Sync.Old.Client;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.Forms;

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
            _stateLabel.Bind($"{collectionName}.SyncStatusHandler.{nameof(SyncStatusHandler.State)}");
            _messageLabel.Bind($"{collectionName}.SyncStatusHandler.{nameof(SyncStatusHandler.StatusMessage)}");
            
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