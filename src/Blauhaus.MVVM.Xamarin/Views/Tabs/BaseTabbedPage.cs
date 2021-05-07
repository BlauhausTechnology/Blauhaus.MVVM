using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.MVVM.Abstractions.Navigation;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Abstractions.ViewModels.Tabs;
using Blauhaus.MVVM.Abstractions.Views;
using Blauhaus.MVVM.Xamarin.Converters;
using Blauhaus.MVVM.Xamarin.Views.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Blauhaus.MVVM.Xamarin.Views.Tabs
{ 
    public abstract class BaseTabbedPage<TViewModel> : TabbedPage, IView 
        where TViewModel : IViewModel
    {
        protected readonly TViewModel ViewModel;

        protected BaseTabbedPage(
            TViewModel viewModel,
            INavigationService navigationService,
            INavigationLookup navigationLookup,
            IServiceLocator serviceLocator,
            IAnalyticsService analyticsService)
        {
            ViewModel = viewModel;
            BindingContext = ViewModel;
            
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            if (viewModel is ITabbedViewModel _)
            {
                SetBinding(TabDefinitionsProperty, new Binding(nameof(ITabbedViewModel.TabDefinitions), BindingMode.OneWay, 
                    new ActionConverter<IReadOnlyList<TabDefinition>>(async tabs =>
                {
                    if (tabs == null)
                    {
                        return;
                    }
                    
                    foreach (var tabDefinition in tabs)
                    {
                        if (TabExists(tabDefinition))
                        {
                            analyticsService.Debug("Tab already exists");
                            return;
                        }

                        var viewType = navigationLookup.GetViewType(tabDefinition.ViewModelType);
                        if (viewType == null)
                        {
                            throw new NavigationException($"No view is registered for {tabDefinition.ViewModelType.Name}");
                        }

                        var view = serviceLocator.Resolve(viewType);
                        if (view == null)
                        {
                            throw new NavigationException($"No View of type {viewType.Name} has been registered with the Ioc container");
                        }
            
                        if (!(view is Page page))
                        {
                            throw new NavigationException($"View type {viewType.Name} is not a {nameof(Page)}");
                        }
                        
                        switch (page.BindingContext)
                        {
                            case IAsyncInitializable initializable:
                                await initializable.InitializeAsync();
                                break;
                            case IInitializingViewModel initializableViewModel:
                                initializableViewModel.InitializeCommand.Execute();
                                break;
                        }

                        if (tabDefinition.NavigationStackName != null)
                        {
                            analyticsService.Debug($"Added new navigating tab for {tabDefinition.ViewModelType.Name}");
                            Children.Add(new NavigationView(navigationService, page, tabDefinition.NavigationStackName) { Title = tabDefinition.Title});
                        }
                        else
                        {
                            analyticsService.Debug($"Added new tab for {tabDefinition.ViewModelType.Name}");
                            page.Title = tabDefinition.Title;
                            Children.Add(page);
                        }
                    }
                })));
            }
            
        }
        
        protected override void OnAppearing()
        {
            if (ViewModel is IAppearingViewModel appearViewModel)
            {
                appearViewModel.AppearCommand.Execute();
            }
        }

        protected override void OnDisappearing()
        {
            if (ViewModel is IDisappearingViewModel disappearViewModel)
            {
                disappearViewModel.DisappearCommand.Execute();
            }
        }
        
        
        public static readonly BindableProperty TabDefinitionsProperty = BindableProperty.Create(
            propertyName: nameof(TabDefinitions),
            returnType: typeof(IReadOnlyList<TabDefinition>),
            declaringType: typeof(BaseTabbedPage<TViewModel>),
            defaultValue: new TabDefinition[0]);

        public object TabDefinitions
        {
            get => GetValue(TabDefinitionsProperty);
            set => SetValue(TabDefinitionsProperty, value);
        }
        
        private bool TabExists(TabDefinition tabDefinition)
        {
            foreach (var child in Children)
            {
                if (PageBelongsToViewModel(child, tabDefinition.ViewModelType))
                    return true;
            }
            return false;
        }
         
        private static bool PageBelongsToViewModel(Page page, Type viewModelType)
        {
            Page rootPage;
            if (page is NavigationPage nav)
            {
                rootPage = nav.RootPage;
            }
            else
            {
                rootPage = page;
            }

            return rootPage.BindingContext.GetType().Name == viewModelType.Name;
        }


    }

}