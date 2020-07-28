using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Cv;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Interfaces.Repositories;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class CvServiceTest : BaseDomainTest
    {
        private readonly CvService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICvRepository> _mockRepositoryCv;

        public CvServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryCv = new Mock<ICvRepository>();
            _service = new CvService(
                _mockRepositoryCv.Object,
                _mockMapper.Object
            );
        }

        [Fact(DisplayName = "Verify that StoreCvAndCandidateCvId when data is valid")]
        public void GivenStoreCvAndCandidateCvId_WhenDataIsValid_CreateCvService()
        {
            _mockMapper.Setup(mm => mm.Map<Cv>(It.IsAny<CvContractAdd>())).Returns(new Cv());
            _mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<Candidate>())).Returns(new Candidate());
            _mockRepositoryCv.Setup(x => x.SaveAll(It.IsAny<Cv>()));
            var googleFile = "filename";
            _service.StoreCvAndCandidateCvId(new Candidate(), new CvContractAdd(), googleFile);

            _mockMapper.Verify(mm => mm.Map<Cv>(It.IsAny<CvContractAdd>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<Candidate>()), Times.Once);
            _mockRepositoryCv.Verify(x => x.SaveAll(It.IsAny<Cv>()), Times.Once);
        }
    }
}