using Microsoft.EntityFrameworkCore;
using System;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class BaseRepositoryTest : IDisposable
    {
        protected readonly DataBaseContext dbContext;

        public BaseRepositoryTest()
        {
            var options =
                   new DbContextOptionsBuilder<DataBaseContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;
            dbContext = new DataBaseContext(options);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
