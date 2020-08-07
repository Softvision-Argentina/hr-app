namespace Domain.Services.Repositories.EF.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class TaskRepositoryTest : BaseRepositoryTest
    {
        private readonly TaskRepository repository;

        public TaskRepositoryTest()
        {
            this.repository = new TaskRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            Task expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsTask()
        {
            var expectedValue = new Task();
            this.DbContext.Tasks.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQueryEager_ThenReturnsTasks()
        {
            var expectedValue = new Task()
            {
                TaskItems = new List<TaskItem>() { new TaskItem() },
                User = new User(),
            };
            this.DbContext.Tasks.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.TaskItems.Count, actualValue.FirstOrDefault().TaskItems.Count);
            Assert.Equal(expectedValue.TaskItems.FirstOrDefault(), actualValue.FirstOrDefault().TaskItems.FirstOrDefault());
            Assert.Equal(expectedValue.User, actualValue.FirstOrDefault().User);
        }

        [Fact(DisplayName = "Verify that repository returns task when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenUpdate_ThenReturnsTask()
        {
            var newTask = new Task()
            {
                Title = "title1",
                CreatedDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
            };
            var updateTask = new Task()
            {
                Title = "title2",
                IsApprove = true,
                IsNew = true,
                CreatedDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today,
                UserId = 1,
                User = new User() { Id = 1 },
                TaskItems = new List<TaskItem>() { new TaskItem() { Id = 1 } },
            };
            this.DbContext.Tasks.Add(newTask);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Update(updateTask);

            Assert.NotNull(actualValue);
            Assert.Equal(updateTask, actualValue);
            Assert.Equal(1, this.DbContext.Tasks.Count());
        }
    }
}
