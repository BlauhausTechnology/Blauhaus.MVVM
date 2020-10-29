using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Xamarin.Views.Content;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.MasterDetail
{
    public abstract class BaseMasterPage<TViewModel> : BaseAppearingContentPage<TViewModel>, IMasterView 
        where TViewModel : IViewModel, IAppear
    {
        private MasterDetailPage _container;

        protected BaseMasterPage(TViewModel viewModel, string title) : base(viewModel)
        {
            Title = title;
        }


        public void SetContainer(MasterDetailPage masterDetailPage)
        {
            _container = masterDetailPage;
        }
    }
}