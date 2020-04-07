using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Postulant;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class PostulantServiceTest : BaseDomainTest
    {
        private readonly PostulantService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Postulant>> _mockRepositoryPostulant;                

        public PostulantServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryPostulant = new Mock<IRepository<Postulant>>();
            _service = new PostulantService(
                _mockMapper.Object,
                _mockRepositoryPostulant.Object
            );
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulantList = new List<ReadedPostulantContract> { new ReadedPostulantContract { Id = 1 } };
            _mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(Postulants);
            _mockMapper.Setup(mm => mm.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>())).Returns(readedPostulantList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulant = new ReadedPostulantContract { Id = 1 };
            _mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(Postulants);
            _mockMapper.Setup(mm => mm.Map<ReadedPostulantContract>(It.IsAny<Postulant>())).Returns(readedPostulant);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedPostulant, actualResult);
            _mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedPostulantContract>(It.IsAny<Postulant>()), Times.Once);
        }
    }
}