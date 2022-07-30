using Blauhaus.MVVM.Abstractions.Navigation.NavigationService;
using Blauhaus.MVVM.Abstractions.Views;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.Navigation
{
    public class NavigationView : NavigationPage, INavigationView
    {
        private readonly INavigationService _navigationService;

        public NavigationView(INavigationService navigationService, Page rootPage, string stackName = "") : base(rootPage)
        {
            _navigationService = navigationService;
            StackName = string.IsNullOrEmpty(stackName) ? rootPage.GetType().Name : stackName;
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