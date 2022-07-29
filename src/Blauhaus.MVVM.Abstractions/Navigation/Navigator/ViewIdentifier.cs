using System.Runtime.CompilerServices;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.ValueObjects.Base;

namespace Blauhaus.MVVM.Abstractions.Navigation.Navigator
{
    public class ViewIdentifier : BaseValueObject<ViewIdentifier>, IHasName
    {
        public ViewIdentifier(string name, string? identifier)
        {
            Name = name;
            Identifier = identifier;
        }
        public string Name { get; }
        public string? Identifier { get; }

        public static ViewIdentifier Create(string? identifier = null, [CallerMemberName] string name = "") => new(name, identifier);

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Identifier))
            {
                return $"/{Name.ToLower()}";
            }

            return $"/{Name.ToLower()}?Id={Identifier}";
        }

        protected override int GetHashCodeCore()
        {
            if (Identifier == null) return Name.GetHashCode();
            return Name.GetHashCode() ^ Identifier.GetHashCode();
        }

        protected override bool EqualsCore(ViewIdentifier other)
        {
            if (Name != other.Name) return false;

            if (Identifier == null && other.Identifier != null) return false;

            return Identifier == other.Identifier;
        }
    }
}