using System;

namespace Blauhaus.MVVM.Abstractions.Navigation
{
    public class NavigationException : Exception
    {
        public NavigationException(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}