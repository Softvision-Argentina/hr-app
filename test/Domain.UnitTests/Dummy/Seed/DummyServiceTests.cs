// <copyright file="DummyServiceTests.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Seed
{
    using System;
    using System.Linq;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy;
    using Xunit;

    public class DummyServiceTests : BaseDummyTest
    {
        public DummyServiceTests(AutomapperFixture automapperFixture) : base(automapperFixture)
        {
        }

        [Fact(DisplayName = "Persistance is called")]
        [Trait("Category", "Dummy")]
        public void When_CreateIsCall_ShouldPersistInfo()
        {
            var result = this.service.Create(new DummyContractBuilder().WithName("Test").Build());

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            this.AssertPersistanceIsCalledForCreateDummy();
        }

        [Fact(DisplayName = "Can get a dummy")]
        [Trait("Category", "Dummy")]
        public void When_GetIsCall_AndTheIdExists_ShouldReturnTheEntity()
        {
            var existingEnitity = this.InMemoryDummies.First();
            var exisitingId = existingEnitity.Id;

            var result = this.service.Read(exisitingId);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(existingEnitity.Id, result.Id);
        }
    }
}
