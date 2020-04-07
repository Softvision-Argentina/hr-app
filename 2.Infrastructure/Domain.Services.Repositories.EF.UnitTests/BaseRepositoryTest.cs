using Microsoft.EntityFrameworkCore;
using System;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class BaseRepositoryTest : IDisposable
    {
        protected readonly DataBaseContext DbContext;

        public BaseRepositoryTest()
        {
            var options =
                   new DbContextOptionsBuilder<DataBaseContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;
            DbContext = new DataBaseContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
