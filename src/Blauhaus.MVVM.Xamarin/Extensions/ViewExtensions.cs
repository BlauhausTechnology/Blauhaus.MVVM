﻿using Blauhaus.Common.Abstractions;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Xamarin.Converters;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class ViewExtensions
    {
        private const string IsExecuting = nameof(IIsExecuting.IsExecuting);

        public static TVisualElement BindIsVisibleToIsExecuting<TVisualElement>(this TVisualElement visualElement, string propertyName) 
            where TVisualElement : View
        {
            visualElement.SetBinding(VisualElement.IsVisibleProperty, new Binding($"{propertyName}.{IsExecuting}"));

            return visualElement;
        }

        public static TVisualElement BindIsVisibleToIsNotExecuting<TVisualElement>(this TVisualElement visualElement, string propertyName) 
            where TVisualElement : View
        {
            visualElement.SetBinding(VisualElement.IsVisibleProperty, 
                new Binding($"{propertyName}.{IsExecuting}", BindingMode.Default, new BoolInverseConverter()));

            return visualElement;
        }

        

        public static TVisualElement BindIsVisible<TVisualElement>(this TVisualElement visualElement, string propertyName) 
            where TVisualElement : View
        {
            visualElement.SetBinding(VisualElement.IsVisibleProperty, new Binding($"{propertyName}.{nameof(IIsVisible.IsVisible)}"));
            return visualElement;
        }

        
    }
}