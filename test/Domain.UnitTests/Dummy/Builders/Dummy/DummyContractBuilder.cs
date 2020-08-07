// <copyright file="DummyContractBuilder.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy
{
    using System;
    using Domain.Services.Contracts.Seed;

    internal class DummyContractBuilder : IBuilder<CreateDummyContract>
    {
        private string name;
        private string description;
        private string testValue;

        public DummyContractBuilder()
        {
            var id = Guid.NewGuid();
            this.name = $"Test {id}";
            this.description = $"this is a dymmy for {this.name}";
            this.testValue = "Test value";
        }

        public CreateDummyContract Build()
        {
            return new CreateDummyContract()
            {
                Name = this.name,
                Description = this.description,
                TestValue = this.testValue,
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
