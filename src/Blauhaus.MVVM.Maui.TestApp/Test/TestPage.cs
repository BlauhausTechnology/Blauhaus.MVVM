namespace Blauhaus.MVVM.Maui.TestApp.Test;

public class TestPage : ContentPage
{
    public TestPage()
    {
        BackgroundColor = Color.FromRgb(100,110,110);

        Content = new StackLayout
        {
            Children =
            {
                new Button
                {
                    Text = "Go to target",
                    Command = new Command(() =>
                    {
                        Shell.Current.GoToAsync(nameof(TestTargetPage));
                    })
                },
                new Button
                {
                    Text = "Go to Modal",
                    Command = new Command(() =>
                    {
                        Shell.Current.GoToAsync(nameof(ModalPage));
                    })
                }

            }
        };
    }
}