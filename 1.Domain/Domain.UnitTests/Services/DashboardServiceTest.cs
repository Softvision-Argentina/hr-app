using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class DashboardServiceTest : BaseDomainTest
    {
        private readonly DashboardService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Dashboard>> _mockRepositoryDashboard;
        private readonly Mock<ILog<DashboardService>> _mockLogDashboardService;

        public DashboardServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryDashboard = new Mock<IRepository<Dashboard>>();            
            _mockLogDashboardService = new Mock<ILog<DashboardService>>();
            _service = new DashboardService(
                _mockMapper.Object,
                _mockRepositoryDashboard.Object,
                MockUnitOfWork.Object,
                _mockLogDashboardService.Object
            );
        }

        [Fact(DisplayName = "Verify that create DashboardService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDashboardService()
        {
            var contract = new CreateDashboardContract();
            var expectedDashboard = new CreatedDashboardContract();
            _mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>())).Returns(new Dashboard());
            _mockRepositoryDashboard.Setup(repoCom => repoCom.Create(It.IsAny<Dashboard>())).Returns(new Dashboard());
            _mockMapper.Setup(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>())).Returns(expectedDashboard);

            var createdDashboard = _service.Create(contract);

            Assert.NotNull(createdDashboard);
            Assert.Equal(expectedDashboard, createdDashboard);
            _mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));            
            _mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>()), Times.Once);
            _mockRepositoryDashboard.Verify(mrt => mrt.Create(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete DashboardService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDashboardService()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            _mockRepositoryDashboard.Setup(mrt => mrt.Query()).Returns(Dashboards);

            _service.Delete(1);

            _mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDashboardNotFoundException()
        {
            var expectedErrorMEssage = $"The dashboard doesn't exist";

            var exception = Assert.Throws<Exception>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DashboardService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDashboardContract();            
            _mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>())).Returns(new Dashboard());

            _service.Update(contract);

            _mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>()), Times.Once);
            _mockRepositoryDashboard.Verify(mrt => mrt.Update(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            var readedDashboardPCList = new List<ReadedDashboardContract> { new ReadedDashboardContract { Id = 1 } };
            _mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(Dashboards);
            _mockMapper.Setup(mm => mm.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>())).Returns(readedDashboardPCList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDashboardPC = new ReadedDashboardContract { Id = 1, Name = "Name" };
            _mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(Dashboards);
            _mockMapper.Setup(mm => mm.Map<ReadedDashboardContract>(It.IsAny<Dashboard>())).Returns(readedDashboardPC);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }
    }
}