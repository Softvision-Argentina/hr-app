using Core.Persistance;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Dummy
{
    [Collection("Service Test Collection")]
    public class BaseDomainTest
    {
        protected Mock<IUnitOfWork> MockUnitOfWork { get; set; }

        public BaseDomainTest()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            MockUnitOfWork.Setup(_ => _.Complete()).Returns(1);
        }
    }
}
