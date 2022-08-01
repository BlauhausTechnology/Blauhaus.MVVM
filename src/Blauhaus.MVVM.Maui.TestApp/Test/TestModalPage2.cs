
namespace Blauhaus.MVVM.Maui.TestApp.Test;

public class ModalPage2 : ContentPage
{
    public ModalPage2()
    {
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);
        BackgroundColor = Color.FromRgb(100,110,0);
    }
}