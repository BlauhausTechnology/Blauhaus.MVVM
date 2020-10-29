using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Navigation
{
    public class NavigationView : NavigationPage, INavigationView
    {
        private readonly INavigationService _navigationService;

        public NavigationView(INavigationService navigationService, Page rootPage) : base(rootPage)
        {
            _navigationService = navigationService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _navigationService.SetCurrentNavigationView(this);
        }
    }
}