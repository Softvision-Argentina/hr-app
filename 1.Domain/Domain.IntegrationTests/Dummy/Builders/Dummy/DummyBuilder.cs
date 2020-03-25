using Domain.Services.Contracts.Seed;
using System;

namespace Domain.Services.Impl.IntegrationTests.Dummy.Builders.Dummy
{
    using Dummy = Model.Seed.Dummy;

    internal class DummyBuilder : IBuilder<Dummy>
    {
        private Guid id;
        private string name;
        private string description;
        private string testValue;

        public DummyBuilder()
        {
            id = Guid.NewGuid();
            name = $"Test {id}";
            description = $"this is a dymmy for {name}";
            testValue = "Test value";
        }

        public Dummy Build()
        {
            return new Dummy()
            {
                Id = id,
                Name = name,
                Description = description,
                TestValue = testValue
            };
        }

        internal DummyBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        internal DummyBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        internal DummyBuilder WithTestValue(string testValue)
        {
            this.testValue = testValue;
            return this;
        }
    }
}
