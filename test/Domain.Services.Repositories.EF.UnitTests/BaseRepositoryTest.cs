//using Core.Persistance;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;

//namespace Domain.Services.Repositories.EF.UnitTests
//{
//    public class BaseRepositoryTest : IDisposable
//    {
//        protected readonly DataBaseContext DbContext;
//        protected readonly Mock<IUnitOfWork> MockUnitOfWork;

//        public BaseRepositoryTest()
//        {
//            var options =
//                   new DbContextOptionsBuilder<DataBaseContext>()
//                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
//                        .Options;
//            DbContext = new DataBaseContext(options);
//            MockUnitOfWork = new Mock<IUnitOfWork>();
//        }

//        public void Dispose()
//        {
//            DbContext.Dispose();
//        }
//    }
//}
