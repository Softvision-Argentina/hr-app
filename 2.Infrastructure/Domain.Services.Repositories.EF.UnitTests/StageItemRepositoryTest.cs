using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class StageItemRepositoryTest : BaseRepositoryTest
    {
        private readonly StageItemRepository _repository;

        public StageItemRepositoryTest()
        {
            _repository = new StageItemRepository(DbContext, MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            StageItem expectedValue = null;

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns StageItem when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsStageItem()
        {
            var expectedValue = new StageItem();
            DbContext.StageItems.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }
    }
}
