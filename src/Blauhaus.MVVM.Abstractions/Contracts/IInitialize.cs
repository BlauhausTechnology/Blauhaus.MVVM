using System.Threading.Tasks;
using Blauhaus.Responses;

namespace Blauhaus.MVVM.Abstractions.Contracts
{
    public interface IInitialize<in T>
    {
        //NB this does not return Task because time-consuming setup in ViewModels should be handled in AppearCommand

        void Initialize(T initialValue);
    }
}