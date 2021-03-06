﻿namespace Domain.Services.Repositories.EF.UnitTests
{
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class TaskItemRepositoryTest : BaseRepositoryTest
    {
        private readonly TaskItemRepository repository;

        public TaskItemRepositoryTest()
        {
            this.repository = new TaskItemRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            TaskItem expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns TaskItem when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsTaskItem()
        {
            var expectedValue = new TaskItem();
            this.DbContext.TaskItems.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }
    }
}
