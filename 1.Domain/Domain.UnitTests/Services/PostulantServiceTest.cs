using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Postulant;
using Domain.Services.Impl.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class PostulantServiceTest : BaseDomainTest
    {
        private readonly PostulantService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Postulant>> mockRepositoryPostulant;                

        public PostulantServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryPostulant = new Mock<IRepository<Postulant>>();
            service = new PostulantService(
                mockMapper.Object,
                mockRepositoryPostulant.Object
            );
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulantList = new List<ReadedPostulantContract> { new ReadedPostulantContract { Id = 1 } };
            mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(Postulants);
            mockMapper.Setup(mm => mm.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>())).Returns(readedPostulantList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulant = new ReadedPostulantContract { Id = 1 };
            mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(Postulants);
            mockMapper.Setup(mm => mm.Map<ReadedPostulantContract>(It.IsAny<Postulant>())).Returns(readedPostulant);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedPostulant, actualResult);
            mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedPostulantContract>(It.IsAny<Postulant>()), Times.Once);
        }
    }
}