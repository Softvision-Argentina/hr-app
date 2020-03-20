using Domain.Services.Contracts.Seed;
using System;

namespace Domain.Services.Impl.UnitTests.Builders.Dummy
{
    internal class DummyContractBuilder : IBuilder<CreateDummyContract>
    {
        private string name;
        private string description;
        private string testValue;

        public DummyContractBuilder()
        {
            var id = Guid.NewGuid();
            name = $"Test {id}";
            description = $"this is a dymmy for {name}";
            testValue = "Test value";
        }

        public CreateDummyContract Build()
        {
            return new CreateDummyContract()
            {
                Name = name,
                Description = description,
                TestValue = testValue
            };
        }

        internal DummyContractBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        internal DummyContractBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        internal DummyContractBuilder WithTestValue(string testValue)
        {
            this.testValue = testValue;
            return this;
        }
    }
}
