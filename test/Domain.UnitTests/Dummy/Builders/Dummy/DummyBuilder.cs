// <copyright file="DummyBuilder.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy
{
    using System;
    using Dummy = Domain.Model.Seed.Dummy;

    internal class DummyBuilder : IBuilder<Dummy>
    {
        private Guid id;
        private string name;
        private string description;
        private string testValue;

        public DummyBuilder()
        {
            this.id = Guid.NewGuid();
            this.name = $"Test {this.id}";
            this.description = $"this is a dymmy for {this.name}";
            this.testValue = "Test value";
        }

        public Dummy Build()
        {
            return new Dummy()
            {
                Id = this.id,
                Name = this.name,
                Description = this.description,
                TestValue = this.testValue,
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
