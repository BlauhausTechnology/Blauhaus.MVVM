using System.Threading.Tasks;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IInitialize<in T>
    {
        void Initialize(T initialValue);
    }
}