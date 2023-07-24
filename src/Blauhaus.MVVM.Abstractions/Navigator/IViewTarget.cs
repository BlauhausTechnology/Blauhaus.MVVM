using Blauhaus.Common.ValueObjects.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blauhaus.MVVM.Abstractions.Navigator;


public interface IViewTarget : IReadOnlyList<ViewIdentifier>
{
    string Path { get; }
    ViewIdentifier View { get; }
}


