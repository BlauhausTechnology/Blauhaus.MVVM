using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Navigation
{
    public class NavigationView : NavigationPage, INavigationView
    {
        private readonly INavigationService _navigationService;

        public NavigationView(INavigationService navigationService, Page rootPage, string name = "") : base(rootPage)
        {
            _navigationService = navigationService;
            StackName = string.IsNullOrEmpty(name) ? rootPage.GetType().Name : name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _navigationService.SetCurrentNavigationView(this);
        }

        public string StackName { get; }
        public bool IsCurrent { get; set; }
    }
}