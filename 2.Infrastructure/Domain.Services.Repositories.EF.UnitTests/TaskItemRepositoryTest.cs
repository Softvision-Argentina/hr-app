using Domain.Model;
using System.Linq;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class TaskItemRepositoryTest : BaseRepositoryTest
    {
        private readonly TaskItemRepository _repository;

        public TaskItemRepositoryTest()
        {
            _repository = new TaskItemRepository(DbContext, MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            TaskItem expectedValue = null;

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns TaskItem when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsTaskItem()
        {
            var expectedValue = new TaskItem();
            DbContext.TaskItems.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }
    }
}
