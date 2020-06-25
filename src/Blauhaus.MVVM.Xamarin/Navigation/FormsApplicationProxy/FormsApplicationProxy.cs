using System.Threading.Tasks;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy
{
    public class FormsApplicationProxy : IFormsApplicationProxy
    {

        public void SetMainPage<TPage>(TPage page) where TPage : Page
        {
            Application.Current.MainPage = page;
        }

        public Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task GoToAsync(string route, bool animate)
        {
            return Shell.Current.GoToAsync(route, animate);
        }
    }
}