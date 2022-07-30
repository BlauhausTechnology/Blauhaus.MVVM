using System;

namespace Blauhaus.MVVM.Abstractions.Navigation.NavigationService
{
    public class NavigationException : Exception
    {
        public NavigationException(string errorMessage) : base(errorMessage)
        {

        }
    }
}