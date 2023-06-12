using Blauhaus.Common.Abstractions;
using System.Linq.Expressions;
using System.Windows.Input;
using System;
using Blauhaus.Common.Utils.Extensions;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions;

public static class ButtonExtensions
{
    
    public static Button BindIsVisible<TViewModel>(this Button button, Expression<Func<TViewModel, IIsVisible>> viewModelCommand)  
    {
        string propertyName = viewModelCommand.ToPropertyName();
        button.SetBinding(VisualElement.IsVisibleProperty, new Binding($"{propertyName}.{nameof(IIsVisible.IsVisible)}"));
        return button;
    }

}