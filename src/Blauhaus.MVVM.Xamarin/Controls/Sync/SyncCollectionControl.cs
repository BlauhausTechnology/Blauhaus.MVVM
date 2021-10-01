//using Blauhaus.MVVM.Xamarin.Extensions;
//using Xamarin.Forms;

//namespace Blauhaus.MVVM.Xamarin.Controls.Sync
//{
//    public class SyncCollectionControl : RefreshView
//    {

//        public SyncCollectionControl()
//        {
//            CollectionView = new CollectionView();
//            Content = CollectionView;
//        }

//        public CollectionView CollectionView { get; }

//        public SyncCollectionControl Bind(string syncCollectionName)
//        {
//            CollectionView.BindSyncCollection(syncCollectionName);
//            this.BindSyncCollection(syncCollectionName);
//            return this;
//        }
//    }
//}