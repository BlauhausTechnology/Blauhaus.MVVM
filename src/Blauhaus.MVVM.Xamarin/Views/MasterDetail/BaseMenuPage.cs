using System;
using System.Windows.Input;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Xamarin.Views.Content;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.MasterDetail
{
    public abstract class BaseMenuPage<TViewModel> : BasePage<TViewModel>, IMasterView 
        where TViewModel : IViewModel, IAppearingViewModel
    {
        private MasterDetailPage? _container;

        protected BaseMenuPage(TViewModel viewModel, string title) : base(viewModel)
        {
            Title = title;
        }


        public void SetContainer(MasterDetailPage masterDetailPage)
        {
            _container = masterDetailPage;
        }

        
        protected void ShowDetailPage(Page page)
        {
            if (_container == null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            _container.Detail = page;
            _container.IsPresented = false;
        }

        protected ICommand ShowDetailCommand(Page page)
        {
            return new Command(() => ShowDetailPage(page));
        }
    }
}