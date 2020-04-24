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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class DashboardControllerTest
    {
        private DashboardController controller;
        private Mock<ILog<DashboardController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IDashboardService> mockService;

        public DashboardControllerTest()
        {
            mockLog = new Mock<ILog<DashboardController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IDashboardService>();
            controller = new DashboardController(mockService.Object, mockLog.Object, mockMapper.Object);
        }


        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDashboards()
        {
            var expectedValue = new List<ReadedDashboardViewModel>();
            expectedValue.Add(new ReadedDashboardViewModel
            {
                Id = 0,
                Name = "TestDashboard",
                UserDashboards = new Collection<ReadedUserDashboardViewModel>()
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDashboardContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedDashboardViewModel>>(It.IsAny<IEnumerable<ReadedDashboardContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDashboardViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedDashboardViewModel>>(It.IsAny<IEnumerable<ReadedDashboardContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DashboardById_WhenDataIsValid()
        {
            var dashboardId = 0;
            var expectedValue = new ReadedDashboardViewModel
            {
                Id = 0,
                Name = "TestDashboard",
                UserDashboards = new Collection<ReadedUserDashboardViewModel>()
            };

            mockService.Setup(_ => _.Read(It.IsAny<int>())).Returns(new ReadedDashboardContract());
            mockMapper.Setup(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>())).Returns(expectedValue);

            var result = controller.Get(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDashboardViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DashboardById_WhenDataIsInvalid()
        {
            var dashboardId = 0;
            var expectedValue = dashboardId;

            var result = controller.Get(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<int>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDashboardViewModel>(It.IsAny<ReadedDashboardContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Dashboard()
        {
            var dashboardVM = new CreateDashboardViewModel
            {
                Name = "TestDashboard",
                UserDashboards = new Collection<CreateUserDashboardViewModel>()
            };

            var expectedValue = new CreatedDashboardViewModel
            {
                Id = 0
            };

            mockMapper.Setup(_ => _.Map<CreateDashboardContract>(It.IsAny<CreateDashboardViewModel>())).Returns(new CreateDashboardContract());
            mockMapper.Setup(_ => _.Map<CreatedDashboardViewModel>(It.IsAny<CreatedDashboardContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateDashboardContract>())).Returns(new CreatedDashboardContract());

            var result = controller.Post(dashboardVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDashboardViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateDashboardContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedDashboardViewModel>(It.IsAny<CreatedDashboardContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Dashboard()
        {
            var dashboardId = 0;
            var dashboardToUpdate = new UpdateDashboardViewModel();
            var expectedValue = new { id = dashboardId };

            mockMapper.Setup(_ => _.Map<UpdateDashboardContract>(It.IsAny<UpdateDashboardViewModel>())).Returns(new UpdateDashboardContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateDashboardContract>()));

            var result = controller.Put(dashboardId, dashboardToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedValue.ToQueryString(), (result as AcceptedResult).Value.ToQueryString());
            mockService.Verify(_ => _.Update(It.IsAny<UpdateDashboardContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateDashboardContract>(It.IsAny<UpdateDashboardViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Dashboard()
        {
            var dashboardId = 0;

            mockService.Setup(_ => _.Delete(It.IsAny<int>()));

            var result = controller.Delete(dashboardId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Ping' returns OkObjectResult")]
        public void Should_Ping()
        {
            var result = controller.Ping();
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }
    }
}