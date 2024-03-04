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
        public Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public Task<string?> DisplayPromptAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK")
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message, acceptButtonText, cancelButtonText);
        }
         
    }
}