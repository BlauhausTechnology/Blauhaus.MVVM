using System;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestDto
    {
        public TestDto(Guid id = default, string name = default, long? modifiedAtTicks = default)
        {
            Id = id == default ? Guid.NewGuid() : id;
            Name = name == default ? Guid.NewGuid().ToString() : name;
            ModifiedAtTicks = modifiedAtTicks ?? DateTime.UtcNow.Ticks;
            EntityState = EntityState.Active;
        }

        public string Name { get; }
        public Guid Id { get; }
        public EntityState EntityState { get; }
        public long ModifiedAtTicks { get; set; }
    }
}