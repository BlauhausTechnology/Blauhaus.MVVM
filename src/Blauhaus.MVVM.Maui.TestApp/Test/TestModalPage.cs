
namespace Blauhaus.MVVM.Maui.TestApp.Test;

public class ModalPage : ContentPage
{
    public ModalPage()
    {
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);
        BackgroundColor = Color.FromRgb(100,0,0);

        Content = new StackLayout
        {
            Children =
            {
                new Button
                {
                    Text = "Go to Modal2",
                    Command = new Command(() =>
                    {
                        Shell.Current.GoToAsync(nameof(ModalPage2));
                    })
                }

            }
        };
    }
}