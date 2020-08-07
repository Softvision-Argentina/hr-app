// <copyright file="DashboardServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Moq;
    using Xunit;

    public class DashboardServiceTest : BaseDomainTest
    {
        private readonly DashboardService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Dashboard>> mockRepositoryDashboard;
        private readonly Mock<ILog<DashboardService>> mockLogDashboardService;

        public DashboardServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryDashboard = new Mock<IRepository<Dashboard>>();
            this.mockLogDashboardService = new Mock<ILog<DashboardService>>();
            this.service = new DashboardService(
                this.mockMapper.Object,
                this.mockRepositoryDashboard.Object,
                this.MockUnitOfWork.Object,
                this.mockLogDashboardService.Object);
        }

        [Fact(DisplayName = "Verify that create DashboardService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDashboardService()
        {
            var contract = new CreateDashboardContract();
            var expectedDashboard = new CreatedDashboardContract();
            this.mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>())).Returns(new Dashboard());
            this.mockRepositoryDashboard.Setup(repoCom => repoCom.Create(It.IsAny<Dashboard>())).Returns(new Dashboard());
            this.mockMapper.Setup(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>())).Returns(expectedDashboard);

            var createdDashboard = this.service.Create(contract);

            Assert.NotNull(createdDashboard);
            Assert.Equal(expectedDashboard, createdDashboard);
            this.mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<CreateDashboardContract>()), Times.Once);
            this.mockRepositoryDashboard.Verify(mrt => mrt.Create(It.IsAny<Dashboard>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete DashboardService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDashboardService()
        {
            var dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            this.mockRepositoryDashboard.Setup(mrt => mrt.Query()).Returns(dashboards);

            this.service.Delete(1);

            this.mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDashboardNotFoundException()
        {
            var expectedErrorMEssage = $"The dashboard doesn't exist";

            var exception = Assert.Throws<Exception>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryDashboard.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDashboard.Verify(mrt => mrt.Delete(It.IsAny<Dashboard>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DashboardService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDashboardContract();
            this.mockMapper.Setup(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>())).Returns(new Dashboard());

            this.service.Update(contract);

            this.mockLogDashboardService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockMapper.Verify(mm => mm.Map<Dashboard>(It.IsAny<UpdateDashboardContract>()), Times.Once);
            this.mockRepositoryDashboard.Verify(mrt => mrt.Update(It.IsAny<Dashboard>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var dashboards = new List<Dashboard>() { new Dashboard() { Id = 1 } }.AsQueryable();
            var readedDashboardPCList = new List<ReadedDashboardContract> { new ReadedDashboardContract { Id = 1 } };
            this.mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(dashboards);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>())).Returns(readedDashboardPCList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDashboardContract>>(It.IsAny<List<Dashboard>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var dashboards = new List<Dashboard>() { new Dashboard() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDashboardPC = new ReadedDashboardContract { Id = 1, Name = "Name" };
            this.mockRepositoryDashboard.Setup(mrt => mrt.QueryEager()).Returns(dashboards);
            this.mockMapper.Setup(mm => mm.Map<ReadedDashboardContract>(It.IsAny<Dashboard>())).Returns(readedDashboardPC);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryDashboard.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDashboardContract>(It.IsAny<Dashboard>()), Times.Once);
        }
    }
}