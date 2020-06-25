using Blauhaus.Domain.Common.Entities;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public interface IModelViewElementUpdater<TModel, TViewElment>
        where TModel : IClientEntity
        where TViewElment : ModelListItemViewElement
    {
        TViewElment Update(TModel model, TViewElment element);
    }
}