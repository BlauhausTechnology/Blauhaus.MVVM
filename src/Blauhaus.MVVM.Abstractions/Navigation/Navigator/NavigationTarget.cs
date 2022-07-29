using System.Text;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.Navigation.Navigator
{
    public class NavigationTarget : BaseValueObject<NavigationTarget>
    {
        private readonly string _pathString;

        public NavigationTarget(params ViewIdentifier[] path)
        {
            Path = path;

            
            var s = new StringBuilder();
            foreach (var viewIdentifier in Path)
            {
                s.Append(viewIdentifier);
            }

            _pathString = s.ToString();
        }

        public ViewIdentifier[] Path { get; }

        public override string ToString()
        {
            return _pathString;
        }

        protected override int GetHashCodeCore()
        {
            return _pathString.GetHashCode();
        }

        protected override bool EqualsCore(NavigationTarget other)
        {
            return _pathString == other.ToString();
        }
    }
}