using ApiServer.Contracts.Seed;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.Seed;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiServer.UnitTests.Controllers
{
    public class DummiesControllerTest
    {
        private DummiesController controller;
        private Mock<ILog<DummiesController>> mockLog;
        private Mock<IMapper> mockMapper;
        private Mock<IDummyService> mockService;

        public DummiesControllerTest()
        {
            mockLog = new Mock<ILog<DummiesController>>();
            mockMapper = new Mock<IMapper>();
            mockService = new Mock<IDummyService>();
            controller = new DummiesController(mockService.Object, mockLog.Object, mockMapper.Object);
        }


        [Fact(DisplayName = "Verify that method 'Get' (which does not receive any data) returns AcceptedResult")]
        public void Should_Get_AllDummies()
        {
            var expectedValue = new List<ReadedDummyViewModel>();
            expectedValue.Add(new ReadedDummyViewModel
            {
                Id = Guid.NewGuid(),
                Name = "TestDeclineReason",
                Description = "TestDeclineReason",
                TestValue = ""
            });

            mockService.Setup(_ => _.List()).Returns(new[] { new ReadedDummyContract() });
            mockMapper.Setup(_ => _.Map<List<ReadedDummyViewModel>>(It.IsAny<IEnumerable<ReadedDummyContract>>())).Returns(expectedValue);

            var result = controller.Get();

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as List<ReadedDummyViewModel>;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.List(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedDummyViewModel>>(It.IsAny<IEnumerable<ReadedDummyContract>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns AcceptedResult when data is valid")]
        public void Should_Get_DummyById_WhenDataIsValid()
        {
            var dummyId = Guid.NewGuid();
            var expectedValue = new ReadedDummyViewModel
            {
                Id = dummyId,
                Name = "Test",
                Description = "Test",
                TestValue = ""
            };

            mockService.Setup(_ => _.Read(It.IsAny<Guid>())).Returns(new ReadedDummyContract());
            mockMapper.Setup(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>())).Returns(expectedValue);

            var result = controller.Get(dummyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);

            var resultData = (result as AcceptedResult).Value as ReadedDummyViewModel;
            var resultDataAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultDataAsJson);
            mockService.Verify(_ => _.Read(It.IsAny<Guid>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Get' (which receives an id) returns NotFoundObjectResult when data is invalid")]
        public void Should_Get_DummyById_WhenDataIsInvalid()
        {
            var dummyId = Guid.NewGuid();
            var expectedValue = dummyId;

            var result = controller.Get(dummyId);

            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedValue, (result as NotFoundObjectResult).Value);
            mockService.Verify(_ => _.Read(It.IsAny<Guid>()), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDummyViewModel>(It.IsAny<ReadedDummyContract>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Post' returns CreatedResult")]
        public void Should_Post_Dummy()
        {
            var dummyVM = new CreateDummyViewModel
            {
                Name = "Test",
                Description = "Test",
                TestValue = ""
            };
            var expectedValue = new CreatedDummyViewModel
            {
                Id = Guid.NewGuid()
            };

            mockMapper.Setup(_ => _.Map<CreateDummyContract>(It.IsAny<CreateDummyViewModel>())).Returns(new CreateDummyContract());
            mockMapper.Setup(_ => _.Map<CreatedDummyViewModel>(It.IsAny<CreatedDummyContract>())).Returns(expectedValue);
            mockService.Setup(_ => _.Create(It.IsAny<CreateDummyContract>())).Returns(new CreatedDummyContract());

            var result = controller.Post(dummyVM);

            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);

            var resultData = (result as CreatedResult).Value as CreatedDummyViewModel;
            var resultAsJson = JsonConvert.SerializeObject(resultData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Create(It.IsAny<CreateDummyContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedDummyViewModel>(It.IsAny<CreatedDummyContract>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Put' returns AcceptedResult")]
        public void Should_Put_Dummy()
        {
            var dummyId = Guid.NewGuid();
            var dummyToUpdate = new UpdateDummyViewModel();
            var expectedStatusCode = 202;

            mockMapper.Setup(_ => _.Map<UpdateDummyContract>(It.IsAny<UpdateDummyViewModel>())).Returns(new UpdateDummyContract());
            mockService.Setup(_ => _.Update(It.IsAny<UpdateDummyContract>()));

            var result = controller.Put(dummyId, dummyToUpdate);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            Assert.Equal(expectedStatusCode, (result as AcceptedResult).StatusCode);
            mockService.Verify(_ => _.Update(It.IsAny<UpdateDummyContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<UpdateDummyContract>(It.IsAny<UpdateDummyViewModel>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Delete' returns AcceptedResult")]
        public void Should_Delete_Dummy()
        {
            var dummyId = Guid.NewGuid();

            mockService.Setup(_ => _.Delete(It.IsAny<Guid>()));

            var result = controller.Delete(dummyId);

            Assert.NotNull(result);
            Assert.IsType<AcceptedResult>(result);
            mockService.Verify(_ => _.Delete(It.IsAny<Guid>()), Times.Once);
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
