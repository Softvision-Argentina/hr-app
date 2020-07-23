using Core.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class BaseRepositoryTest : IDisposable
    {
        protected readonly DataBaseContext DbContext;
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;
        protected readonly Mock<IHttpContextAccessor> _httpContext;

        public BaseRepositoryTest()
        {
            var options =
                   new DbContextOptionsBuilder<DataBaseContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

            _httpContext = new Mock<IHttpContextAccessor>();
            DbContext = new DataBaseContext(options, _httpContext.Object);
            MockUnitOfWork = new Mock<IUnitOfWork>();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
