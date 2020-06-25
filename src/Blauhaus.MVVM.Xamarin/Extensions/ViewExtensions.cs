﻿using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.Xamarin.Converters;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Extensions
{
    public static class ViewExtensions
    {
        private const string IsExecuting = nameof(IExecuting.IsExecuting);

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

        
    }
}