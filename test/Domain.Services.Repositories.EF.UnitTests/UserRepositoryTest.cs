namespace Domain.Services.Repositories.EF.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class UserRepositoryTest : BaseRepositoryTest
    {
        private readonly UserRepository repository;

        public UserRepositoryTest() : base()
        {
            this.repository = new UserRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotUsersInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            User expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns user when Query there is data")]
        public void GivenUsersInRepositorysDbcontext_WhenQuery_ThenReturnsUser()
        {
            var expectedValue = new User();
            this.DbContext.Users.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns user when QueryEager there is data")]
        public void GivenUsersInRepositorysDbcontext_WhenQueryEager_ThenReturnsUser()
        {
            var expectedValue = new User()
            {
                UserDashboards = new List<UserDashboard>() { new UserDashboard() },
            };
            this.DbContext.Users.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.Count, actualValue.FirstOrDefault().UserDashboards.Count);
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault(), actualValue.FirstOrDefault().UserDashboards.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault().DashboardId, actualValue.FirstOrDefault().UserDashboards.FirstOrDefault().DashboardId);
        }
    }
}
