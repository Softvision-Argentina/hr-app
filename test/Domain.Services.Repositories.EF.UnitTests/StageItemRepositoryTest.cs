namespace Domain.Services.Repositories.EF.UnitTests
{
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class StageItemRepositoryTest : BaseRepositoryTest
    {
        private readonly StageItemRepository repository;

        public StageItemRepositoryTest()
        {
            this.repository = new StageItemRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            StageItem expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns StageItem when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsStageItem()
        {
            var expectedValue = new StageItem();
            this.DbContext.StageItems.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }
    }
}
