using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class CollectionViewExtensions
    {
        public static CollectionView BindSyncCollection(this CollectionView collectionView, string nameOfSyncCollection)
        {
            collectionView.SetBinding(ItemsView.ItemsSourceProperty, $"{nameOfSyncCollection}.ListItems");
            return collectionView;
        }

    }
}