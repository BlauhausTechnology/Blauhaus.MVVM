using System.Threading.Tasks;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Navigation.FormsApplicationProxy
{
    public interface IFormsApplicationProxy
    {
        void SetMainPage<TPage>(TPage page) where TPage : Page;
        Task<bool> DisplayAlertAsync(string title, string message, string cancel, string accept);
        Task DisplayAlertAsync(string title, string message, string cancel);
        Task<string> DisplayActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
        Task<string?> DisplayPromptAsync(string title, string message, string cancelButtonText = "Cancel", string acceptButtonText = "OK");

    }
}