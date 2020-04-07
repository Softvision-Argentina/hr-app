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
        private readonly UserRepository _repository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UserRepositoryTest() : base()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _repository = new UserRepository(DbContext, _mockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotUsersInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            User expectedValue = null;

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns user when Query there is data")]
        public void GivenUsersInRepositorysDbcontext_WhenQuery_ThenReturnsUser()
        {
            var expectedValue = new User();
            DbContext.Users.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.Query();

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
            DbContext.Users.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.Count, actualValue.FirstOrDefault().UserDashboards.Count);
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault(), actualValue.FirstOrDefault().UserDashboards.FirstOrDefault());
            Assert.Equal(expectedValue.UserDashboards.FirstOrDefault().DashboardId, actualValue.FirstOrDefault().UserDashboards.FirstOrDefault().DashboardId);
        }
    }
}
