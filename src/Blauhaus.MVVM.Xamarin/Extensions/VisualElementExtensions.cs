using System;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class VisualElementExtensions
    {
        public static TControl BindTapToParent<TViewModel, TControl>(this TControl control, Expression<Func<TViewModel, IExecutingCommand>> viewModelCommand) 
            where TViewModel : IViewModel
            where TControl : View
            
        {
            var commandName = viewModelCommand.ToPropertyName() + "." + nameof(IExecutingCommand);

            var tapGestureRecognizer = new TapGestureRecognizer();
            var bindingSource = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext, typeof(TViewModel));
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(commandName, source:bindingSource));
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding("."));
            control.GestureRecognizers.Add(tapGestureRecognizer);

            return control;
        }
         
    }
}