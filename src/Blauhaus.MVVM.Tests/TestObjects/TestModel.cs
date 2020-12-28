using System;
using System.Collections.Generic;
using System.Globalization;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    public class TestModel : IClientEntity
    {
        public TestModel(Guid id = default, string name = default, long? modifiedAtTicks = default)
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

        public override string ToString()
        {
            return new DateTime(ModifiedAtTicks).ToString(CultureInfo.InvariantCulture);
        }

        public static List<TestModel> Generate(int number, DateTime start)
        {
            var list = new List<TestModel>();
            for (var i = 0; i < number; i++)
            {
                list.Add(new TestModel(Guid.NewGuid(), Guid.NewGuid().ToString(), start.AddMinutes(-i).Ticks));
            }

            return list;
        }
    }
}