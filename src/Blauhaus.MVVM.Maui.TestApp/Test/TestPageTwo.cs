namespace Blauhaus.MVVM.Maui.TestApp.Test;

public class TestPageTwo : ContentPage
{
    public TestPageTwo()
    {
        BackgroundColor = Color.FromRgb(0,110,0);

        Content = new StackLayout
        {
            Children =
            {
                new Button
                {
                    Text = "Go to target in tab",
                    Command = new Command(async () =>
                    {

                        await Shell.Current.GoToAsync($"//AccountItems2/{nameof(TestTargetPage)}");

                    })
                }
            }
        };
    }
}