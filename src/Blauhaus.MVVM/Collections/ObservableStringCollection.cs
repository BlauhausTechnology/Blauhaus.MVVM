using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.MVVM.Collections
{
    public class ObservableStringCollection<T> : ObservableIdCollection<T, string> where T : class, IId<string>, IInitialize<string>
    {
        public ObservableStringCollection(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }
    }
}