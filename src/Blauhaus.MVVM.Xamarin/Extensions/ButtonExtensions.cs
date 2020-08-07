using System;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Xamarin.Converters;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class ButtonExtensions
    {
        public static Button BindExecutingCommand(this Button button, string executingCommandName) 
        {
            var commandPropertyName = $"{executingCommandName}.{nameof(IExecutingCommand)}";
            button.SetBinding(Button.CommandProperty, new Binding(commandPropertyName));
            return button;
        }

        public static Button BindExecutingCommand<TViewModel>(this Button button, Expression<Func<TViewModel, IExecutingCommand>> expression) 
        {
            var executingCommandName = expression.ToPropertyName();
            var commandPropertyName = $"{executingCommandName}.{nameof(IExecutingCommand)}";
            var isExecutingPropertyName = $"{executingCommandName}.{nameof(IExecutingCommand.IsExecuting)}";

            button.SetBinding(Button.CommandProperty, new Binding(commandPropertyName));
            button.SetBinding(VisualElement.IsEnabledProperty, new Binding(isExecutingPropertyName, BindingMode.OneWay, new BoolInverseConverter()));
            
            return button;
        }
    }
}