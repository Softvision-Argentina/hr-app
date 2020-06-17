using ApiServer.Contracts.Seed;
using System;

namespace ApiServer.FunctionalTests.Dummy.Seed.Builder
{
    public class CreateDummyViewModelBuilder
    {
        private string name;
        private string description;
        private string testValue;

        public CreateDummyViewModelBuilder()
        {
            name = $"test {Guid.NewGuid()}";
            description = $"Some Description for {name}";
            testValue = $"test value for {name}";
        }

        public CreateDummyViewModel Build()
        {
            return new CreateDummyViewModel
            {
                Name = name,
                Description = description,
                TestValue = testValue,
            };
        }

        internal CreateDummyViewModelBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        internal CreateDummyViewModelBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        internal CreateDummyViewModelBuilder WithTestValue(string testValue)
        {
            this.testValue = testValue;
            return this;
        }
    }
}
