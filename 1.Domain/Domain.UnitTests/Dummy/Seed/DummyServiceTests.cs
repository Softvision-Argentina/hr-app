using System;
using System.Linq;
using Xunit;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy;

namespace Domain.Services.Impl.UnitTests.Seed
{
    public class DummyServiceTests : BaseDummyTest
    {
        public DummyServiceTests(AutomapperFixture automapperFixture) : base(automapperFixture)
        {
        }

        [Fact(DisplayName = "Persistance is called")]
        [Trait("Category", "Dummy")]
        public void When_CreateIsCall_ShouldPersistInfo()
        {
            var result = Service.Create(new DummyContractBuilder().WithName("Test").Build());

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            AssertPersistanceIsCalledForCreateDummy();
        }

        [Fact(DisplayName = "Can get a dummy")]
        [Trait("Category", "Dummy")]
        public void When_GetIsCall_AndTheIdExists_ShouldReturnTheEntity()
        {
            var existingEnitity = InMemoryDummies.First();
            var exisitingId = existingEnitity.Id;

            var result = Service.Read(exisitingId);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(existingEnitity.Id, result.Id);
        }
    }
}
