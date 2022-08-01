using CommunityToolkit.Maui.Markup;

namespace Blauhaus.MVVM.Maui.TestApp.Test;

public class TestShell : Shell
{
    //tier one
    public TestShell()
    {
        BackgroundColor = Color.FromRgb(100,110,220);


        Routing.RegisterRoute(nameof(TestTargetPage), typeof(TestTargetPage));
        Routing.RegisterRoute(nameof(ModalPage), typeof(ModalPage));
        Routing.RegisterRoute(nameof(ModalPage2), typeof(ModalPage2));

        ItemTemplate = new DataTemplate(() =>
        {
            return new Label().Bind("Title");
        });

        MenuItemTemplate = new DataTemplate(() =>
        {
            return new Label().Bind("Text");
        });

        Items.Add(new MenuItem
        {
            Text = "Click me",
            Command = new Command(() =>
            {
                DisplayAlert("Thanks!", "don't do it again", "OK");
            })
        });

        Items.Add(new FlyoutItem
        {
            Title = "Some Items",
            Route = "AccountItems",
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {
                new ShellContent
                {
                    Title = "Shell item one",
                    ContentTemplate = new DataTemplate(typeof(TestPageTwo)),
                },

            }
        });

        Items.Add(new FlyoutItem
        {
            Title = "Tabs",
            Route = "AccountItems3",
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {
                new Tab
                {
                    Icon = "menu_dark.png",
                    Title = "Tab1",
                    Items =
                    {
                        new ShellContent
                        {
                            Title = "SubTab1",
                            Icon = "menu_dark.png",
                            ContentTemplate = new DataTemplate(typeof(TestPageTwo))
                        },
                        new ShellContent
                        {
                            Title = "SubTab2",
                            Icon = "menu_dark.png",
                            ContentTemplate = new DataTemplate(typeof(TestPage))
                        },
                    }
                },
            }
        });
        //tier one
        Items.Add(new FlyoutItem
        {
            Title = "Some More",
            Route = "AccountItems2",
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {

                new ShellContent
                {
                    Icon = "menu_dark.png",
                    Title = "Shell item two",
                    ContentTemplate = new DataTemplate(typeof(TestPage))
                },
                new ShellContent
                {
                    Title = "Shell item three",
                    Icon = "menu_dark.png",
                    ContentTemplate = new DataTemplate(typeof(TestPage))
                },
                //tier three
                new Tab
                {
                    Icon = "menu_dark.png",
                    Title = "Tab1",
                    Items =
                    {
                        //tier four
                        new ShellContent
                        {
                            Title = "SubTab1",
                            Icon = "menu_dark.png",
                            ContentTemplate = new DataTemplate(typeof(TestPage))
                        },
                        new ShellContent
                        {
                            Title = "SubTab2",
                            Icon = "menu_dark.png",
                            ContentTemplate = new DataTemplate(typeof(TestPageTwo))
                        },
                    }
                }
                //new Tab()
            }
        });

        Items.Add(new ShellContent()
        {
            Content = new ContentPage
            {
                BackgroundColor = Color.FromRgb(100, 0, 0)
            }
        });


        var x = Items.ToList();
    }
}