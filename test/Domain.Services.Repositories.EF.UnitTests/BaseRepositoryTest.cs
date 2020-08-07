namespace Domain.Services.Repositories.EF.UnitTests
{
    using System;
    using Core.Persistance;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;

    public class BaseRepositoryTest : IDisposable
    {
        protected readonly DataBaseContext DbContext;
        protected readonly Mock<IUnitOfWork> MockUnitOfWork;
        protected readonly Mock<IHttpContextAccessor> httpContext;

        public BaseRepositoryTest()
        {
            var options =
                   new DbContextOptionsBuilder<DataBaseContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

            this.httpContext = new Mock<IHttpContextAccessor>();
            this.DbContext = new DataBaseContext(options, this.httpContext.Object);
            this.MockUnitOfWork = new Mock<IUnitOfWork>();
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }
}
