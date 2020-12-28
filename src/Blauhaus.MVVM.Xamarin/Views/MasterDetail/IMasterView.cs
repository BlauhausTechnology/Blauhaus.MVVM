using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.MasterDetail
{
    public interface IMasterView : IView
    {
        void SetContainer(MasterDetailPage masterDetailPage);
    }
}