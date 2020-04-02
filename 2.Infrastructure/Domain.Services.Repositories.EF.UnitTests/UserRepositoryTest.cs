using Core.Persistance;
using Domain.Model;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class UserRepositoryTest : BaseRepositoryTest
    {
        private readonly UserRepository repository;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;

        public UserRepositoryTest() : base()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            repository = new UserRepository(dbContext, mockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotUsersInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            User expectedValue = null;

            var actualValue = repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns user when Query there is data")]
        public void GivenUsersInRepositorysDbcontext_WhenQuery_ThenReturnsUser()
        {
            var expectedValue = new User();
            dbContext.Users.Add(expectedValue);
            dbContext.SaveChanges();

            var actualValue = repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns user when QueryEager there is data")]
        public void GivenUsersInRepositorysDbcontext_WhenQueryEager_ThenReturnsUser()
        {
            var expectedValue = new User()
            {
                UserDashboards = new List<UserDashboard>() { new UserDashboard() }
            };
            dbContext.Users.Add(expectedValue);
            dbContext.SaveChanges();

            var actualValue = repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.Count, actualValue.FirstOrDefault().UserDashboards.Count);
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault(), actualValue.FirstOrDefault().UserDashboards.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault().DashboardId, actualValue.FirstOrDefault().UserDashboards.FirstOrDefault().DashboardId);
        }
    }
}
