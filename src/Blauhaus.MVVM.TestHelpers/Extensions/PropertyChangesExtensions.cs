using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Blauhaus.TestHelpers.PropertiesChanged.NotifyPropertyChanged;

namespace Blauhaus.MVVM.TestHelpers.Extensions
{
    public static class PropertyChangesExtensions
    {
        public static bool IsSequenceEqual<TNotifyPropertyChanged, T>(this PropertyChanges<TNotifyPropertyChanged, T> propertyChanges, params T[] expectedSequence) where TNotifyPropertyChanged : INotifyPropertyChanged
        {
             return expectedSequence.SequenceEqual(propertyChanges);
        }

    }
}