using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Impl.Services;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class DashboardServiceTest : BaseDomainTest
    {
        private readonly DashboardService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Dashboard>> mockRepositoryDashboard;
        private readonly Mock<ILog<DashboardService>> mockLogDashboardService;

        public DashboardServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryDashboard = new Mock<IRepository<Dashboard>>();            
            mockLogDashboardService = new Mock<ILog<DashboardService>>();
            service = new DashboardService(
                mockMapper.Object,
                mockRepositoryDashboard.Object,
                MockUnitOfWork.Object,
                mockLogDashboardService.Object
            );
        }

        [Fact(DisplayName = "Verify that create DashboardService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDashboardService()
        {
            var contract = new CreateDashboardContract();
            var expectedDashboard = new CreatedDashboardContract();
            mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>())).Returns(new Dashboard());
            mockRepositoryDashboard.Setup(repoCom => repoCom.Create(It.IsAny<Dashboard>())).Returns(new Dashboard());
            mockMapper.Setup(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>())).Returns(expectedDashboard);

            var createdDashboard = service.Create(contract);

            Assert.NotNull(createdDashboard);
            Assert.Equal(expectedDashboard, createdDashboard);
            mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));            
            mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>()), Times.Once);
            mockRepositoryDashboard.Verify(mrt => mrt.Create(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete DashboardService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDashboardService()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            mockRepositoryDashboard.Setup(mrt => mrt.Query()).Returns(Dashboards);

            service.Delete(1);

            mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDashboardNotFoundException()
        {
            var expectedErrorMEssage = $"The dashboard doesn't exist";

            var exception = Assert.Throws<Exception>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DashboardService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDashboardContract();            
            mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>())).Returns(new Dashboard());

            service.Update(contract);

            mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>()), Times.Once);
            mockRepositoryDashboard.Verify(mrt => mrt.Update(It.IsAny<Dashboard>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            var readedDashboardPCList = new List<ReadedDashboardContract> { new ReadedDashboardContract { Id = 1 } };
            mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(Dashboards);
            mockMapper.Setup(mm => mm.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>())).Returns(readedDashboardPCList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Dashboards = new List<Dashboard>() { new Dashboard() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDashboardPC = new ReadedDashboardContract { Id = 1, Name = "Name" };
            mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(Dashboards);
            mockMapper.Setup(mm => mm.Map<ReadedDashboardContract>(It.IsAny<Dashboard>())).Returns(readedDashboardPC);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }
    }
}