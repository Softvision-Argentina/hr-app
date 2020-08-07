// <copyright file="DashboardControllerTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.UnitTests.Controllers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using ApiServer.Contracts.Dashboard;
    using ApiServer.Contracts.UserDashboard;
    using ApiServer.Controllers;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class DashboardControllerTest
    {
        private readonly DashboardController controller;
        private readonly Mock<ILog<DashboardController>> mockLog;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IDashboardService> mockService;

        public DashboardControllerTest()
        {
            this.mockLog = new Mock<ILog<DashboardController>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockService = new Mock<IDashboardService>();
            this.controller = new DashboardController(this.mockService.Object, this.mockLog.Object, this.mockMapper.Object);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDashboards()
        {
            var expectedValue = new List<ReadedDashboardViewModel>();
            expectedValue.Add(new ReadedDashboardViewModel
            {
                Id = 0,
                Name = "TestDashboard",
                UserDashboards = new Collection<ReadedUserDashboardViewModel>(),
            });

            this.mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDashboardContract() });
            this.mockMapper.Setup(_ => _.Map<List<ReadedDashboardViewModel>>(It.IsAny<IEnumerable<ReadedDashboardContract>>())).Returns(expectedValue);

            var result = this.controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDashboardViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.List(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDashboardViewModel>>(It.IsAny<IEnumerable<ReadedDashboardContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DashboardById_WhenDataIsValid()
        {
            var dashboardId = 0;
            var expectedValue = new ReadedDashboardViewModel
            {
                Id = 0,
                Name = "TestDashboard",
                UserDashboards = new Collection<ReadedUserDashboardViewModel>(),
            };

            this.mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedDashboardContract());
            this.mockMapper.Setup(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>())).Returns(expectedValue);

            var result = this.controller.Get(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDashboardViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DashboardById_WhenDataIsInvalid()
        {
            var dashboardId = 0;
            var expectedValue = dashboardId;

            var result = this.controller.Get(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            this.mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Dashboard()
        {
            var dashboardVM = new CreateDashboardViewModel
            {
                Name = "TestDashboard",
                UserDashboards = new Collection<CreateUserDashboardViewModel>(),
            };

            var expectedValue = new CreatedDashboardViewModel
            {
                Id = 0,
            };

            this.mockMapper.Setup(_ => _.Map<CreateDashboardContract>(It.IsAny<CreateDashboardViewModel>())).Returns(new CreateDashboardContract());
            this.mockMapper.Setup(_ => _.Map<CreatedDashboardViewModel>(It.IsAny<CreatedDashboardContract>())).Returns(expectedValue);
            this.mockService.Setup(_ => _.Create(It.IsAny<CreateDashboardContract>())).Returns(new CreatedDashboardContract());

            var result = this.controller.Post(dashboardVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDashboardViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            this.mockService.Verify(_ => _.Create(It.IsAny<CreateDashboardContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedDashboardViewModel>(It.IsAny<CreatedDashboardContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Dashboard()
        {
            var dashboardId = 0;
            var dashboardToUpdate = new UpdateDashboardViewModel();
            var expectedValue = new { id = dashboardId };

            this.mockMapper.Setup(_ => _.Map<UpdateDashboardContract>(It.IsAny<UpdateDashboardViewModel>())).Returns(new UpdateDashboardContract());
            this.mockService.Setup(_ => _.Update(It.IsAny<UpdateDashboardContract>()));

            var result = this.controller.Put(dashboardId, dashboardToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            this.mockService.Verify(_ => _.Update(It.IsAny<UpdateDashboardContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<UpdateDashboardContract>(It.IsAny<UpdateDashboardViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Dashboard()
        {
            var dashboardId = 0;

            this.mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = this.controller.Delete(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            this.mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Ping' returns OkObjectResult")]
        public void Should_Ping()
        {
            var result = this.controller.Ping();
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}