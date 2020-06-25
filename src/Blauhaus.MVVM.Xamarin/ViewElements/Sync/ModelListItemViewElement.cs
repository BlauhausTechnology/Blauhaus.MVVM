using System;
using Blauhaus.Domain.Common.Entities;
using Blauhaus.MVVM.Abstractions.Commands;

namespace Blauhaus.MVVM.Xamarin.ViewElements.Sync
{
    public class ModelListItemViewElement : IClientEntity
    { 

        public Guid Id { get; set; }
        public long ModifiedAtTicks { get; set; }
        public EntityState EntityState { get; } = EntityState.Active;

    }
}